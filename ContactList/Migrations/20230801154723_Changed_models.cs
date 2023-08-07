using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactList.Migrations
{
    /// <inheritdoc />
    public partial class Changed_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "ContactListUser",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHashSalt",
                table: "ContactListUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ContactListUser",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordHashSalt", "VerifiedAt" },
                values: new object[] { new DateTime(2023, 8, 1, 18, 47, 23, 361, DateTimeKind.Local).AddTicks(3687), "$2a$16$g5SOGxCee8G5xTPKzWpgQ.QqAq6iHzXUwyKoGCvY3dyErjqbUNjpu", "$2a$16$g5SOGxCee8G5xTPKzWpgQ.", new DateTime(2023, 8, 1, 18, 47, 23, 361, DateTimeKind.Local).AddTicks(3759) });

            migrationBuilder.CreateIndex(
                name: "IX_ContactListUser_Login",
                table: "ContactListUser",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContactListUser_Login",
                table: "ContactListUser");

            migrationBuilder.DropColumn(
                name: "PasswordHashSalt",
                table: "ContactListUser");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "ContactListUser",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "ContactListUser",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "VerifiedAt" },
                values: new object[] { new DateTime(2023, 8, 1, 16, 2, 47, 286, DateTimeKind.Local).AddTicks(7325), "john_doe", new DateTime(2023, 8, 1, 13, 2, 47, 286, DateTimeKind.Utc).AddTicks(7405) });
        }
    }
}
