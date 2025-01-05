using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrettyNeatGenericAPI.Migrations
{
    /// <inheritdoc />
    public partial class LastAssignedTo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastAssignedTo",
                table: "BagInventory",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastAssignedTo",
                table: "BagInventory");
        }
    }
}
