namespace ClinicManagement.Entities;

public enum Gender { Male, Female, Other }

public class Patient
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;       // BN000001
    public string FullName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }               // Thay birth_year
    public Gender Gender { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
