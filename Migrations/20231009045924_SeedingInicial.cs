using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWT_AuthAndRefrest.Migrations
{
    /// <inheritdoc />
    public partial class SeedingInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Key", "NameUser" },
                values: new object[] { 1, "123", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
