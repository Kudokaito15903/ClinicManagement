namespace ClinicManagement.Entities;

public class Doctor
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Specialty { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
