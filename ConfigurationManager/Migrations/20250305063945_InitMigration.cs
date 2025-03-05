using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigurationManager.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CurrentVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateAdded = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConfigurationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<string>(type: "jsonb", nullable: false),
                    DateAdded = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigurationVersions_Configurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "Configurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationVersions_ConfigurationId",
                table: "ConfigurationVersions",
                column: "ConfigurationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Configurations_Name_UserId",
                table: "Configurations",
                columns: new[] { "Name", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigurationVersions");

            migrationBuilder.DropTable(
                name: "Configurations");
        }
    }
}
