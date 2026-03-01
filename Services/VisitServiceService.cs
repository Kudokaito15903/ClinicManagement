using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class VisitServiceService
{
    private readonly ClinicDbContext _db;
    private readonly MedicalServiceService _medicalServiceService;

    public VisitServiceService(ClinicDbContext db, MedicalServiceService medicalServiceService)
    {
        _db = db;
        _medicalServiceService = medicalServiceService;
    }

    public async Task<VisitServiceResponse> AddServiceAsync(long visitId, VisitServiceAddRequest req)
    {
        var visit = await _db.Visits.FindAsync(visitId)
                    ?? throw new ResourceNotFoundException($"Lan kham khong ton tai: {visitId}");

        var svc = await _medicalServiceService.GetByIdAsync(req.ServiceId);

        var vsItem = new VisitServiceItem
        {
            VisitId = visitId,
            ServiceId = svc.Id,
            UnitPrice = svc.Price,
            Quantity = req.Quantity
        };

        _db.VisitServices.Add(vsItem);
        await _db.SaveChangesAsync();

        // Reload with service
        vsItem = await _db.VisitServices
            .Include(v => v.Service)
            .FirstAsync(v => v.Id == vsItem.Id);

        return ToResponse(vsItem);
    }

    public async Task<List<VisitServiceResponse>> FindByVisitIdAsync(long visitId)
    {
        var items = await _db.VisitServices
            .Include(vs => vs.Service)
            .Where(vs => vs.VisitId == visitId)
            .ToListAsync();
        return items.Select(ToResponse).ToList();
    }

    public async Task RemoveServiceAsync(long visitId, long vsId)
    {
        var vs = await _db.VisitServices.FindAsync(vsId)
                 ?? throw new ResourceNotFoundException($"Dich vu chi dinh khong ton tai: {vsId}");

        if (vs.VisitId != visitId)
            throw new ResourceNotFoundException($"Dich vu nay khong thuoc lan kham: {visitId}");

        _db.VisitServices.Remove(vs);
        await _db.SaveChangesAsync();
    }

    public VisitServiceResponse ToResponse(VisitServiceItem vs)
    {
        var subtotal = vs.UnitPrice * vs.Quantity;
        return new VisitServiceResponse(
            vs.Id,
            vs.ServiceId,
            vs.Service?.Name ?? "",
            vs.UnitPrice,
            vs.Quantity,
            subtotal,
            vs.CreatedAt
        );
    }
}
