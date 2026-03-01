namespace ClinicManagement.Entities;

public enum Gender { Male, Female, Other }

public class Patient
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int? BirthYear { get; set; }
    public Gender? Gender { get; set; }
    public string? Address { get; set; }

    public bool Deleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
