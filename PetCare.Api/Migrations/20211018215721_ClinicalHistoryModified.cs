using Microsoft.EntityFrameworkCore.Migrations;

namespace PetCare.Api.Migrations
{
    public partial class ClinicalHistoryModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicalHistories_Pets_petId",
                table: "ClinicalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Details_ClinicalHistories_HistoryId",
                table: "Details");

            migrationBuilder.RenameColumn(
                name: "HistoryId",
                table: "Details",
                newName: "ClinicalHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Details_HistoryId",
                table: "Details",
                newName: "IX_Details_ClinicalHistoryId");

            migrationBuilder.RenameColumn(
                name: "petId",
                table: "ClinicalHistories",
                newName: "PetId");

            migrationBuilder.RenameIndex(
                name: "IX_ClinicalHistories_petId",
                table: "ClinicalHistories",
                newName: "IX_ClinicalHistories_PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicalHistories_Pets_PetId",
                table: "ClinicalHistories",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Details_ClinicalHistories_ClinicalHistoryId",
                table: "Details",
                column: "ClinicalHistoryId",
                principalTable: "ClinicalHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicalHistories_Pets_PetId",
                table: "ClinicalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Details_ClinicalHistories_ClinicalHistoryId",
                table: "Details");

            migrationBuilder.RenameColumn(
                name: "ClinicalHistoryId",
                table: "Details",
                newName: "HistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Details_ClinicalHistoryId",
                table: "Details",
                newName: "IX_Details_HistoryId");

            migrationBuilder.RenameColumn(
                name: "PetId",
                table: "ClinicalHistories",
                newName: "petId");

            migrationBuilder.RenameIndex(
                name: "IX_ClinicalHistories_PetId",
                table: "ClinicalHistories",
                newName: "IX_ClinicalHistories_petId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicalHistories_Pets_petId",
                table: "ClinicalHistories",
                column: "petId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Details_ClinicalHistories_HistoryId",
                table: "Details",
                column: "HistoryId",
                principalTable: "ClinicalHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
