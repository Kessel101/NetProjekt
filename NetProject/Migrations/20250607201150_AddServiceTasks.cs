using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetProject.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTasks_ServiceOrders_ServiceOrderId",
                table: "ServiceTasks");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ServiceTasks",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceOrderId",
                table: "ServiceTasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "WorkOrderId",
                table: "ServiceTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTasks_WorkOrderId",
                table: "ServiceTasks",
                column: "WorkOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTasks_ServiceOrders_ServiceOrderId",
                table: "ServiceTasks",
                column: "ServiceOrderId",
                principalTable: "ServiceOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTasks_WorkOrders_WorkOrderId",
                table: "ServiceTasks",
                column: "WorkOrderId",
                principalTable: "WorkOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTasks_ServiceOrders_ServiceOrderId",
                table: "ServiceTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTasks_WorkOrders_WorkOrderId",
                table: "ServiceTasks");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTasks_WorkOrderId",
                table: "ServiceTasks");

            migrationBuilder.DropColumn(
                name: "WorkOrderId",
                table: "ServiceTasks");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ServiceTasks",
                newName: "Description");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceOrderId",
                table: "ServiceTasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTasks_ServiceOrders_ServiceOrderId",
                table: "ServiceTasks",
                column: "ServiceOrderId",
                principalTable: "ServiceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
