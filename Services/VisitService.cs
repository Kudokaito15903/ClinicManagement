using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class VisitService
{
    private readonly ClinicDbContext _db;
    private readonly PatientService _patientService;

    public VisitService(ClinicDbContext db, PatientService patientService)
    {
        _db = db;
        _patientService = patientService;
    }

    /// <summary>Danh sách lượt khám — lọc theo ngày, trạng thái, bác sĩ</summary>
    public async Task<List<VisitListItemResponse>> GetAllAsync(
        DateOnly? date, VisitStatus? status, long? doctorId)
    {
        var query = _db.Visits
            .Include(v => v.Patient)
            .Include(v => v.Doctor)
            .Include(v => v.Room)
            .AsQueryable();

        if (date.HasValue)
        {
            var start = date.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            var end   = date.Value.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);
            query = query.Where(v => v.VisitDate >= start && v.VisitDate <= end);
        }
        if (status.HasValue)
            query = query.Where(v => v.Status == status.Value);
        if (doctorId.HasValue)
            query = query.Where(v => v.DoctorId == doctorId.Value);

        var visits = await query.OrderByDescending(v => v.VisitDate).ToListAsync();

        return visits.Select(v => new VisitListItemResponse(
            v.Id,
            v.Code,
            v.Patient?.FullName ?? "",
            v.Doctor?.FullName,
            v.Room?.Name,
            v.VisitDate,
            v.Status.ToString()
        )).ToList();
    }

    public async Task<VisitResponse> CreateAsync(VisitCreateRequest req)
    {
        await _patientService.GetByIdAsync(req.PatientId);

        var visit = new Visit
        {
            Code      = $"VIS{DateTime.UtcNow:yyyyMMdd}{new Random().Next(1000, 9999)}",
            PatientId = req.PatientId,
            DoctorId  = req.DoctorId,
            RoomId    = req.RoomId,
            VisitDate = DateTime.UtcNow,
            Reason    = req.Reason
        };

        _db.Visits.Add(visit);
        await _db.SaveChangesAsync();

        return await ToResponseAsync(await GetByIdRawAsync(visit.Id));
    }

    public async Task<VisitResponse> FindByIdAsync(long id)
        => await ToResponseAsync(await GetByIdRawAsync(id));

    public async Task<VisitResponse> UpdateAsync(long id, VisitUpdateRequest req)
    {
        var visit = await GetByIdRawAsync(id);
        if (req.DoctorId.HasValue)  visit.DoctorId   = req.DoctorId.Value;
        if (req.RoomId.HasValue)    visit.RoomId      = req.RoomId.Value;
        if (req.Reason != null)     visit.Reason      = req.Reason;
        if (req.Conclusion != null) visit.Conclusion  = req.Conclusion;
        if (req.Status.HasValue)    visit.Status      = req.Status.Value;

        await _db.SaveChangesAsync();
        return await ToResponseAsync(await GetByIdRawAsync(id));
    }

    public async Task<VisitResponse> UpdateStatusAsync(long id, VisitStatus status)
    {
        var visit = await GetByIdRawAsync(id);
        visit.Status = status;
        await _db.SaveChangesAsync();
        return await ToResponseAsync(await GetByIdRawAsync(id));
    }

    public async Task<List<VisitSummaryResponse>> GetPatientHistoryAsync(long patientId)
    {
        await _patientService.GetByIdAsync(patientId);

        var visits = await _db.Visits
            .Include(v => v.Doctor)
            .Include(v => v.Room)
            .Include(v => v.VisitDiagnoses).ThenInclude(vd => vd.Diagnosis)
            .Where(v => v.PatientId == patientId)
            .OrderByDescending(v => v.VisitDate)
            .ToListAsync();

        var result = new List<VisitSummaryResponse>();
        foreach (var v in visits)
        {
            var vsList = await _db.VisitServices
                .Include(vs => vs.Service)
                .Where(vs => vs.VisitId == v.Id)
                .ToListAsync();

            var serviceTotal = vsList.Sum(vs => vs.UnitPrice * vs.Quantity);
            var primaryDiag  = v.VisitDiagnoses.FirstOrDefault(vd => vd.IsPrimary)?.Diagnosis
                ?? v.VisitDiagnoses.FirstOrDefault()?.Diagnosis;

            result.Add(new VisitSummaryResponse(
                v.Id,
                v.VisitDate,
                v.Doctor != null ? new RefResponse(v.Doctor.Id, v.Doctor.FullName) : null,
                v.Room   != null ? new RefResponse(v.Room.Id,   v.Room.Name)   : null,
                primaryDiag != null ? new RefResponse(primaryDiag.Id, primaryDiag.Name) : null,
                serviceTotal,
                v.Reason
            ));
        }

        return result;
    }

    public async Task<Visit> GetByIdRawAsync(long id)
        => await _db.Visits
            .Include(v => v.Patient)
            .Include(v => v.Doctor)
            .Include(v => v.Room)
            .Include(v => v.Payment)
            .Include(v => v.VisitDiagnoses).ThenInclude(vd => vd.Diagnosis)
            .FirstOrDefaultAsync(v => v.Id == id)
           ?? throw new ResourceNotFoundException($"Lan kham khong ton tai: {id}");

    public async Task<VisitResponse> ToResponseAsync(Visit v) => new(
        v.Id,
        v.Code,
        v.Patient != null ? _patientService.ToResponse(v.Patient) : null,
        v.Doctor  != null ? new RefResponse(v.Doctor.Id, v.Doctor.FullName) : null,
        v.Room    != null ? new RefResponse(v.Room.Id,   v.Room.Name)   : null,
        v.VisitDate,
        v.Reason,
        v.Conclusion,
        v.Status.ToString(),
        v.VisitDiagnoses.Select(vd => new VisitDiagnosisResponse(
            vd.Diagnosis.Id, vd.Diagnosis.IcdCode, vd.Diagnosis.Name,
            vd.Diagnosis.Category, vd.Diagnosis.Description,
            vd.IsPrimary, vd.Note)).ToList()
    );
}
