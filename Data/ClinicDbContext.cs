using ClinicManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Data;

public class ClinicDbContext : DbContext
{
    public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options) { }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Diagnosis> Diagnoses => Set<Diagnosis>();
    public DbSet<MedicalService> MedicalServices => Set<MedicalService>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Visit> Visits => Set<Visit>();
    public DbSet<VisitServiceItem> VisitServices => Set<VisitServiceItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Doctor
        modelBuilder.Entity<Doctor>(e =>
        {
            e.ToTable("doctors");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(255).IsRequired();
            e.Property(x => x.Specialty).HasColumnName("specialty").HasMaxLength(255);
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        // Room
        modelBuilder.Entity<Room>(e =>
        {
            e.ToTable("rooms");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        // Diagnosis
        modelBuilder.Entity<Diagnosis>(e =>
        {
            e.ToTable("diagnoses");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            e.Property(x => x.Description).HasColumnName("description").HasColumnType("TEXT");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        // MedicalService
        modelBuilder.Entity<MedicalService>(e =>
        {
            e.ToTable("medical_services");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            e.Property(x => x.Price).HasColumnName("price").HasColumnType("NUMERIC(15,2)").IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        // Patient
        modelBuilder.Entity<Patient>(e =>
        {
            e.ToTable("patients");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(255).IsRequired();
            e.Property(x => x.BirthYear).HasColumnName("birth_year");
            e.Property(x => x.Gender).HasColumnName("gender").HasMaxLength(10)
             .HasConversion<string>();
            e.Property(x => x.Address).HasColumnName("address").HasColumnType("TEXT");
            e.Property(x => x.Deleted).HasColumnName("deleted").HasDefaultValue(false);
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            // Index tìm kiếm theo tên
            e.HasIndex(x => x.FullName).HasDatabaseName("ix_patients_full_name");
        });

        // Visit
        modelBuilder.Entity<Visit>(e =>
        {
            e.ToTable("visits");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.VisitDate).HasColumnName("visit_date").IsRequired();
            e.Property(x => x.ExaminationFee).HasColumnName("examination_fee").HasColumnType("NUMERIC(15,2)").IsRequired();
            e.Property(x => x.Notes).HasColumnName("notes").HasColumnType("TEXT");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.HasOne(x => x.Patient).WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Doctor).WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.Room).WithMany().HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.Diagnosis).WithMany().HasForeignKey(x => x.DiagnosisId).OnDelete(DeleteBehavior.SetNull);
            e.HasMany(x => x.VisitServices).WithOne(vs => vs.Visit).HasForeignKey(vs => vs.VisitId).OnDelete(DeleteBehavior.Cascade);
            // Performance indexes cho các FK hay dùng trong WHERE/JOIN
            e.HasIndex(x => x.PatientId).HasDatabaseName("ix_visits_patient_id");
            e.HasIndex(x => x.DoctorId).HasDatabaseName("ix_visits_doctor_id");
            e.HasIndex(x => x.RoomId).HasDatabaseName("ix_visits_room_id");
            e.HasIndex(x => x.DiagnosisId).HasDatabaseName("ix_visits_diagnosis_id");
            e.HasIndex(x => x.VisitDate).HasDatabaseName("ix_visits_visit_date");
        });

        // VisitServiceItem
        modelBuilder.Entity<VisitServiceItem>(e =>
        {
            e.ToTable("visit_services");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.UnitPrice).HasColumnName("unit_price").HasColumnType("NUMERIC(15,2)").IsRequired();
            e.Property(x => x.Quantity).HasColumnName("quantity").IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.HasOne(x => x.Visit).WithMany(v => v.VisitServices).HasForeignKey(x => x.VisitId);
            e.HasOne(x => x.Service).WithMany().HasForeignKey(x => x.ServiceId).OnDelete(DeleteBehavior.Restrict);
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
                if (entry.Entity is Doctor d) { d.CreatedAt = now; d.UpdatedAt = now; }
                else if (entry.Entity is Room r) { r.CreatedAt = now; r.UpdatedAt = now; }
                else if (entry.Entity is Diagnosis diag) { diag.CreatedAt = now; diag.UpdatedAt = now; }
                else if (entry.Entity is MedicalService ms) { ms.CreatedAt = now; ms.UpdatedAt = now; }
                else if (entry.Entity is Patient p) { p.CreatedAt = now; p.UpdatedAt = now; }
                else if (entry.Entity is Visit v) { v.CreatedAt = now; v.UpdatedAt = now; }
                else if (entry.Entity is VisitServiceItem vs) { vs.CreatedAt = now; }
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is Doctor d) d.UpdatedAt = now;
                else if (entry.Entity is Room r) r.UpdatedAt = now;
                else if (entry.Entity is Diagnosis diag) diag.UpdatedAt = now;
                else if (entry.Entity is MedicalService ms) ms.UpdatedAt = now;
                else if (entry.Entity is Patient p) p.UpdatedAt = now;
                else if (entry.Entity is Visit v) v.UpdatedAt = now;
            }
        }
    }
}
