namespace ClinicManagement.Entities;

public class Diagnosis
{
    public long Id { get; set; }
    public string IcdCode { get; set; } = string.Empty;    // A00, B01.1, ...
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }                   // Nhóm bệnh
    public string? Description { get; set; }

    // Navigation
    public ICollection<VisitDiagnosis> VisitDiagnoses { get; set; } = new List<VisitDiagnosis>();
}
