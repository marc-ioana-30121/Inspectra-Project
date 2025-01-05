using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrettyNeatGenericAPI.Migrations
{
    /// <inheritdoc />
    public partial class DatesForCheckedBags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateIssued",
                table: "CheckedBagsLogs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequestedDate",
                table: "CheckedBagsLogs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "CheckedBagsLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateIssued",
                table: "CheckedBagsLogs");

            migrationBuilder.DropColumn(
                name: "RequestedDate",
                table: "CheckedBagsLogs");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "CheckedBagsLogs");
        }
    }
}
