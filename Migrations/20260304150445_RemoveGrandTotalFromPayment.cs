using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagement.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGrandTotalFromPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE payments DROP COLUMN IF EXISTS grand_total;");
            migrationBuilder.Sql("ALTER TABLE rooms DROP COLUMN IF EXISTS is_active;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE payments ADD COLUMN IF NOT EXISTS grand_total NUMERIC(15,2) DEFAULT 0;");
            migrationBuilder.Sql("ALTER TABLE rooms ADD COLUMN IF NOT EXISTS is_active BOOLEAN DEFAULT true;");
        }
    }
}
