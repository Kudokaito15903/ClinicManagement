namespace ClinicManagement.Entities;

public class VisitServiceItem
{
    public long Id { get; set; }

    public long VisitId { get; set; }
    public Visit Visit { get; set; } = null!;

    public long ServiceId { get; set; }
    public MedicalService Service { get; set; } = null!;

    public decimal UnitPrice { get; set; }                  // Snapshot giá tại thời điểm chỉ định
    public int Quantity { get; set; } = 1;
    public decimal TotalPrice => UnitPrice * Quantity;      // Computed
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
}
