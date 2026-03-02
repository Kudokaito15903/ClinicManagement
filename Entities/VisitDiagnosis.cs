namespace ClinicManagement.Entities;

public class VisitDiagnosis
{
    public long Id { get; set; }

    public long VisitId { get; set; }
    public Visit Visit { get; set; } = null!;

    public long DiagnosisId { get; set; }
    public Diagnosis Diagnosis { get; set; } = null!;

    public bool IsPrimary { get; set; } = false;            // Chẩn đoán chính hay phụ
    public string? Note { get; set; }
}
