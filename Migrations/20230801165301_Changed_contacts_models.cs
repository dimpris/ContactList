using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactList.Migrations
{
    /// <inheritdoc />
    public partial class Changed_contacts_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ContactListUser",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordHashSalt", "VerifiedAt" },
                values: new object[] { new DateTime(2023, 8, 1, 19, 53, 1, 389, DateTimeKind.Local).AddTicks(2879), "$2a$16$.2Hhf9fcMwf64L.0Xmnwb.Y2QORtgML/.DVCSQUPWSnz1h1JA1eqy", "$2a$16$.2Hhf9fcMwf64L.0Xmnwb.", new DateTime(2023, 8, 1, 19, 53, 1, 389, DateTimeKind.Local).AddTicks(2938) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ContactListUser",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordHashSalt", "VerifiedAt" },
                values: new object[] { new DateTime(2023, 8, 1, 18, 47, 23, 361, DateTimeKind.Local).AddTicks(3687), "$2a$16$g5SOGxCee8G5xTPKzWpgQ.QqAq6iHzXUwyKoGCvY3dyErjqbUNjpu", "$2a$16$g5SOGxCee8G5xTPKzWpgQ.", new DateTime(2023, 8, 1, 18, 47, 23, 361, DateTimeKind.Local).AddTicks(3759) });
        }
    }
}
