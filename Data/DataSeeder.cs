using ClinicManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Data;

public class DataSeeder
{
    public static async Task SeedAsync(ClinicDbContext db)
    {
        // Only seed if tables are empty
        if (await db.Doctors.AnyAsync()) return;

        db.Doctors.AddRange(
            new Doctor { Code = "BS001", FullName = "BS. Nguyen Van An", Specialty = "Noi khoa" },
            new Doctor { Code = "BS002", FullName = "BS. Tran Thi Bich", Specialty = "Sieu am" },
            new Doctor { Code = "BS003", FullName = "BS. Le Van Cuong", Specialty = "X-quang" },
            new Doctor { Code = "BS004", FullName = "BS. Pham Thi Dung", Specialty = "Xet nghiem" }
        );

        db.Rooms.AddRange(
            new Room { Code = "PK001", Name = "Phong kham noi", Description = "Phong kham benh ly noi khoa" },
            new Room { Code = "PK002", Name = "Phong sieu am", Description = "Phong thuc hien sieu am" },
            new Room { Code = "PK003", Name = "Phong x-quang", Description = "Phong chup X-quang" },
            new Room { Code = "PK004", Name = "Phong xet nghiem", Description = "Phong lay mau va xet nghiem" }
        );

        db.Diagnoses.AddRange(
            new Diagnosis { IcdCode = "J00", Name = "Cam lanh thong thuong" },
            new Diagnosis { IcdCode = "J06.9", Name = "Nhiem trung duong ho hap tren cap tinh" },
            new Diagnosis { IcdCode = "K21.0", Name = "Trao nguoc da day thuc quan voi viem thuc quan" },
            new Diagnosis { IcdCode = "K29.5", Name = "Viem da day man tinh" },
            new Diagnosis { IcdCode = "I10", Name = "Tang huyet ap nguyen phat" },
            new Diagnosis { IcdCode = "E11", Name = "Dai thao duong type 2" },
            new Diagnosis { IcdCode = "M54.5", Name = "Dau lung duoi" },
            new Diagnosis { IcdCode = "A09", Name = "Nhiem trung duong ruot va viem da day ruot" },
            new Diagnosis { IcdCode = "B34.9", Name = "Nhiem virus khong xac dinh" },
            new Diagnosis { IcdCode = "R05", Name = "Ho" }
        );

        db.MedicalServices.AddRange(
            new MedicalService { Code = "SV001", Name = "Sieu am o bung", Price = 150000 },
            new MedicalService { Code = "SV002", Name = "Sieu am tuyen giao", Price = 200000 },
            new MedicalService { Code = "SV003", Name = "Xet nghiem mau toan phan (CBC)", Price = 120000 },
            new MedicalService { Code = "SV004", Name = "Xet nghiem duong huyet (Glucose)", Price = 80000 },
            new MedicalService { Code = "SV005", Name = "Xet nghiem chuc nang gan (LFT)", Price = 250000 },
            new MedicalService { Code = "SV006", Name = "Xet nghiem chuc nang than (KFT)", Price = 200000 },
            new MedicalService { Code = "SV007", Name = "Chup X-quang phoi thang", Price = 180000 },
            new MedicalService { Code = "SV008", Name = "Chup X-quang cot song", Price = 180000 },
            new MedicalService { Code = "SV009", Name = "Dien tam do (ECG)", Price = 100000 },
            new MedicalService { Code = "SV010", Name = "Do huyet ap (BP)", Price = 30000 }
        );

        await db.SaveChangesAsync();
    }
}
