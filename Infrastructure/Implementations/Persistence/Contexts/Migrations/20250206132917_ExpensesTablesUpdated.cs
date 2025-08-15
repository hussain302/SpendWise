using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExpensesTablesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenditure_AspNetUsers_PaidById",
                schema: "Bills",
                table: "Expenditure");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenditure_Organizations_OrganizationId",
                schema: "Bills",
                table: "Expenditure");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenditure_AspNetUsers_SharedWithId",
                schema: "Ownership",
                table: "Expenditure");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenditure_Expenditure_BillId",
                schema: "Ownership",
                table: "Expenditure");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenditure_Expenditure_BillId1",
                schema: "Ownership",
                table: "Expenditure");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenditure",
                schema: "Ownership",
                table: "Expenditure");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenditure",
                schema: "Bills",
                table: "Expenditure");

            migrationBuilder.EnsureSchema(
                name: "Expenditure");

            migrationBuilder.RenameTable(
                name: "Expenditure",
                schema: "Ownership",
                newName: "BillDetails",
                newSchema: "Ownership");

            migrationBuilder.RenameTable(
                name: "Expenditure",
                schema: "Bills",
                newName: "Bills",
                newSchema: "Expenditure");

            migrationBuilder.RenameIndex(
                name: "IX_Expenditure_SharedWithId",
                schema: "Ownership",
                table: "BillDetails",
                newName: "IX_BillDetails_SharedWithId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenditure_BillId1",
                schema: "Ownership",
                table: "BillDetails",
                newName: "IX_BillDetails_BillId1");

            migrationBuilder.RenameIndex(
                name: "IX_Expenditure_BillId",
                schema: "Ownership",
                table: "BillDetails",
                newName: "IX_BillDetails_BillId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenditure_PaidById",
                schema: "Expenditure",
                table: "Bills",
                newName: "IX_Bills_PaidById");

            migrationBuilder.RenameIndex(
                name: "IX_Expenditure_OrganizationId",
                schema: "Expenditure",
                table: "Bills",
                newName: "IX_Bills_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BillDetails",
                schema: "Ownership",
                table: "BillDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bills",
                schema: "Expenditure",
                table: "Bills",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_AspNetUsers_SharedWithId",
                schema: "Ownership",
                table: "BillDetails",
                column: "SharedWithId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_Bills_BillId",
                schema: "Ownership",
                table: "BillDetails",
                column: "BillId",
                principalSchema: "Expenditure",
                principalTable: "Bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_Bills_BillId1",
                schema: "Ownership",
                table: "BillDetails",
                column: "BillId1",
                principalSchema: "Expenditure",
                principalTable: "Bills",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_AspNetUsers_PaidById",
                schema: "Expenditure",
                table: "Bills",
                column: "PaidById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Organizations_OrganizationId",
                schema: "Expenditure",
                table: "Bills",
                column: "OrganizationId",
                principalSchema: "Ownership",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillDetails_AspNetUsers_SharedWithId",
                schema: "Ownership",
                table: "BillDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BillDetails_Bills_BillId",
                schema: "Ownership",
                table: "BillDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BillDetails_Bills_BillId1",
                schema: "Ownership",
                table: "BillDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_AspNetUsers_PaidById",
                schema: "Expenditure",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Organizations_OrganizationId",
                schema: "Expenditure",
                table: "Bills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bills",
                schema: "Expenditure",
                table: "Bills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BillDetails",
                schema: "Ownership",
                table: "BillDetails");

            migrationBuilder.EnsureSchema(
                name: "Bills");

            migrationBuilder.RenameTable(
                name: "Bills",
                schema: "Expenditure",
                newName: "Expenditure",
                newSchema: "Bills");

            migrationBuilder.RenameTable(
                name: "BillDetails",
                schema: "Ownership",
                newName: "Expenditure",
                newSchema: "Ownership");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_PaidById",
                schema: "Bills",
                table: "Expenditure",
                newName: "IX_Expenditure_PaidById");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_OrganizationId",
                schema: "Bills",
                table: "Expenditure",
                newName: "IX_Expenditure_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_BillDetails_SharedWithId",
                schema: "Ownership",
                table: "Expenditure",
                newName: "IX_Expenditure_SharedWithId");

            migrationBuilder.RenameIndex(
                name: "IX_BillDetails_BillId1",
                schema: "Ownership",
                table: "Expenditure",
                newName: "IX_Expenditure_BillId1");

            migrationBuilder.RenameIndex(
                name: "IX_BillDetails_BillId",
                schema: "Ownership",
                table: "Expenditure",
                newName: "IX_Expenditure_BillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenditure",
                schema: "Bills",
                table: "Expenditure",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenditure",
                schema: "Ownership",
                table: "Expenditure",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenditure_AspNetUsers_PaidById",
                schema: "Bills",
                table: "Expenditure",
                column: "PaidById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenditure_Organizations_OrganizationId",
                schema: "Bills",
                table: "Expenditure",
                column: "OrganizationId",
                principalSchema: "Ownership",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenditure_AspNetUsers_SharedWithId",
                schema: "Ownership",
                table: "Expenditure",
                column: "SharedWithId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenditure_Expenditure_BillId",
                schema: "Ownership",
                table: "Expenditure",
                column: "BillId",
                principalSchema: "Bills",
                principalTable: "Expenditure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenditure_Expenditure_BillId1",
                schema: "Ownership",
                table: "Expenditure",
                column: "BillId1",
                principalSchema: "Bills",
                principalTable: "Expenditure",
                principalColumn: "Id");
        }
    }
}
