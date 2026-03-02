namespace ClinicManagement.Entities;

public class Doctor
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;       // BS001
    public string FullName { get; set; } = string.Empty;
    public string? Specialty { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    public User? User { get; set; }
}
 