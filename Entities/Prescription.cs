namespace ClinicManagement.Entities;

public class Prescription
{
    public long Id { get; set; }

    public long VisitId { get; set; }                          // 1 Visit = 1 Prescription
    public Visit Visit { get; set; } = null!;

    public string? Note { get; set; }                          // Lời dặn của bác sĩ
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ICollection<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();
}
