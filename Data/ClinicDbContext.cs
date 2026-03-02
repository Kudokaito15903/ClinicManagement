using ClinicManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Data;

public class ClinicDbContext : DbContext
{
    public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Diagnosis> Diagnoses => Set<Diagnosis>();
    public DbSet<MedicalService> MedicalServices => Set<MedicalService>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Visit> Visits => Set<Visit>();
    public DbSet<VisitDiagnosis> VisitDiagnoses => Set<VisitDiagnosis>();
    public DbSet<VisitServiceItem> VisitServices => Set<VisitServiceItem>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── User ────────────────────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Username).HasColumnName("username").HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.Username).IsUnique();
            e.Property(x => x.PasswordHash).HasColumnName("password").HasMaxLength(255).IsRequired();
            e.Property(x => x.Role).HasColumnName("role").HasMaxLength(20).HasConversion<string>();
            e.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            e.Property(x => x.DoctorId).HasColumnName("doctor_id");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.HasOne(x => x.Doctor).WithOne(d => d.User).HasForeignKey<User>(x => x.DoctorId).OnDelete(DeleteBehavior.SetNull);
        });

        // ── Doctor ──────────────────────────────────────────────────────
        modelBuilder.Entity<Doctor>(e =>
        {
            e.ToTable("doctors");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Code).HasColumnName("code").HasMaxLength(20).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(100).IsRequired();
            e.Property(x => x.Specialty).HasColumnName("specialty").HasMaxLength(100);
            e.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(15);
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(100);
            e.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        // ── Room ─────────────────────────────────────────────────────────
        modelBuilder.Entity<Room>(e =>
        {
            e.ToTable("rooms");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Code).HasColumnName("code").HasMaxLength(20).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            e.Property(x => x.Description).HasColumnName("description").HasMaxLength(255);
            e.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        });

        // ── Diagnosis ────────────────────────────────────────────────────
        modelBuilder.Entity<Diagnosis>(e =>
        {
            e.ToTable("diagnoses");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.IcdCode).HasColumnName("icd_code").HasMaxLength(20).IsRequired();
            e.HasIndex(x => x.IcdCode).IsUnique();
            e.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            e.Property(x => x.Category).HasColumnName("category").HasMaxLength(100);
            e.Property(x => x.Description).HasColumnName("description").HasColumnType("TEXT");
        });

        // ── MedicalService ───────────────────────────────────────────────
        modelBuilder.Entity<MedicalService>(e =>
        {
            e.ToTable("services");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Code).HasColumnName("code").HasMaxLength(20).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            e.Property(x => x.Unit).HasColumnName("unit").HasMaxLength(50).HasDefaultValue("lần");
            e.Property(x => x.Price).HasColumnName("price").HasColumnType("NUMERIC(15,2)").IsRequired();
            e.Property(x => x.Category).HasColumnName("category").HasMaxLength(100);
            e.Property(x => x.Description).HasColumnName("description").HasColumnType("TEXT");
            e.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        // ── Patient ──────────────────────────────────────────────────────
        modelBuilder.Entity<Patient>(e =>
        {
            e.ToTable("patients");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Code).HasColumnName("code").HasMaxLength(20).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.FullName).HasDatabaseName("ix_patients_full_name");
            e.Property(x => x.DateOfBirth).HasColumnName("date_of_birth").IsRequired();
            e.Property(x => x.Gender).HasColumnName("gender").HasMaxLength(10).HasConversion<string>();
            e.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(15);
            e.Property(x => x.Address).HasColumnName("address").HasColumnType("TEXT");
            e.Property(x => x.Note).HasColumnName("note").HasColumnType("TEXT");
            e.Property(x => x.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        // ── Visit ─────────────────────────────────────────────────────────
        modelBuilder.Entity<Visit>(e =>
        {
            e.ToTable("visits");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Code).HasColumnName("code").HasMaxLength(20).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.VisitDate).HasColumnName("visit_date").IsRequired();
            e.Property(x => x.Reason).HasColumnName("reason").HasColumnType("TEXT");
            e.Property(x => x.Conclusion).HasColumnName("conclusion").HasColumnType("TEXT");
            e.Property(x => x.Status).HasColumnName("status").HasMaxLength(20).HasConversion<string>();
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");

            e.HasOne(x => x.Patient).WithMany(p => p.Visits).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Doctor).WithMany(d => d.Visits).HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Room).WithMany(r => r.Visits).HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(x => x.VisitServices).WithOne(vs => vs.Visit).HasForeignKey(vs => vs.VisitId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.VisitDiagnoses).WithOne(vd => vd.Visit).HasForeignKey(vd => vd.VisitId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Payment).WithOne(p => p.Visit).HasForeignKey<Payment>(p => p.VisitId);

            e.HasIndex(x => x.PatientId).HasDatabaseName("ix_visits_patient_id");
            e.HasIndex(x => x.DoctorId).HasDatabaseName("ix_visits_doctor_id");
            e.HasIndex(x => x.RoomId).HasDatabaseName("ix_visits_room_id");
            e.HasIndex(x => x.VisitDate).HasDatabaseName("ix_visits_visit_date");
        });

        // ── VisitDiagnosis ────────────────────────────────────────────────
        modelBuilder.Entity<VisitDiagnosis>(e =>
        {
            e.ToTable("visit_diagnoses");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.IsPrimary).HasColumnName("is_primary").HasDefaultValue(false);
            e.Property(x => x.Note).HasColumnName("note").HasMaxLength(255);
            e.HasIndex(x => new { x.VisitId, x.DiagnosisId }).IsUnique().HasDatabaseName("uq_visit_diagnosis");
            e.HasOne(x => x.Visit).WithMany(v => v.VisitDiagnoses).HasForeignKey(x => x.VisitId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Diagnosis).WithMany(d => d.VisitDiagnoses).HasForeignKey(x => x.DiagnosisId).OnDelete(DeleteBehavior.Restrict);
        });

        // ── VisitServiceItem ──────────────────────────────────────────────
        modelBuilder.Entity<VisitServiceItem>(e =>
        {
            e.ToTable("visit_services");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.UnitPrice).HasColumnName("unit_price").HasColumnType("NUMERIC(15,2)").IsRequired();
            e.Property(x => x.Quantity).HasColumnName("quantity").HasDefaultValue(1).IsRequired();
            e.Ignore(x => x.TotalPrice);    // computed in C#, not a stored column
            e.Property(x => x.Note).HasColumnName("note").HasMaxLength(255);
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.HasOne(x => x.Visit).WithMany(v => v.VisitServices).HasForeignKey(x => x.VisitId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Service).WithMany(s => s.VisitServiceItems).HasForeignKey(x => x.ServiceId).OnDelete(DeleteBehavior.Restrict);
        });

        // ── Payment ───────────────────────────────────────────────────────
        modelBuilder.Entity<Payment>(e =>
        {
            e.ToTable("payments");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.ExaminationFee).HasColumnName("examination_fee").HasColumnType("NUMERIC(15,2)").IsRequired();
            e.Property(x => x.ServiceTotal).HasColumnName("service_total").HasColumnType("NUMERIC(15,2)").IsRequired();
            e.Property(x => x.GrandTotal).HasColumnName("grand_total").HasColumnType("NUMERIC(15,2)").IsRequired();
            e.Property(x => x.Discount).HasColumnName("discount").HasColumnType("NUMERIC(15,2)").HasDefaultValue(0);
            e.Property(x => x.FinalAmount).HasColumnName("final_amount").HasColumnType("NUMERIC(15,2)").IsRequired();
            e.Property(x => x.PaymentMethod).HasColumnName("payment_method").HasMaxLength(20).HasConversion<string>();
            e.Property(x => x.PaidAt).HasColumnName("paid_at");
            e.Property(x => x.CashierNote).HasColumnName("cashier_note").HasColumnType("TEXT");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.HasOne(x => x.Cashier).WithMany().HasForeignKey(x => x.CashierId).OnDelete(DeleteBehavior.SetNull);
        });
    }

    public override int SaveChanges()
    {
        SetTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetTimestamps()
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity is User u)                  { u.CreatedAt = now; u.UpdatedAt = now; }
                else if (entry.Entity is Doctor d)           { d.CreatedAt = now; }
                else if (entry.Entity is MedicalService ms)  { ms.CreatedAt = now; }
                else if (entry.Entity is Patient p)          { p.CreatedAt = now; p.UpdatedAt = now; }
                else if (entry.Entity is Visit v)            { v.CreatedAt = now; v.UpdatedAt = now; }
                else if (entry.Entity is VisitServiceItem vs){ vs.CreatedAt = now; }
                else if (entry.Entity is Payment pay)        { pay.CreatedAt = now; }
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is User u)         u.UpdatedAt = now;
                else if (entry.Entity is Patient p) p.UpdatedAt = now;
                else if (entry.Entity is Visit v)   v.UpdatedAt = now;
            }
        }
    }
}
