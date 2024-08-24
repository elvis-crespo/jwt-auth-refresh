using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWT_AuthAndRefrest.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    RefreshTokenS = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EsActivo = table.Column<bool>(type: "bit", nullable: true, computedColumnSql: "(case when [ExpirationDate]<getdate() then CONVERT([bit],(0)) else CONVERT([bit], (1)) end)", stored: false),
                    IdUser = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");
        }
    }
}
