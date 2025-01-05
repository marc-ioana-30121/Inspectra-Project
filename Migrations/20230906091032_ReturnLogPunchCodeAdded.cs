using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrettyNeatGenericAPI.Migrations
{
    /// <inheritdoc />
    public partial class ReturnLogPunchCodeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PunchCode",
                table: "ReturnLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PunchCode",
                table: "ReturnLogs");
        }
    }
}
