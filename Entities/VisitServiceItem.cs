namespace ClinicManagement.Entities;

public class VisitServiceItem
{
    public long Id { get; set; }

    public long VisitId { get; set; }
    public Visit Visit { get; set; } = null!;

    public long ServiceId { get; set; }
    public MedicalService Service { get; set; } = null!;

    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
}
