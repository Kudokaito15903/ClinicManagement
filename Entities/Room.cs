namespace ClinicManagement.Entities;

public class Room
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;       // PK001
    public string Name { get; set; } = string.Empty;       // Phòng khám nội
    public string? Description { get; set; }

    // Navigation
    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
