using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PrettyNeatGenericAPI.Migrations
{
    /// <inheritdoc />
    public partial class QualifiersAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Qualifiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResponsiblePerson = table.Column<string>(type: "text", nullable: true),
                    DateOfIssue = table.Column<string>(type: "text", nullable: true),
                    PartNumber = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    TssCode = table.Column<string>(type: "text", nullable: true),
                    ChargePerPiece = table.Column<float>(type: "real", nullable: true),
                    PayPerPice = table.Column<float>(type: "real", nullable: true),
                    ChargePerPieceDelay = table.Column<float>(type: "real", nullable: true),
                    PayPerPieceDelay = table.Column<float>(type: "real", nullable: true),
                    ChargePerPieceExtraDelay = table.Column<float>(type: "real", nullable: true),
                    PayPerPieceExtraDelay = table.Column<float>(type: "real", nullable: true),
                    EmployeePiecesPerHour = table.Column<float>(type: "real", nullable: true),
                    ClientPiecesPerHour = table.Column<float>(type: "real", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualifiers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Qualifiers");
        }
    }
}
