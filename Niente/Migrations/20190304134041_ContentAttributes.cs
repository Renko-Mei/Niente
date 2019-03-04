using Microsoft.EntityFrameworkCore.Migrations;

namespace Niente.Migrations
{
    public partial class ContentAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayLevel",
                table: "Articles",
                nullable: false,
                defaultValue: "Default");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Articles",
                nullable: false,
                defaultValue: "Universal");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Articles",
                nullable: false,
                defaultValue: "Visible");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayLevel",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Articles");
        }
    }
}
