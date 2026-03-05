using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class MedicalServiceService
{
    private readonly ClinicDbContext _db;
    public MedicalServiceService(ClinicDbContext db) => _db = db;

    public async Task<List<MedicalServiceResponse>> FindAllAsync()
        => await _db.MedicalServices.Select(ms => ToResponse(ms)).ToListAsync();

    public async Task<MedicalServiceResponse> FindByIdAsync(long id)
        => ToResponse(await GetByIdAsync(id));

    public async Task<MedicalServiceResponse> CreateAsync(MedicalServiceRequest req)
    {
        var ms = new MedicalService { Code = req.Code, Name = req.Name, Price = req.Price };
        _db.MedicalServices.Add(ms);
        await _db.SaveChangesAsync();
        return ToResponse(ms);
    }

    public async Task<MedicalServiceResponse> UpdateAsync(long id, MedicalServiceRequest req)
    {
        var ms = await GetByIdAsync(id);
        ms.Code = req.Code;
        ms.Name = req.Name;
        ms.Price = req.Price;
        await _db.SaveChangesAsync();
        return ToResponse(ms);
    }

    public async Task DeleteAsync(long id)
    {
        var ms = await GetByIdAsync(id);
        _db.MedicalServices.Remove(ms);
        await _db.SaveChangesAsync();
    }

    public async Task<MedicalService> GetByIdAsync(long id)
        => await _db.MedicalServices.FindAsync(id)
           ?? throw new ResourceNotFoundException($"Dich vu y te khong ton tai: {id}");

    public static MedicalServiceResponse ToResponse(MedicalService ms)
        => new(ms.Id, ms.Code, ms.Name, ms.Unit, ms.Price, ms.Category, ms.IsActive);
}
