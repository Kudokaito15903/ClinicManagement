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
            .Where(p => !p.IsDeleted);

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
        string code = await GenerateCodeAsync();

        var patient = new Patient
        {
            Code = code,
            FullName = req.FullName,
            DateOfBirth = req.DateOfBirth,
            Gender = req.Gender,
            Phone = req.Phone,
            Address = req.Address,
            Note = req.Note
        };

        _db.Patients.Add(patient);
        await _db.SaveChangesAsync();

        return ToResponse(patient);
    }

    public async Task<PatientResponse> UpdateAsync(long id, PatientUpdateRequest req)
    {
        var patient = await GetByIdAsync(id);
        patient.FullName = req.FullName;
        patient.DateOfBirth = req.DateOfBirth;
        patient.Gender = req.Gender;
        patient.Phone = req.Phone;
        patient.Address = req.Address;
        patient.Note = req.Note;

        await _db.SaveChangesAsync();
        return ToResponse(patient);
    }

    public async Task DeleteAsync(long id)
    {
        var patient = await GetByIdAsync(id);
        patient.IsDeleted = true;
        await _db.SaveChangesAsync();
    }

    public async Task<Patient> GetByIdAsync(long id)
        => await _db.Patients
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
           ?? throw new ResourceNotFoundException($"Benh nhan khong ton tai: {id}");

    private async Task<string> GenerateCodeAsync()
    {
        long maxId = await _db.Patients.AnyAsync()
            ? await _db.Patients.MaxAsync(p => p.Id)
            : 0;
        string candidate;
        do
        {
            maxId++;
            candidate = $"BN{maxId:D5}";
        } while (await _db.Patients.AnyAsync(p => p.Code == candidate));

        return candidate;
    }

    public PatientResponse ToResponse(Patient p) => new(
        p.Id, p.Code, p.FullName, p.DateOfBirth, p.Gender, p.Phone, p.Address, p.Note,
        null, null, null
    );
}
