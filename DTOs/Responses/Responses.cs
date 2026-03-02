using ClinicManagement.Entities;

namespace ClinicManagement.DTOs.Responses;

public record RefResponse(long Id, string Name);

public record DoctorResponse(long Id, string Code, string FullName, string? Specialty, string? Phone, string? Email, bool IsActive);

public record RoomResponse(long Id, string Code, string Name, string? Description, bool IsActive);

public record DiagnosisResponse(long Id, string IcdCode, string Name, string? Category, string? Description);

public record MedicalServiceResponse(long Id, string Code, string Name, string Unit, decimal Price, string? Category, bool IsActive);

public record PatientResponse(
    long Id,
    string Code,
    string FullName,
    DateOnly DateOfBirth,
    Gender Gender,
    string? Phone,
    string? Address,
    string? Note,
    RefResponse? Doctor,
    RefResponse? Room,
    List<DiagnosisResponse>? Diagnoses
);

public record VisitResponse(
    long Id,
    string Code,
    PatientResponse? Patient,
    RefResponse? Doctor,
    RefResponse? Room,
    DateTime VisitDate,
    string? Reason,
    string? Conclusion,
    string Status,
    List<DiagnosisResponse>? Diagnoses
);

public record PaymentResponse(
    long Id,
    long VisitId,
    decimal ExaminationFee,
    decimal ServiceTotal,
    decimal GrandTotal,
    decimal Discount,
    decimal FinalAmount,
    string PaymentMethod,
    DateTime? PaidAt,
    string? CashierNote,
    DateTime CreatedAt
);

public record VisitListItemResponse(
    long Id,
    string Code,
    string PatientName,
    string? DoctorName,
    string? RoomName,
    DateTime VisitDate,
    string Status
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
