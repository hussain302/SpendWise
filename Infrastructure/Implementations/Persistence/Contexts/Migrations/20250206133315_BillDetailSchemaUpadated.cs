using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BillDetailSchemaUpadated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "BillDetails",
                schema: "Ownership",
                newName: "BillDetails",
                newSchema: "Expenditure");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "BillDetails",
                schema: "Expenditure",
                newName: "BillDetails",
                newSchema: "Ownership");
        }
    }
}
