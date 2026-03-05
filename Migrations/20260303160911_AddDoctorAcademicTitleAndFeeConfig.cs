using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClinicManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorAcademicTitleAndFeeConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "academic_title",
                table: "doctors",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "None");

            migrationBuilder.CreateTable(
                name: "system_configs",
                columns: table => new
                {
                    config_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    config_value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_configs", x => x.config_key);
                });

            migrationBuilder.InsertData(
                table: "system_configs",
                columns: new[] { "config_key", "config_value", "description" },
                values: new object[,]
                {
                    { "clinic_address", "123 Đường ABC, Quận X, TP.HCM", "Địa chỉ phòng khám" },
                    { "clinic_email", "", "Email phòng khám" },
                    { "clinic_name", "Phòng khám Đa khoa", "Tên phòng khám" },
                    { "clinic_phone", "028 1234 5678", "Số điện thoại phòng khám" },
                    { "clinic_tax_code", "", "Mã số thuế" },
                    { "currency", "VND", "Đơn vị tiền tệ" },
                    { "examination_fee", "100000", "Phí khám mặc định – bác sĩ thường (VNĐ)" },
                    { "fee_associate_professor", "450000", "Phí khám – Phó Giáo sư (VNĐ)" },
                    { "fee_master_cki", "250000", "Phí khám – Thạc sĩ / Bác sĩ chuyên khoa I (VNĐ)" },
                    { "fee_phd_ckii", "350000", "Phí khám – Tiến sĩ / Bác sĩ chuyên khoa II (VNĐ)" },
                    { "fee_professor", "550000", "Phí khám – Giáo sư (VNĐ)" },
                    { "receipt_footer", "Cảm ơn quý khách đã tin tưởng!", "Chân trang phiếu thu" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "system_configs");

            migrationBuilder.DropColumn(
                name: "academic_title",
                table: "doctors");
        }
    }
}
