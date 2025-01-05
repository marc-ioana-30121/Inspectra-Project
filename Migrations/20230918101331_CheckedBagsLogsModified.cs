using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrettyNeatGenericAPI.Migrations
{
    /// <inheritdoc />
    public partial class CheckedBagsLogsModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ChargePrice",
                table: "CheckedBagsLogs",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChitNumber",
                table: "CheckedBagsLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChargePrice",
                table: "CheckedBagsLogs");

            migrationBuilder.DropColumn(
                name: "ChitNumber",
                table: "CheckedBagsLogs");
        }
    }
}
