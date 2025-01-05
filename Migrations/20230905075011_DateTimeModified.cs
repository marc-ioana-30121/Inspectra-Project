using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrettyNeatGenericAPI.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AssignedDate",
                table: "BagInventory",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedDate",
                table: "BagInventory",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckedDate",
                table: "BagInventory");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "AssignedDate",
                table: "BagInventory",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }
    }
}
