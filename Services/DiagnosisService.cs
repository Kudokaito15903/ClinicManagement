using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class DiagnosisService
{
    private readonly ClinicDbContext _db;
    public DiagnosisService(ClinicDbContext db) => _db = db;

    public async Task<List<DiagnosisResponse>> FindAllAsync(string? keyword)
    {
        var query = _db.Diagnoses.AsQueryable();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.ToLower();
            query = query.Where(d => d.Name.ToLower().Contains(kw) || d.Code.ToLower().Contains(kw));
        }
        return await query.Select(d => ToResponse(d)).ToListAsync();
    }

    public async Task<DiagnosisResponse> FindByIdAsync(long id)
        => ToResponse(await GetByIdAsync(id));

    public async Task<DiagnosisResponse> CreateAsync(DiagnosisRequest req)
    {
        var diag = new Diagnosis { Code = req.Code, Name = req.Name, Description = req.Description };
        _db.Diagnoses.Add(diag);
        await _db.SaveChangesAsync();
        return ToResponse(diag);
    }

    public async Task<DiagnosisResponse> UpdateAsync(long id, DiagnosisRequest req)
    {
        var diag = await GetByIdAsync(id);
        diag.Code = req.Code;
        diag.Name = req.Name;
        diag.Description = req.Description;
        await _db.SaveChangesAsync();
        return ToResponse(diag);
    }

    public async Task DeleteAsync(long id)
    {
        var diag = await GetByIdAsync(id);
        _db.Diagnoses.Remove(diag);
        await _db.SaveChangesAsync();
    }

    public async Task<Diagnosis> GetByIdAsync(long id)
        => await _db.Diagnoses.FindAsync(id)
           ?? throw new ResourceNotFoundException($"Chan doan khong ton tai: {id}");

    public static DiagnosisResponse ToResponse(Diagnosis d)
        => new(d.Id, d.Code, d.Name, d.Description);
}
