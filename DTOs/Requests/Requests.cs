using System.ComponentModel.DataAnnotations;
using ClinicManagement.Entities;

namespace ClinicManagement.DTOs.Requests;

public record DoctorRequest(
    [Required] string FullName,
    string? Specialty
);

public record RoomRequest(
    [Required] string Name,
    string? Description
);

public record DiagnosisRequest(
    [Required] string Code,
    [Required] string Name,
    string? Description
);

public record MedicalServiceRequest(
    [Required] string Code,
    [Required] string Name,
    [Required] decimal Price
);

public record PatientCreateRequest(
    string? Code,
    [Required] string FullName,
    int? BirthYear,
    Gender? Gender,
    string? Address
);

public record PatientUpdateRequest(
    [Required] string FullName,
    int? BirthYear,
    Gender? Gender,
    string? Address
);

public record VisitCreateRequest(
    [Required] long PatientId,
    long? DoctorId,
    long? RoomId,
    long? DiagnosisId,
    decimal? ExaminationFee,
    string? Notes
);

public record VisitUpdateRequest(
    long? DoctorId,
    long? RoomId,
    long? DiagnosisId,
    decimal? ExaminationFee,
    string? Notes
);

public record VisitServiceAddRequest(
    [Required] long ServiceId,
    [Required][Range(1, int.MaxValue)] int Quantity
);
