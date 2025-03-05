using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigurationManager.Migrations
{
    /// <inheritdoc />
    public partial class AddedBaseConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "ConfigurationVersions");

            migrationBuilder.AddColumn<string>(
                name: "ConfigurationData",
                table: "ConfigurationVersions",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConfigurationType",
                table: "ConfigurationVersions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfigurationData",
                table: "ConfigurationVersions");

            migrationBuilder.DropColumn(
                name: "ConfigurationType",
                table: "ConfigurationVersions");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "ConfigurationVersions",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }
    }
}
