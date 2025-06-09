using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetProject.Migrations
{
    /// <inheritdoc />
    public partial class AddPartsToTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceTaskParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceTaskId = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTaskParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceTaskParts_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceTaskParts_ServiceTasks_ServiceTaskId",
                        column: x => x.ServiceTaskId,
                        principalTable: "ServiceTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTaskParts_PartId",
                table: "ServiceTaskParts",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTaskParts_ServiceTaskId",
                table: "ServiceTaskParts",
                column: "ServiceTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceTaskParts");
        }
    }
}
