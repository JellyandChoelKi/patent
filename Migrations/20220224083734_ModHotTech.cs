using Microsoft.EntityFrameworkCore.Migrations;

namespace K2GGTT.Migrations
{
    public partial class ModHotTech : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationsAndAdvantages",
                table: "HotTech");

            migrationBuilder.DropColumn(
                name: "BackgroundAndUnmetNeed",
                table: "HotTech");

            migrationBuilder.DropColumn(
                name: "DevelopmentStatus",
                table: "HotTech");

            migrationBuilder.DropColumn(
                name: "Overview",
                table: "HotTech");

            migrationBuilder.DropColumn(
                name: "PatentStatus",
                table: "HotTech");

            migrationBuilder.DropColumn(
                name: "References",
                table: "HotTech");

            migrationBuilder.DropColumn(
                name: "TechnologyEssence",
                table: "HotTech");

            migrationBuilder.RenameColumn(
                name: "TheSolution",
                table: "HotTech",
                newName: "Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "HotTech",
                newName: "TheSolution");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationsAndAdvantages",
                table: "HotTech",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundAndUnmetNeed",
                table: "HotTech",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DevelopmentStatus",
                table: "HotTech",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Overview",
                table: "HotTech",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatentStatus",
                table: "HotTech",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "References",
                table: "HotTech",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnologyEssence",
                table: "HotTech",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
