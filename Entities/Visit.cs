namespace ClinicManagement.Entities;

public enum VisitStatus
{
    Received,       // Tiếp nhận
    Examining,      // Đang khám
    WaitingResult,  // Chờ kết quả dịch vụ
    Completed,      // Hoàn thành khám
    Paid            // Đã thanh toán
}

public class Visit
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;       // VIS20240601001

    public long PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public long DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public long RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public DateTime VisitDate { get; set; }
    public string? Reason { get; set; }                     // Lý do khám / triệu chứng
    public string? Conclusion { get; set; }                 // Kết luận của bác sĩ
    public VisitStatus Status { get; set; } = VisitStatus.Received;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public ICollection<VisitDiagnosis> VisitDiagnoses { get; set; } = new List<VisitDiagnosis>();
    public ICollection<VisitServiceItem> VisitServices { get; set; } = new List<VisitServiceItem>();
    public Payment? Payment { get; set; }
}
