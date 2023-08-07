using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactList.Migrations
{
    /// <inheritdoc />
    public partial class Added_password_reset_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PasswordResets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResetCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResets_ContactListUser_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "ContactListUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ContactListUser",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordHashSalt", "VerifiedAt" },
                values: new object[] { new DateTime(2023, 8, 3, 18, 45, 42, 138, DateTimeKind.Local).AddTicks(1370), "$2a$16$4AmKk10nKnNT5T1dRTZRdeKtLBNZNr1TI9t89ykrCzuK2ebQ/eOU6", "$2a$16$4AmKk10nKnNT5T1dRTZRde", new DateTime(2023, 8, 3, 18, 45, 42, 138, DateTimeKind.Local).AddTicks(1439) });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResets_OwnerId",
                table: "PasswordResets",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordResets");

            migrationBuilder.UpdateData(
                table: "ContactListUser",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordHashSalt", "VerifiedAt" },
                values: new object[] { new DateTime(2023, 8, 1, 19, 53, 1, 389, DateTimeKind.Local).AddTicks(2879), "$2a$16$.2Hhf9fcMwf64L.0Xmnwb.Y2QORtgML/.DVCSQUPWSnz1h1JA1eqy", "$2a$16$.2Hhf9fcMwf64L.0Xmnwb.", new DateTime(2023, 8, 1, 19, 53, 1, 389, DateTimeKind.Local).AddTicks(2938) });
        }
    }
}
