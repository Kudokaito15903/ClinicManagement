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
        var doctor = new Doctor { FullName = req.FullName, Specialty = req.Specialty };
        _db.Doctors.Add(doctor);
        await _db.SaveChangesAsync();
        return ToResponse(doctor);
    }

    public async Task<DoctorResponse> UpdateAsync(long id, DoctorRequest req)
    {
        var doctor = await GetByIdAsync(id);
        doctor.FullName = req.FullName;
        doctor.Specialty = req.Specialty;
        await _db.SaveChangesAsync();
        return ToResponse(doctor);
    }

    public async Task DeleteAsync(long id)
    {
        var doctor = await GetByIdAsync(id);
        _db.Doctors.Remove(doctor);
        await _db.SaveChangesAsync();
    }

    public async Task<Doctor> GetByIdAsync(long id)
        => await _db.Doctors.FindAsync(id)
           ?? throw new ResourceNotFoundException($"Bac si khong ton tai: {id}");

    public static DoctorResponse ToResponse(Doctor d)
        => new(d.Id, d.Code, d.FullName, d.Specialty, d.Phone, d.Email, d.IsActive);
}
