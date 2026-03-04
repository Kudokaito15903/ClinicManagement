using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class PaymentService
{
    private readonly ClinicDbContext _db;

    public PaymentService(ClinicDbContext db) => _db = db;

    public async Task<PaymentResponse> CreateAsync(long visitId, PaymentCreateRequest req)
    {
        // Validate visit exists (include Doctor for fee lookup)
        var visit = await _db.Visits
            .Include(v => v.VisitServices)
            .Include(v => v.Doctor)
            .FirstOrDefaultAsync(v => v.Id == visitId)
            ?? throw new ResourceNotFoundException($"Lan kham khong ton tai: {visitId}");

        // Check if payment already exists
        if (await _db.Payments.AnyAsync(p => p.VisitId == visitId))
            throw new BadRequestException("Lan kham nay da duoc thanh toan.");

        // Resolve ExaminationFee: use override if provided, else lookup from SystemConfig
        decimal examinationFee;
        if (req.ExaminationFee.HasValue)
        {
            examinationFee = req.ExaminationFee.Value;
        }
        else
        {
            var configKey = visit.Doctor.AcademicTitle switch
            {
                AcademicTitle.Professor           => "fee_professor",
                AcademicTitle.AssociateProfessor  => "fee_associate_professor",
                AcademicTitle.PhD_CKII            => "fee_phd_ckii",
                AcademicTitle.Master_CKI          => "fee_master_cki",
                _                                 => "examination_fee"
            };
            var config = await _db.SystemConfigs.FindAsync(configKey)
                ?? await _db.SystemConfigs.FindAsync("examination_fee");
            examinationFee = config != null && decimal.TryParse(config.ConfigValue, out var fee)
                ? fee : 100000m;
        }

        var serviceTotal = visit.VisitServices.Sum(vs => vs.UnitPrice * vs.Quantity);
        var grandTotal   = examinationFee + serviceTotal;
        var finalAmount  = grandTotal - req.Discount;

        var payment = new Payment
        {
            VisitId        = visitId,
            ExaminationFee = examinationFee,
            ServiceTotal   = serviceTotal,
            Discount       = req.Discount,
            FinalAmount    = finalAmount,
            PaymentMethod  = req.PaymentMethod,
            PaidAt         = DateTime.UtcNow,
            CashierNote    = req.CashierNote
        };

        _db.Payments.Add(payment);

        // Auto-update visit status to Paid
        visit.Status = VisitStatus.Paid;

        await _db.SaveChangesAsync();
        return ToResponse(payment);
    }

    public async Task<PaymentResponse> GetByVisitIdAsync(long visitId)
    {
        var payment = await _db.Payments.FirstOrDefaultAsync(p => p.VisitId == visitId)
            ?? throw new ResourceNotFoundException($"Lan kham {visitId} chua co thanh toan.");

        return ToResponse(payment);
    }

    public static PaymentResponse ToResponse(Payment p)
    {
        var grandTotal = p.FinalAmount + p.Discount;
        return new PaymentResponse(
            p.Id,
            p.VisitId,
            p.ExaminationFee,
            p.ServiceTotal,
            grandTotal,
            p.Discount,
            p.FinalAmount,
            p.PaymentMethod.ToString(),
            p.PaidAt,
            p.CashierNote,
            p.CreatedAt
        );
    }
}
