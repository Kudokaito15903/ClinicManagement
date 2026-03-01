using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class PatientService
{
    private readonly ClinicDbContext _db;

    public PatientService(ClinicDbContext db)
    {
        _db = db;
    }

    public async Task<List<PatientResponse>> FindAllAsync(string? keyword)
    {
        var query = _db.Patients
            .Where(p => !p.Deleted);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.ToLower();
            query = query.Where(p =>
                p.FullName.ToLower().Contains(kw) ||
                p.Code.ToLower().Contains(kw));
        }

        var patients = await query.ToListAsync();
        return patients.Select(ToResponse).ToList();
    }

    public async Task<PatientResponse> FindByIdAsync(long id)
        => ToResponse(await GetByIdAsync(id));

    public async Task<PatientResponse> CreateAsync(PatientCreateRequest req)
    {
        string code = string.IsNullOrWhiteSpace(req.Code) ? await GenerateCodeAsync() : req.Code;

        if (!string.IsNullOrWhiteSpace(req.Code))
        {
            if (await _db.Patients.AnyAsync(p => p.Code == req.Code))
                throw new BadRequestException($"Ma benh nhan da ton tai: {req.Code}");
        }

        var patient = new Patient
        {
            Code = code,
            FullName = req.FullName,
            BirthYear = req.BirthYear,
            Gender = req.Gender,
            Address = req.Address
        };

        _db.Patients.Add(patient);
        await _db.SaveChangesAsync();

        return ToResponse(patient);
    }

    public async Task<PatientResponse> UpdateAsync(long id, PatientUpdateRequest req)
    {
        var patient = await GetByIdAsync(id);
        patient.FullName = req.FullName;
        patient.BirthYear = req.BirthYear;
        patient.Gender = req.Gender;
        patient.Address = req.Address;

        await _db.SaveChangesAsync();
        return ToResponse(patient);
    }

    public async Task DeleteAsync(long id)
    {
        var patient = await GetByIdAsync(id);
        patient.Deleted = true;
        await _db.SaveChangesAsync();
    }

    public async Task<Patient> GetByIdAsync(long id)
        => await _db.Patients
            .FirstOrDefaultAsync(p => p.Id == id && !p.Deleted)
           ?? throw new ResourceNotFoundException($"Benh nhan khong ton tai: {id}");

    private async Task<string> GenerateCodeAsync()
    {
        long count = await _db.Patients.CountAsync() + 1;
        return $"BN{count:D5}";
    }

    public PatientResponse ToResponse(Patient p) => new(
        p.Id, p.Code, p.FullName, p.BirthYear, p.Gender, p.Address,
        null, null, null
    );
}
