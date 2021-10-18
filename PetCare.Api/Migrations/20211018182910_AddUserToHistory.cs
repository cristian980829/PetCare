using Microsoft.EntityFrameworkCore.Migrations;

namespace PetCare.Api.Migrations
{
    public partial class AddUserToHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ClinicalHistories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalHistories_UserId",
                table: "ClinicalHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicalHistories_AspNetUsers_UserId",
                table: "ClinicalHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicalHistories_AspNetUsers_UserId",
                table: "ClinicalHistories");

            migrationBuilder.DropIndex(
                name: "IX_ClinicalHistories_UserId",
                table: "ClinicalHistories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ClinicalHistories");
        }
    }
}
