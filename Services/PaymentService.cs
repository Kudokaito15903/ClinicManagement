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
        // Validate visit exists
        var visit = await _db.Visits
            .Include(v => v.VisitServices)
            .FirstOrDefaultAsync(v => v.Id == visitId)
            ?? throw new ResourceNotFoundException($"Lan kham khong ton tai: {visitId}");

        // Check if payment already exists
        if (await _db.Payments.AnyAsync(p => p.VisitId == visitId))
            throw new InvalidOperationException("Lan kham nay da duoc thanh toan.");

        var serviceTotal = visit.VisitServices.Sum(vs => vs.UnitPrice * vs.Quantity);
        var grandTotal   = req.ExaminationFee + serviceTotal;
        var finalAmount  = grandTotal - req.Discount;

        var payment = new Payment
        {
            VisitId        = visitId,
            ExaminationFee = req.ExaminationFee,
            ServiceTotal   = serviceTotal,
            GrandTotal     = grandTotal,
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

    public static PaymentResponse ToResponse(Payment p) => new(
        p.Id,
        p.VisitId,
        p.ExaminationFee,
        p.ServiceTotal,
        p.GrandTotal,
        p.Discount,
        p.FinalAmount,
        p.PaymentMethod.ToString(),
        p.PaidAt,
        p.CashierNote,
        p.CreatedAt
    );
}
