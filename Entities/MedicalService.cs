namespace ClinicManagement.Entities;

public class MedicalService
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;       // SV001
    public string Name { get; set; } = string.Empty;       // Siêu âm ổ bụng
    public string Unit { get; set; } = "lần";
    public decimal Price { get; set; }
    public string? Category { get; set; }                   // Xét nghiệm / Chẩn đoán hình ảnh
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ICollection<VisitServiceItem> VisitServiceItems { get; set; } = new List<VisitServiceItem>();
}
