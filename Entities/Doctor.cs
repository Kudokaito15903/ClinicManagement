namespace ClinicManagement.Entities;

public enum AcademicTitle
{
    None,               // Không có / bác sĩ thường
    Master_CKI,         // Thạc sĩ / Bác sĩ chuyên khoa I
    PhD_CKII,           // Tiến sĩ / Bác sĩ chuyên khoa II
    AssociateProfessor, // Phó Giáo sư
    Professor           // Giáo sư
}

public class Doctor
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;       // BS001
    public string FullName { get; set; } = string.Empty;
    public string? Specialty { get; set; }
    public AcademicTitle AcademicTitle { get; set; } = AcademicTitle.None;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }

    public long? UserId { get; set; }                           // NULL nếu chưa có tài khoản

    // Navigation
    public User? User { get; set; }
    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
 