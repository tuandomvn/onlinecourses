using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acme.OnlineCourses.Migrations
{
    /// <inheritdoc />
    public partial class AddOrgNameToAgency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrgName",
                table: "Agencies",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrgName",
                table: "Agencies");
        }
    }
}
