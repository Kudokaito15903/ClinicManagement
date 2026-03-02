namespace ClinicManagement.Entities;

public enum UserRole
{
    Admin,
    Doctor,
    Receptionist,
    Cashier
}

public class User
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;    // bcrypt hash
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;

    public long? DoctorId { get; set; }                         // NULL nếu không phải bác sĩ
    public Doctor? Doctor { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
