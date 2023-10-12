using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InternetShopAspNetCoreMvc.Migrations
{
    /// <inheritdoc />
    public partial class addedseedwithusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "Fullname", "Username" },
                values: new object[,]
                {
                    { 1, "test-1", new DateTime(2023, 10, 12, 17, 11, 56, 291, DateTimeKind.Utc).AddTicks(7160), "vasya@gmail.com", "Vasya Pupkin", "Vasya" },
                    { 2, "test-2", new DateTime(2023, 10, 12, 17, 11, 56, 291, DateTimeKind.Utc).AddTicks(7176), "petya@gmail.com", "Petya Pupkin", "Petya" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
