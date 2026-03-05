using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class PrescriptionService
{
    private readonly ClinicDbContext _db;
    public PrescriptionService(ClinicDbContext db) => _db = db;

    // ─── helpers ──────────────────────────────────────────────────────────────
    private static PrescriptionResponse ToResponse(Prescription p) => new(
        p.Id,
        p.VisitId,
        p.Note,
        p.CreatedAt,
        p.Items.Select(i => new PrescriptionItemResponse(
            i.Id,
            i.MedicineId,
            i.Medicine.Code,
            i.Medicine.Name,
            i.Medicine.DosageForm,
            i.Medicine.Unit,
            i.Quantity,
            i.DosageInstruction,
            i.Note
        )).ToList()
    );

    private async Task<Prescription> LoadFullAsync(long prescriptionId) =>
        await _db.Prescriptions
            .Include(p => p.Items)
                .ThenInclude(i => i.Medicine)
            .FirstOrDefaultAsync(p => p.Id == prescriptionId)
        ?? throw new ResourceNotFoundException($"Đơn thuốc không tồn tại: {prescriptionId}");

    // ─── GET by visit ─────────────────────────────────────────────────────────
    public async Task<PrescriptionResponse?> GetByVisitAsync(long visitId)
    {
        var p = await _db.Prescriptions
            .Include(p => p.Items).ThenInclude(i => i.Medicine)
            .FirstOrDefaultAsync(p => p.VisitId == visitId);

        return p is null ? null : ToResponse(p);
    }

    // ─── CREATE ───────────────────────────────────────────────────────────────
    public async Task<PrescriptionResponse> CreateAsync(long visitId, PrescriptionCreateRequest req)
    {
        var visitExists = await _db.Visits.AnyAsync(v => v.Id == visitId);
        if (!visitExists)
            throw new ResourceNotFoundException($"Lần khám không tồn tại: {visitId}");

        if (await _db.Prescriptions.AnyAsync(p => p.VisitId == visitId))
            throw new InvalidOperationException("Lần khám này đã có đơn thuốc. Dùng PUT để cập nhật.");

        var p = new Prescription { VisitId = visitId, Note = req.Note };
        _db.Prescriptions.Add(p);
        await _db.SaveChangesAsync();

        return ToResponse(await LoadFullAsync(p.Id));
    }

    // ─── UPDATE note ──────────────────────────────────────────────────────────
    public async Task<PrescriptionResponse> UpdateAsync(long visitId, PrescriptionUpdateRequest req)
    {
        var p = await _db.Prescriptions
            .FirstOrDefaultAsync(p => p.VisitId == visitId)
            ?? throw new ResourceNotFoundException($"Lần khám {visitId} chưa có đơn thuốc.");

        p.Note = req.Note;
        await _db.SaveChangesAsync();

        return ToResponse(await LoadFullAsync(p.Id));
    }

    // ─── DELETE ───────────────────────────────────────────────────────────────
    public async Task DeleteAsync(long visitId)
    {
        var p = await _db.Prescriptions.FirstOrDefaultAsync(p => p.VisitId == visitId)
            ?? throw new ResourceNotFoundException($"Lần khám {visitId} chưa có đơn thuốc.");
        _db.Prescriptions.Remove(p);
        await _db.SaveChangesAsync();
    }

    // ─── ADD item ─────────────────────────────────────────────────────────────
    public async Task<PrescriptionResponse> AddItemAsync(long visitId, PrescriptionItemAddRequest req)
    {
        var p = await _db.Prescriptions.FirstOrDefaultAsync(p => p.VisitId == visitId)
            ?? throw new ResourceNotFoundException($"Lần khám {visitId} chưa có đơn thuốc.");

        var medicine = await _db.Medicines.FindAsync(req.MedicineId)
            ?? throw new ResourceNotFoundException($"Thuốc không tồn tại: {req.MedicineId}");

        if (!medicine.IsActive)
            throw new InvalidOperationException("Thuốc này đã ngừng sử dụng.");

        var duplicate = await _db.PrescriptionItems
            .AnyAsync(i => i.PrescriptionId == p.Id && i.MedicineId == req.MedicineId);
        if (duplicate)
            throw new InvalidOperationException($"Thuốc {medicine.Name} đã có trong đơn. Dùng PUT để cập nhật số lượng.");

        var item = new PrescriptionItem
        {
            PrescriptionId    = p.Id,
            MedicineId        = req.MedicineId,
            Quantity          = req.Quantity,
            DosageInstruction = req.DosageInstruction,
            Note              = req.Note
        };
        _db.PrescriptionItems.Add(item);
        await _db.SaveChangesAsync();

        return ToResponse(await LoadFullAsync(p.Id));
    }

    // ─── UPDATE item ──────────────────────────────────────────────────────────
    public async Task<PrescriptionResponse> UpdateItemAsync(long visitId, long itemId, PrescriptionItemUpdateRequest req)
    {
        var p = await _db.Prescriptions.FirstOrDefaultAsync(p => p.VisitId == visitId)
            ?? throw new ResourceNotFoundException($"Lần khám {visitId} chưa có đơn thuốc.");

        var item = await _db.PrescriptionItems.FirstOrDefaultAsync(i => i.Id == itemId && i.PrescriptionId == p.Id)
            ?? throw new ResourceNotFoundException($"Mục đơn thuốc {itemId} không thuộc đơn này.");

        item.Quantity          = req.Quantity;
        item.DosageInstruction = req.DosageInstruction;
        item.Note              = req.Note;
        await _db.SaveChangesAsync();

        return ToResponse(await LoadFullAsync(p.Id));
    }

    // ─── REMOVE item ──────────────────────────────────────────────────────────
    public async Task<PrescriptionResponse> RemoveItemAsync(long visitId, long itemId)
    {
        var p = await _db.Prescriptions.FirstOrDefaultAsync(p => p.VisitId == visitId)
            ?? throw new ResourceNotFoundException($"Lần khám {visitId} chưa có đơn thuốc.");

        var item = await _db.PrescriptionItems.FirstOrDefaultAsync(i => i.Id == itemId && i.PrescriptionId == p.Id)
            ?? throw new ResourceNotFoundException($"Mục đơn thuốc {itemId} không thuộc đơn này.");

        _db.PrescriptionItems.Remove(item);
        await _db.SaveChangesAsync();

        return ToResponse(await LoadFullAsync(p.Id));
    }
}
