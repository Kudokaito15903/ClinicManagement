using ClinicManagement.Data;
using ClinicManagement.DTOs.Requests;
using ClinicManagement.DTOs.Responses;
using ClinicManagement.Entities;
using ClinicManagement.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClinicManagement.Services;

public class VisitService
{
    private readonly ClinicDbContext _db;
    private readonly PatientService _patientService;
    private readonly DoctorService _doctorService;
    private readonly RoomService _roomService;
    private readonly DiagnosisService _diagnosisService;
    private readonly decimal _defaultExaminationFee;

    public VisitService(ClinicDbContext db, PatientService patientService,
        DoctorService doctorService, RoomService roomService,
        DiagnosisService diagnosisService, IConfiguration config)
    {
        _db = db;
        _patientService = patientService;
        _doctorService = doctorService;
        _roomService = roomService;
        _diagnosisService = diagnosisService;
        _defaultExaminationFee = config.GetValue<decimal>("Clinic:ExaminationFee", 50000m);
    }

    public async Task<VisitResponse> CreateAsync(VisitCreateRequest req)
    {
        // Validate patient exists
        await _patientService.GetByIdAsync(req.PatientId);

        var visit = new Visit
        {
            PatientId = req.PatientId,
            DoctorId = req.DoctorId,
            RoomId = req.RoomId,
            DiagnosisId = req.DiagnosisId,
            VisitDate = DateTime.UtcNow,
            ExaminationFee = req.ExaminationFee ?? _defaultExaminationFee,
            Notes = req.Notes
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
        if (req.DoctorId.HasValue) visit.DoctorId = req.DoctorId;
        if (req.RoomId.HasValue) visit.RoomId = req.RoomId;
        if (req.DiagnosisId.HasValue) visit.DiagnosisId = req.DiagnosisId;
        if (req.ExaminationFee.HasValue) visit.ExaminationFee = req.ExaminationFee.Value;
        if (req.Notes != null) visit.Notes = req.Notes;

        await _db.SaveChangesAsync();
        return await ToResponseAsync(await GetByIdRawAsync(id));
    }

    public async Task<List<VisitSummaryResponse>> GetPatientHistoryAsync(long patientId)
    {
        // Validate patient exists
        await _patientService.GetByIdAsync(patientId);

        var visits = await _db.Visits
            .Include(v => v.Doctor)
            .Include(v => v.Room)
            .Include(v => v.Diagnosis)
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

            result.Add(new VisitSummaryResponse(
                v.Id,
                v.VisitDate,
                v.Doctor != null ? new RefResponse(v.Doctor.Id, v.Doctor.FullName) : null,
                v.Room != null ? new RefResponse(v.Room.Id, v.Room.Name) : null,
                v.Diagnosis != null ? new RefResponse(v.Diagnosis.Id, v.Diagnosis.Name) : null,
                v.ExaminationFee + serviceTotal,
                v.Notes
            ));
        }

        return result;
    }

    public async Task<Visit> GetByIdRawAsync(long id)
        => await _db.Visits
            .Include(v => v.Patient)
            .Include(v => v.Doctor)
            .Include(v => v.Room)
            .Include(v => v.Diagnosis)
            .FirstOrDefaultAsync(v => v.Id == id)
           ?? throw new ResourceNotFoundException($"Lan kham khong ton tai: {id}");

    public async Task<VisitResponse> ToResponseAsync(Visit v) => new(
        v.Id,
        v.Patient != null ? _patientService.ToResponse(v.Patient) : null,
        v.Doctor != null ? new RefResponse(v.Doctor.Id, v.Doctor.FullName) : null,
        v.Room != null ? new RefResponse(v.Room.Id, v.Room.Name) : null,
        v.Diagnosis != null ? new RefResponse(v.Diagnosis.Id, v.Diagnosis.Name) : null,
        v.VisitDate,
        v.ExaminationFee,
        v.Notes
    );
}
