using ClinicManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Data;

public class DataSeeder
{
    public static async Task SeedAsync(ClinicDbContext db)
    {
        if (!await db.Doctors.AnyAsync())
        {

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

        if (!await db.Medicines.AnyAsync())
        {
            db.Medicines.AddRange(
                new Medicine { Code = "M001", Name = "Paracetamol 500mg", Unit = "Viên", UnitPrice = 2000, Ingredient = "Paracetamol", DosageForm = "Viên nén", Manufacturer = "DHG Pharma" },
                new Medicine { Code = "M002", Name = "Amoxicillin 500mg", Unit = "Viên", UnitPrice = 3500, Ingredient = "Amoxicillin", DosageForm = "Viên nang", Manufacturer = "Domesco" },
                new Medicine { Code = "M003", Name = "Omeprazole 20mg", Unit = "Viên", UnitPrice = 5000, Ingredient = "Omeprazole", DosageForm = "Viên bao tan trong ruột", Manufacturer = "Traphaco" },
                new Medicine { Code = "M004", Name = "Vitamin C 500mg", Unit = "Viên", UnitPrice = 1500, Ingredient = "Ascorbic Acid", DosageForm = "Viên nén", Manufacturer = "OPC" },
                new Medicine { Code = "M005", Name = "Oresol", Unit = "Gói", UnitPrice = 4000, Ingredient = "Muối bù nước", DosageForm = "Bột", Manufacturer = "Navi" }
            );
            await db.SaveChangesAsync(); // Save to get IDs for foreign keys
        }

        if (!await db.Patients.AnyAsync())
        {
            var p1 = new Patient { Code = "BN001", FullName = "Nguyen Van A", DateOfBirth = new DateOnly(1990, 5, 20), Gender = Gender.Male, Phone = "0901234567" };
            var p2 = new Patient { Code = "BN002", FullName = "Tran Thi B", DateOfBirth = new DateOnly(1985, 10, 15), Gender = Gender.Female, Phone = "0912345678" };
            db.Patients.AddRange(p1, p2);
            await db.SaveChangesAsync();

            var docId = await db.Doctors.Select(d => d.Id).FirstAsync();
            var roomId = await db.Rooms.Select(r => r.Id).FirstAsync();
            var diagId = await db.Diagnoses.Select(d => d.Id).FirstAsync();
            var medId = await db.Medicines.Select(m => m.Id).FirstAsync();

            var v1 = new Visit { Code = "KB001", PatientId = p1.Id, DoctorId = docId, RoomId = roomId, Reason = "Sốt, ho", Status = VisitStatus.Completed, VisitDate = DateTime.UtcNow };
            db.Visits.Add(v1);
            await db.SaveChangesAsync();

            db.VisitDiagnoses.Add(new VisitDiagnosis { VisitId = v1.Id, DiagnosisId = diagId, IsPrimary = true, Note = "Cảm cúm" });
            
            var pres = new Prescription { VisitId = v1.Id, Note = "Uống sau ăn" };
            db.Prescriptions.Add(pres);
            await db.SaveChangesAsync();

            db.PrescriptionItems.Add(new PrescriptionItem { PrescriptionId = pres.Id, MedicineId = medId, Quantity = 10, UnitPrice = 2000, DosageInstruction = "Sáng 1 viên, tối 1 viên" });
            await db.SaveChangesAsync();
        }
    }
}
