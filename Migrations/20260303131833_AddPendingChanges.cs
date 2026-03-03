using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClinicManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_visit_services_medical_services_ServiceId",
                table: "visit_services");

            migrationBuilder.DropForeignKey(
                name: "FK_visits_diagnoses_DiagnosisId",
                table: "visits");

            migrationBuilder.DropForeignKey(
                name: "FK_visits_doctors_DoctorId",
                table: "visits");

            migrationBuilder.DropForeignKey(
                name: "FK_visits_rooms_RoomId",
                table: "visits");

            migrationBuilder.DropIndex(
                name: "ix_visits_diagnosis_id",
                table: "visits");

            migrationBuilder.DropIndex(
                name: "IX_diagnoses_code",
                table: "diagnoses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_medical_services",
                table: "medical_services");

            migrationBuilder.DropColumn(
                name: "DiagnosisId",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "examination_fee",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "birth_year",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "code",
                table: "diagnoses");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "diagnoses");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "diagnoses");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "medical_services");

            migrationBuilder.RenameTable(
                name: "medical_services",
                newName: "services");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "visits",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "deleted",
                table: "patients",
                newName: "is_deleted");

            migrationBuilder.RenameIndex(
                name: "IX_medical_services_code",
                table: "services",
                newName: "IX_services_code");

            migrationBuilder.AlterColumn<long>(
                name: "RoomId",
                table: "visits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DoctorId",
                table: "visits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "visits",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "conclusion",
                table: "visits",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "visits",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "visit_services",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "visit_services",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "rooms",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "rooms",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "rooms",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "rooms",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "patients",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "patients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "patients",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateOnly>(
                name: "date_of_birth",
                table: "patients",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "patients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "patients",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "specialty",
                table: "doctors",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "doctors",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "doctors",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "doctors",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "doctors",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "doctors",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "diagnoses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "icd_code",
                table: "diagnoses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "services",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "services",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "services",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "services",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "services",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "services",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "lần");

            migrationBuilder.AddPrimaryKey(
                name: "PK_services",
                table: "services",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    doctor_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_doctors_doctor_id",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "visit_diagnoses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VisitId = table.Column<long>(type: "bigint", nullable: false),
                    DiagnosisId = table.Column<long>(type: "bigint", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visit_diagnoses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_visit_diagnoses_diagnoses_DiagnosisId",
                        column: x => x.DiagnosisId,
                        principalTable: "diagnoses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_visit_diagnoses_visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VisitId = table.Column<long>(type: "bigint", nullable: false),
                    CashierId = table.Column<long>(type: "bigint", nullable: true),
                    examination_fee = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    service_total = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    grand_total = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    discount = table.Column<decimal>(type: "numeric(15,2)", nullable: false, defaultValue: 0m),
                    final_amount = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    payment_method = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cashier_note = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payments_users_CashierId",
                        column: x => x.CashierId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_payments_visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_visits_code",
                table: "visits",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rooms_code",
                table: "rooms",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_doctors_code",
                table: "doctors",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_diagnoses_icd_code",
                table: "diagnoses",
                column: "icd_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_CashierId",
                table: "payments",
                column: "CashierId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_VisitId",
                table: "payments",
                column: "VisitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_doctor_id",
                table: "users",
                column: "doctor_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_visit_diagnoses_DiagnosisId",
                table: "visit_diagnoses",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "uq_visit_diagnosis",
                table: "visit_diagnoses",
                columns: new[] { "VisitId", "DiagnosisId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_visit_services_services_ServiceId",
                table: "visit_services",
                column: "ServiceId",
                principalTable: "services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_visits_doctors_DoctorId",
                table: "visits",
                column: "DoctorId",
                principalTable: "doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_visits_rooms_RoomId",
                table: "visits",
                column: "RoomId",
                principalTable: "rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_visit_services_services_ServiceId",
                table: "visit_services");

            migrationBuilder.DropForeignKey(
                name: "FK_visits_doctors_DoctorId",
                table: "visits");

            migrationBuilder.DropForeignKey(
                name: "FK_visits_rooms_RoomId",
                table: "visits");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "visit_diagnoses");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropIndex(
                name: "IX_visits_code",
                table: "visits");

            migrationBuilder.DropIndex(
                name: "IX_rooms_code",
                table: "rooms");

            migrationBuilder.DropIndex(
                name: "IX_doctors_code",
                table: "doctors");

            migrationBuilder.DropIndex(
                name: "IX_diagnoses_icd_code",
                table: "diagnoses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_services",
                table: "services");

            migrationBuilder.DropColumn(
                name: "code",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "conclusion",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "status",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "note",
                table: "visit_services");

            migrationBuilder.DropColumn(
                name: "code",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "note",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "code",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "email",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "category",
                table: "diagnoses");

            migrationBuilder.DropColumn(
                name: "icd_code",
                table: "diagnoses");

            migrationBuilder.DropColumn(
                name: "category",
                table: "services");

            migrationBuilder.DropColumn(
                name: "description",
                table: "services");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "services");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "services");

            migrationBuilder.RenameTable(
                name: "services",
                newName: "medical_services");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "visits",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "patients",
                newName: "deleted");

            migrationBuilder.RenameIndex(
                name: "IX_services_code",
                table: "medical_services",
                newName: "IX_medical_services_code");

            migrationBuilder.AlterColumn<long>(
                name: "RoomId",
                table: "visits",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "DoctorId",
                table: "visits",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "DiagnosisId",
                table: "visits",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "examination_fee",
                table: "visits",
                type: "numeric(15,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "visit_services",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "rooms",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "rooms",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "rooms",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "rooms",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "patients",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "patients",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "patients",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "birth_year",
                table: "patients",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "specialty",
                table: "doctors",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "doctors",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "doctors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "diagnoses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "diagnoses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "diagnoses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "medical_services",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "medical_services",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "medical_services",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_medical_services",
                table: "medical_services",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "ix_visits_diagnosis_id",
                table: "visits",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_diagnoses_code",
                table: "diagnoses",
                column: "code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_visit_services_medical_services_ServiceId",
                table: "visit_services",
                column: "ServiceId",
                principalTable: "medical_services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_visits_diagnoses_DiagnosisId",
                table: "visits",
                column: "DiagnosisId",
                principalTable: "diagnoses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_visits_doctors_DoctorId",
                table: "visits",
                column: "DoctorId",
                principalTable: "doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_visits_rooms_RoomId",
                table: "visits",
                column: "RoomId",
                principalTable: "rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
