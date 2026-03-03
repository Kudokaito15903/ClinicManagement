using System.ComponentModel.DataAnnotations;
using ClinicManagement.Entities;

namespace ClinicManagement.DTOs.Requests;

public record DoctorRequest(
    [Required] string FullName,
    string? Specialty,
    AcademicTitle? AcademicTitle
);

public record RoomRequest(
    [Required] string Name,
    string? Description
);

public record DiagnosisRequest(
    [Required] string IcdCode,
    [Required] string Name,
    string? Category,
    string? Description
);

public record MedicalServiceRequest(
    [Required] string Code,
    [Required] string Name,
    [Required] decimal Price
);

public record PatientCreateRequest(
    [Required] string FullName,
    [Required] DateOnly DateOfBirth,
    [Required] Gender Gender,
    string? Phone,
    string? Address,
    string? Note
);

public record PatientUpdateRequest(
    [Required] string FullName,
    [Required] DateOnly DateOfBirth,
    [Required] Gender Gender,
    string? Phone,
    string? Address,
    string? Note
);

public record VisitCreateRequest(
    [Required] long PatientId,
    [Required] long DoctorId,
    [Required] long RoomId,
    string? Reason
);

public record VisitUpdateRequest(
    long? DoctorId,
    long? RoomId,
    string? Reason,
    string? Conclusion,
    VisitStatus? Status
);

public record VisitServiceAddRequest(
    [Required] long ServiceId,
    [Required][Range(1, int.MaxValue)] int Quantity
);

public record VisitDiagnosisAddRequest(
    [Required] long DiagnosisId,
    bool IsPrimary = false,
    string? Note = null
);

public record PaymentCreateRequest(
    decimal? ExaminationFee = null,         // null = tự tra từ SystemConfig theo trình độ bác sĩ
    decimal Discount = 0,
    PaymentMethod PaymentMethod = PaymentMethod.Cash,
    string? CashierNote = null
);

public record VisitStatusUpdateRequest(
    [Required] VisitStatus Status
);

public record SystemConfigUpdateRequest(
    [Required] string ConfigValue
);
