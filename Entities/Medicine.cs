namespace ClinicManagement.Entities;

public class Medicine
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;           // TH001
    public string Name { get; set; } = string.Empty;           // Amoxicillin 500mg
    public string? Ingredient { get; set; }                    // Hoạt chất
    public string? DosageForm { get; set; }                    // Viên nén, Siro, Tiêm…
    public string Unit { get; set; } = string.Empty;           // viên, lọ, hộp
    public string? Manufacturer { get; set; }                  // Nhà sản xuất
    public string? CountryOfOrigin { get; set; }               // Nước sản xuất
    public decimal UnitPrice { get; set; }                     // Giá bán lẻ
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();
}
