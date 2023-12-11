using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace green_craze_be_v1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixdbsale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "All",
                table: "Sales",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "Name", "NormalizedName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "283ad9d7-453b-4f62-81af-08dd52209b03", "199e4bd4-939d-4a2f-9537-c08c0a6e6d3c", new DateTime(2023, 11, 4, 11, 55, 55, 54, DateTimeKind.Local).AddTicks(7806), "System", "USER", "USER", null, null },
                    { "a0149560-08d7-44d0-86ae-108fa8ec9610", "e3a39443-61a6-439c-9175-f1f088f5c944", new DateTime(2023, 11, 4, 11, 55, 55, 54, DateTimeKind.Local).AddTicks(7806), "System", "STAFF", "STAFF", null, null },
                    { "d1161d5c-0c42-4e46-b087-15abe136ab91", "8ec1c67b-4c8c-425f-8819-d978b3ebca75", new DateTime(2023, 11, 4, 11, 55, 55, 54, DateTimeKind.Local).AddTicks(7806), "System", "ADMIN", "ADMIN", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "283ad9d7-453b-4f62-81af-08dd52209b03");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "a0149560-08d7-44d0-86ae-108fa8ec9610");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "d1161d5c-0c42-4e46-b087-15abe136ab91");

            migrationBuilder.DropColumn(
                name: "All",
                table: "Sales");

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
    }
}
