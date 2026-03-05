namespace ClinicManagement.Entities;

public class PrescriptionItem
{
    public long Id { get; set; }

    public long PrescriptionId { get; set; }
    public Prescription Prescription { get; set; } = null!;

    public long MedicineId { get; set; }
    public Medicine Medicine { get; set; } = null!;

    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }                     // Snapshot giá lúc kê đơn
    public string? DosageInstruction { get; set; }             // "Ngày 3 lần, mỗi lần 1 viên sau ăn"
    public string? Note { get; set; }
}
