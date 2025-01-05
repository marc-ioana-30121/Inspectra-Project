using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrettyNeatGenericAPI.Migrations
{
    /// <inheritdoc />
    public partial class ReturnLogsModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountReturned",
                table: "ReturnLogs");

            migrationBuilder.AddColumn<string>(
                name: "Fault",
                table: "ReturnLogs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fault",
                table: "ReturnLogs");

            migrationBuilder.AddColumn<int>(
                name: "CountReturned",
                table: "ReturnLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
