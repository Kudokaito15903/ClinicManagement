using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class MedicineService
{
    private readonly ClinicDbContext _db;
    public MedicineService(ClinicDbContext db) => _db = db;

    // ─── helpers ──────────────────────────────────────────────────────────────
    private static MedicineResponse ToResponse(Medicine m) => new(
        m.Id, m.Code, m.Name, m.Ingredient, m.DosageForm, m.Unit,
        m.Manufacturer, m.CountryOfOrigin, m.UnitPrice, m.IsActive, m.CreatedAt);

    // ─── GET list ─────────────────────────────────────────────────────────────
    public async Task<List<MedicineResponse>> GetAllAsync(string? search, bool? activeOnly)
    {
        var q = _db.Medicines.AsQueryable();

        if (activeOnly == true)
            q = q.Where(m => m.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            q = q.Where(m => m.Name.ToLower().Contains(s)
                          || m.Code.ToLower().Contains(s)
                          || (m.Ingredient != null && m.Ingredient.ToLower().Contains(s)));
        }

        return (await q.OrderBy(m => m.Code).ToListAsync()).Select(ToResponse).ToList();
    }

    // ─── GET by id ────────────────────────────────────────────────────────────
    public async Task<MedicineResponse> GetByIdAsync(long id)
    {
        var m = await _db.Medicines.FindAsync(id)
            ?? throw new ResourceNotFoundException($"Thuốc không tồn tại: {id}");
        return ToResponse(m);
    }

    // ─── CREATE ───────────────────────────────────────────────────────────────
    public async Task<MedicineResponse> CreateAsync(MedicineCreateRequest req)
    {
        if (await _db.Medicines.AnyAsync(m => m.Code == req.Code))
            throw new InvalidOperationException($"Mã thuốc đã tồn tại: {req.Code}");

        var m = new Medicine
        {
            Code            = req.Code.Trim().ToUpper(),
            Name            = req.Name,
            Unit            = req.Unit,
            UnitPrice       = req.UnitPrice,
            Ingredient      = req.Ingredient,
            DosageForm      = req.DosageForm,
            Manufacturer    = req.Manufacturer,
            CountryOfOrigin = req.CountryOfOrigin,
            IsActive        = true
        };

        _db.Medicines.Add(m);
        await _db.SaveChangesAsync();
        return ToResponse(m);
    }

    // ─── UPDATE ───────────────────────────────────────────────────────────────
    public async Task<MedicineResponse> UpdateAsync(long id, MedicineUpdateRequest req)
    {
        var m = await _db.Medicines.FindAsync(id)
            ?? throw new ResourceNotFoundException($"Thuốc không tồn tại: {id}");

        m.Name            = req.Name;
        m.Unit            = req.Unit;
        m.UnitPrice       = req.UnitPrice;
        m.Ingredient      = req.Ingredient;
        m.DosageForm      = req.DosageForm;
        m.Manufacturer    = req.Manufacturer;
        m.CountryOfOrigin = req.CountryOfOrigin;

        await _db.SaveChangesAsync();
        return ToResponse(m);
    }

    // ─── TOGGLE ACTIVE ────────────────────────────────────────────────────────
    public async Task<MedicineResponse> SetActiveAsync(long id, bool isActive)
    {
        var m = await _db.Medicines.FindAsync(id)
            ?? throw new ResourceNotFoundException($"Thuốc không tồn tại: {id}");
        m.IsActive = isActive;
        await _db.SaveChangesAsync();
        return ToResponse(m);
    }
}
