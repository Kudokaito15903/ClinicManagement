using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClinicManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixFKAndAddPrescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_users_CashierId",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "FK_users_doctors_doctor_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_doctor_id",
                table: "users");


            migrationBuilder.RenameColumn(
                name: "CashierId",
                table: "payments",
                newName: "cashier_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_payments_CashierId",
                table: "payments",
                newName: "IX_payments_cashier_user_id");

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "doctors",
                type: "bigint",
                nullable: true);

            // ── Data migration: copy users.doctor_id → doctors.user_id ──────
            migrationBuilder.Sql(@"
                UPDATE doctors d
                SET user_id = u.""Id""
                FROM users u
                WHERE u.doctor_id = d.""Id""
                  AND u.doctor_id IS NOT NULL;
            ");

            migrationBuilder.DropColumn(
                name: "doctor_id",
                table: "users");

            migrationBuilder.CreateTable(
                name: "medicines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ingredient = table.Column<string>(type: "TEXT", nullable: true),
                    dosage_form = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    unit = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    manufacturer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    country_of_origin = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    unit_price = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medicines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "prescriptions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VisitId = table.Column<long>(type: "bigint", nullable: false),
                    note = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_prescriptions_visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prescription_items",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: false),
                    MedicineId = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    unit_price = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    dosage_instruction = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prescription_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_prescription_items_medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prescription_items_prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "uq_doctors_user_id",
                table: "doctors",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_medicines_code",
                table: "medicines",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prescription_items_MedicineId",
                table: "prescription_items",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "uq_prescription_medicine",
                table: "prescription_items",
                columns: new[] { "PrescriptionId", "MedicineId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prescriptions_VisitId",
                table: "prescriptions",
                column: "VisitId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_users_user_id",
                table: "doctors",
                column: "user_id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_payments_users_cashier_user_id",
                table: "payments",
                column: "cashier_user_id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_users_user_id",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_payments_users_cashier_user_id",
                table: "payments");

            migrationBuilder.DropTable(
                name: "prescription_items");

            migrationBuilder.DropTable(
                name: "medicines");

            migrationBuilder.DropTable(
                name: "prescriptions");

            migrationBuilder.DropIndex(
                name: "uq_doctors_user_id",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "doctors");

            migrationBuilder.RenameColumn(
                name: "cashier_user_id",
                table: "payments",
                newName: "CashierId");

            migrationBuilder.RenameIndex(
                name: "IX_payments_cashier_user_id",
                table: "payments",
                newName: "IX_payments_CashierId");

            migrationBuilder.AddColumn<long>(
                name: "doctor_id",
                table: "users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_doctor_id",
                table: "users",
                column: "doctor_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_payments_users_CashierId",
                table: "payments",
                column: "CashierId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_users_doctors_doctor_id",
                table: "users",
                column: "doctor_id",
                principalTable: "doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
