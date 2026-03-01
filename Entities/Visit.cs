namespace ClinicManagement.Entities;

public class Visit
{
    public long Id { get; set; }

    public long PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public long? DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    public long? RoomId { get; set; }
    public Room? Room { get; set; }

    public long? DiagnosisId { get; set; }
    public Diagnosis? Diagnosis { get; set; }

    public DateTime VisitDate { get; set; }
    public decimal ExaminationFee { get; set; }
    public string? Notes { get; set; }

    public List<VisitServiceItem> VisitServices { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
