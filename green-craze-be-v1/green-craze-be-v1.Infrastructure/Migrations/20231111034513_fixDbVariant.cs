using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace green_craze_be_v1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixDbVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPromotionalPrice",
                table: "Variants",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "Name", "NormalizedName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "15e0e2d1-c1fb-498c-a38c-ef37ca5cda4c", "31c433b2-a859-49fa-95d6-4af885716b0a", new DateTime(2023, 11, 11, 10, 45, 13, 206, DateTimeKind.Local).AddTicks(8016), "System", "STAFF", "STAFF", null, null },
                    { "6fa6266c-7089-4198-8944-cd2765354f81", "2b2f6ec1-9172-410c-b1fc-471c0f42e751", new DateTime(2023, 11, 11, 10, 45, 13, 206, DateTimeKind.Local).AddTicks(8016), "System", "USER", "USER", null, null },
                    { "df3661e9-daba-4655-9eda-dc19a9fa740d", "d60ca3cc-871a-4b1a-9915-90684e8c1fe3", new DateTime(2023, 11, 11, 10, 45, 13, 206, DateTimeKind.Local).AddTicks(8016), "System", "ADMIN", "ADMIN", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "15e0e2d1-c1fb-498c-a38c-ef37ca5cda4c");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "6fa6266c-7089-4198-8944-cd2765354f81");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "df3661e9-daba-4655-9eda-dc19a9fa740d");

            migrationBuilder.DropColumn(
                name: "TotalPromotionalPrice",
                table: "Variants");

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
    }
}
