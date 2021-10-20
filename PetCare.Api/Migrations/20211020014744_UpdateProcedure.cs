using Microsoft.EntityFrameworkCore.Migrations;

namespace PetCare.Api.Migrations
{
    public partial class UpdateProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicineId",
                table: "Details",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Details_MedicineId",
                table: "Details",
                column: "MedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Medicines_MedicineId",
                table: "Details",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Details_Medicines_MedicineId",
                table: "Details");

            migrationBuilder.DropIndex(
                name: "IX_Details_MedicineId",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "MedicineId",
                table: "Details");
        }
    }
}
