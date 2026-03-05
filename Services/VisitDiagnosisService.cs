using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class VisitDiagnosisService
{
    private readonly ClinicDbContext _db;

    public VisitDiagnosisService(ClinicDbContext db) => _db = db;

    public async Task<List<VisitDiagnosisResponse>> GetByVisitIdAsync(long visitId)
    {
        var list = await _db.VisitDiagnoses
            .Include(vd => vd.Diagnosis)
            .Where(vd => vd.VisitId == visitId)
            .OrderByDescending(vd => vd.IsPrimary)
            .ToListAsync();

        return list.Select(vd => new VisitDiagnosisResponse(
            vd.Diagnosis.Id, vd.Diagnosis.IcdCode, vd.Diagnosis.Name,
            vd.Diagnosis.Category, vd.Diagnosis.Description,
            vd.IsPrimary, vd.Note)).ToList();
    }

    public async Task<VisitDiagnosisResponse> AddAsync(long visitId, VisitDiagnosisAddRequest req)
    {
        // Validate visit exists
        var visitExists = await _db.Visits.AnyAsync(v => v.Id == visitId);
        if (!visitExists)
            throw new ResourceNotFoundException($"Lan kham khong ton tai: {visitId}");

        // Validate diagnosis exists
        var diagnosis = await _db.Diagnoses.FindAsync(req.DiagnosisId)
            ?? throw new ResourceNotFoundException($"Chan doan khong ton tai: {req.DiagnosisId}");

        // Check duplicate
        var exists = await _db.VisitDiagnoses
            .AnyAsync(vd => vd.VisitId == visitId && vd.DiagnosisId == req.DiagnosisId);
        if (exists)
            throw new InvalidOperationException("Chan doan nay da duoc chi dinh cho lan kham.");

        // If this is primary, unset current primary
        if (req.IsPrimary)
        {
            var currentPrimary = await _db.VisitDiagnoses
                .FirstOrDefaultAsync(vd => vd.VisitId == visitId && vd.IsPrimary);
            if (currentPrimary != null)
                currentPrimary.IsPrimary = false;
        }

        var vd = new VisitDiagnosis
        {
            VisitId = visitId,
            DiagnosisId = req.DiagnosisId,
            IsPrimary = req.IsPrimary,
            Note = req.Note
        };

        _db.VisitDiagnoses.Add(vd);
        await _db.SaveChangesAsync();

        return new VisitDiagnosisResponse(
            diagnosis.Id, diagnosis.IcdCode, diagnosis.Name,
            diagnosis.Category, diagnosis.Description,
            vd.IsPrimary, vd.Note);
    }

    public async Task RemoveAsync(long visitId, long diagnosisId)
    {
        var vd = await _db.VisitDiagnoses
            .FirstOrDefaultAsync(vd => vd.VisitId == visitId && vd.DiagnosisId == diagnosisId)
            ?? throw new ResourceNotFoundException($"Chan doan {diagnosisId} khong thuoc lan kham {visitId}.");

        _db.VisitDiagnoses.Remove(vd);
        await _db.SaveChangesAsync();
    }
}
