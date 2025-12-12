using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedNewUserWithRoleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "a8c67410-72e1-47be-9fbb-41bc49f7e9eb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "353677d8-2c60-4b82-aca4-6285e3d7187e");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "LockoutEnabled", "LockoutEnd", "MiddleName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "SurName", "TwoFactorEnabled", "UserName" },
                values: new object[] { "bffa69b1-e355-4dc6-b98e-691ebb771cbd", 0, "47b31a50-0f46-4895-afa7-8bfc62e5289a", "Users", "darvin@email.com", false, "Darvin", false, null, "Oma", "DARVIN@EMAIL.COM", "DARVIN123", "AQAAAAIAAYagAAAAEEwhOUrjmAklexgInZaqAJakKF4Pj+gDmLWrQipLRTWMgk/dBqcN2G3+5MeCreBFhw==", null, false, "a9346043-c694-44ed-9528-cf68fda7279a", "Palma", false, "darvin123" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "bffa69b1-e355-4dc6-b98e-691ebb771cbd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "bffa69b1-e355-4dc6-b98e-691ebb771cbd" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bffa69b1-e355-4dc6-b98e-691ebb771cbd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "f02fb6c5-e675-4a25-832f-52450711ddc6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "e2e6f6d6-9f96-4e1c-a55a-65303da2e5fb");
        }
    }
}
