using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class DoctorService
{
    private readonly ClinicDbContext _db;
    public DoctorService(ClinicDbContext db) => _db = db;

    public async Task<List<DoctorResponse>> FindAllAsync()
        => await _db.Doctors.Select(d => ToResponse(d)).ToListAsync();

    public async Task<DoctorResponse> FindByIdAsync(long id)
        => ToResponse(await GetByIdAsync(id));

    public async Task<DoctorResponse> CreateAsync(DoctorRequest req)
    {
        var doctor = new Doctor
        {
            Code          = await GenerateCodeAsync(),
            FullName      = req.FullName,
            Specialty     = req.Specialty,
            AcademicTitle = req.AcademicTitle ?? AcademicTitle.None,
            Phone         = req.Phone,
            Email         = req.Email
        };
        _db.Doctors.Add(doctor);
        await _db.SaveChangesAsync();
        return ToResponse(doctor);
    }

    private async Task<string> GenerateCodeAsync()
    {
        long count = await _db.Doctors.LongCountAsync();
        string candidate;
        do
        {
            count++;
            candidate = $"BS{count:D3}";
        } while (await _db.Doctors.AnyAsync(d => d.Code == candidate));
        return candidate;
    }

    public async Task<DoctorResponse> UpdateAsync(long id, DoctorRequest req)
    {
        var doctor = await GetByIdAsync(id);
        doctor.FullName      = req.FullName;
        doctor.Specialty     = req.Specialty;
        doctor.AcademicTitle = req.AcademicTitle ?? doctor.AcademicTitle;
        doctor.Phone         = req.Phone;
        doctor.Email         = req.Email;
        await _db.SaveChangesAsync();
        return ToResponse(doctor);
    }

    public async Task DeleteAsync(long id)
    {
        var doctor = await GetByIdAsync(id);
        doctor.IsActive = false;          // soft-delete: giữ lại lịch sử khám
        await _db.SaveChangesAsync();
    }

    public async Task<Doctor> GetByIdAsync(long id)
        => await _db.Doctors.FindAsync(id)
           ?? throw new ResourceNotFoundException($"Bac si khong ton tai: {id}");

    public static DoctorResponse ToResponse(Doctor d)
        => new(d.Id, d.Code, d.FullName, d.Specialty, d.AcademicTitle, d.Phone, d.Email, d.IsActive);
}
