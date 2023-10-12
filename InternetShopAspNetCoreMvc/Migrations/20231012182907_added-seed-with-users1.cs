using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternetShopAspNetCoreMvc.Migrations
{
    /// <inheritdoc />
    public partial class addedseedwithusers1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 12, 18, 29, 7, 330, DateTimeKind.Utc).AddTicks(7717));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 12, 18, 29, 7, 330, DateTimeKind.Utc).AddTicks(7735));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 12, 17, 11, 56, 291, DateTimeKind.Utc).AddTicks(7160));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 12, 17, 11, 56, 291, DateTimeKind.Utc).AddTicks(7176));
        }
    }
}
