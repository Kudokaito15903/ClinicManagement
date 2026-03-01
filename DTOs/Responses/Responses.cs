using ClinicManagement.Entities;

namespace ClinicManagement.DTOs.Responses;

public record RefResponse(long Id, string Name);

public record DoctorResponse(long Id, string FullName, string? Specialty);

public record RoomResponse(long Id, string Name, string? Description);

public record DiagnosisResponse(long Id, string Code, string Name, string? Description);

public record MedicalServiceResponse(long Id, string Code, string Name, decimal Price);

public record PatientResponse(
    long Id,
    string Code,
    string FullName,
    int? BirthYear,
    Gender? Gender,
    string? Address,
    RefResponse? Doctor,
    RefResponse? Room,
    RefResponse? Diagnosis
);

public record VisitResponse(
    long Id,
    PatientResponse? Patient,
    RefResponse? Doctor,
    RefResponse? Room,
    RefResponse? Diagnosis,
    DateTime VisitDate,
    decimal ExaminationFee,
    string? Notes
);

public record VisitSummaryResponse(
    long Id,
    DateTime VisitDate,
    RefResponse? Doctor,
    RefResponse? Room,
    RefResponse? Diagnosis,
    decimal Total,
    string? Notes
);

public record VisitServiceResponse(
    long Id,
    long ServiceId,
    string ServiceName,
    decimal UnitPrice,
    int Quantity,
    decimal Subtotal,
    DateTime CreatedAt
);

public record VisitDetailResponse(
    VisitResponse Visit,
    List<VisitServiceResponse> Services,
    decimal ServiceTotal,
    decimal ExaminationFee,
    decimal GrandTotal
);

public record BillResponse(
    long VisitId,
    DateTime? VisitDate,
    string ClinicName,
    string PatientCode,
    string PatientName,
    int? PatientBirthYear,
    string? PatientGender,
    string? PatientAddress,
    string? DoctorName,
    string? RoomName,
    string? DiagnosisName,
    decimal ExaminationFee,
    string? Notes,
    List<VisitServiceResponse> Services,
    decimal ServiceTotal,
    decimal GrandTotal
);

public record DailyRevenueResponse(
    DateOnly Date,
    long VisitCount,
    decimal ExaminationRevenue,
    decimal ServiceRevenue,
    decimal Total
);

public record RevenueReportResponse(
    string From,
    string To,
    long TotalVisits,
    decimal ExaminationTotal,
    decimal ServiceTotal,
    decimal GrandTotal,
    List<DailyRevenueResponse> Daily
);

public record StatisticsResponse(
    long TotalPatients,
    long VisitsToday,
    long VisitsThisMonth,
    decimal RevenueToday,
    decimal RevenueMonth,
    decimal RevenueTotal
);
