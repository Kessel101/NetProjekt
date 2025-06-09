using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetProject.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityAndTypeToParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsedParts_Parts_PartId",
                table: "UsedParts");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedParts_Parts_PartId",
                table: "UsedParts",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsedParts_Parts_PartId",
                table: "UsedParts");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedParts_Parts_PartId",
                table: "UsedParts",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
