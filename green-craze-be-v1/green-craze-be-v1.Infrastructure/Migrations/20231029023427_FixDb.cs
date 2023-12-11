using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace green_craze_be_v1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "bc104b77-9fe1-4e5a-a506-20fcbc43d077");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "f754f820-b8c1-4d85-b944-be646359eec1");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "fcb73747-4a36-4e57-9884-e0c25cf38615");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Dockets");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "Name", "NormalizedName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "5d1633fd-34bc-42c3-91a8-21befdc2e148", "83de4b8f-7d55-41ce-8bf5-f167eff06b1d", new DateTime(2023, 10, 29, 9, 34, 26, 816, DateTimeKind.Local).AddTicks(2571), "System", "ADMIN", "ADMIN", null, null },
                    { "724d872c-89ca-47ae-a25c-6de48604210b", "22504c33-2925-48f1-965d-761da767cade", new DateTime(2023, 10, 29, 9, 34, 26, 816, DateTimeKind.Local).AddTicks(2571), "System", "USER", "USER", null, null },
                    { "a6f7805f-6a33-4012-add5-b7de13005edb", "18328461-43d5-41b9-9653-78cc950ef2e3", new DateTime(2023, 10, 29, 9, 34, 26, 816, DateTimeKind.Local).AddTicks(2571), "System", "STAFF", "STAFF", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "5d1633fd-34bc-42c3-91a8-21befdc2e148");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "724d872c-89ca-47ae-a25c-6de48604210b");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "a6f7805f-6a33-4012-add5-b7de13005edb");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Dockets",
                type: "longtext",
                nullable: false);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "Name", "NormalizedName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "bc104b77-9fe1-4e5a-a506-20fcbc43d077", "13e00ad9-d071-4874-8634-76d5397ec768", new DateTime(2023, 10, 28, 11, 29, 59, 814, DateTimeKind.Local).AddTicks(2098), "System", "STAFF", "STAFF", null, null },
                    { "f754f820-b8c1-4d85-b944-be646359eec1", "67136d79-b4b5-4ded-b7d0-e0e4b58e741a", new DateTime(2023, 10, 28, 11, 29, 59, 814, DateTimeKind.Local).AddTicks(2098), "System", "USER", "USER", null, null },
                    { "fcb73747-4a36-4e57-9884-e0c25cf38615", "857b6441-524e-493b-b8e4-e884b1e11327", new DateTime(2023, 10, 28, 11, 29, 59, 814, DateTimeKind.Local).AddTicks(2098), "System", "ADMIN", "ADMIN", null, null }
                });
        }
    }
}
