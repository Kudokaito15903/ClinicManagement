using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagement.Migrations
{
    /// <inheritdoc />
    public partial class RemovePatientFKsAndAddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patients_diagnoses_DiagnosisId",
                table: "patients");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_doctors_DoctorId",
                table: "patients");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_rooms_RoomId",
                table: "patients");

            migrationBuilder.DropIndex(
                name: "IX_patients_DiagnosisId",
                table: "patients");

            migrationBuilder.DropIndex(
                name: "IX_patients_DoctorId",
                table: "patients");

            migrationBuilder.DropIndex(
                name: "IX_patients_RoomId",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "DiagnosisId",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "patients");

            migrationBuilder.RenameIndex(
                name: "IX_visits_RoomId",
                table: "visits",
                newName: "ix_visits_room_id");

            migrationBuilder.RenameIndex(
                name: "IX_visits_PatientId",
                table: "visits",
                newName: "ix_visits_patient_id");

            migrationBuilder.RenameIndex(
                name: "IX_visits_DoctorId",
                table: "visits",
                newName: "ix_visits_doctor_id");

            migrationBuilder.RenameIndex(
                name: "IX_visits_DiagnosisId",
                table: "visits",
                newName: "ix_visits_diagnosis_id");

            migrationBuilder.CreateIndex(
                name: "ix_visits_visit_date",
                table: "visits",
                column: "visit_date");

            migrationBuilder.CreateIndex(
                name: "ix_patients_full_name",
                table: "patients",
                column: "full_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_visits_visit_date",
                table: "visits");

            migrationBuilder.DropIndex(
                name: "ix_patients_full_name",
                table: "patients");

            migrationBuilder.RenameIndex(
                name: "ix_visits_room_id",
                table: "visits",
                newName: "IX_visits_RoomId");

            migrationBuilder.RenameIndex(
                name: "ix_visits_patient_id",
                table: "visits",
                newName: "IX_visits_PatientId");

            migrationBuilder.RenameIndex(
                name: "ix_visits_doctor_id",
                table: "visits",
                newName: "IX_visits_DoctorId");

            migrationBuilder.RenameIndex(
                name: "ix_visits_diagnosis_id",
                table: "visits",
                newName: "IX_visits_DiagnosisId");

            migrationBuilder.AddColumn<long>(
                name: "DiagnosisId",
                table: "patients",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DoctorId",
                table: "patients",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RoomId",
                table: "patients",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_patients_DiagnosisId",
                table: "patients",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_patients_DoctorId",
                table: "patients",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_patients_RoomId",
                table: "patients",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_patients_diagnoses_DiagnosisId",
                table: "patients",
                column: "DiagnosisId",
                principalTable: "diagnoses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_patients_doctors_DoctorId",
                table: "patients",
                column: "DoctorId",
                principalTable: "doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_patients_rooms_RoomId",
                table: "patients",
                column: "RoomId",
                principalTable: "rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
