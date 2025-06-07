using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // === KROK 1: Usunięcie starych kluczy głównych (Primary Keys) ===
        // Musimy je usunąć, aby móc zmodyfikować kolumny, które są ich częścią.
        migrationBuilder.DropPrimaryKey(
            name: "PK_AspNetUserTokens",
            table: "AspNetUserTokens");

        migrationBuilder.DropPrimaryKey(
            name: "PK_AspNetUserLogins",
            table: "AspNetUserLogins");

        // === KROK 2: Modyfikacja kolumn (Twój oryginalny kod) ===
        // Te operacje teraz się powiodą, bo kolumny nie są już zablokowane przez klucze główne.
        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "AspNetUserTokens",
            type: "nvarchar(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AlterColumn<string>(
            name: "LoginProvider",
            table: "AspNetUserTokens",
            type: "nvarchar(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AlterColumn<string>(
            name: "ProviderKey",
            table: "AspNetUserLogins",
            type: "nvarchar(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AlterColumn<string>(
            name: "LoginProvider",
            table: "AspNetUserLogins",
            type: "nvarchar(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        // === KROK 3: Odtworzenie kluczy głównych ===
        // Po modyfikacji kolumn, tworzymy klucze główne na nowo.
        migrationBuilder.AddPrimaryKey(
            name: "PK_AspNetUserTokens",
            table: "AspNetUserTokens",
            columns: new[] { "UserId", "LoginProvider", "Name" });

        migrationBuilder.AddPrimaryKey(
            name: "PK_AspNetUserLogins",
            table: "AspNetUserLogins",
            columns: new[] { "LoginProvider", "ProviderKey" });
    }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);
        }
    }
}
