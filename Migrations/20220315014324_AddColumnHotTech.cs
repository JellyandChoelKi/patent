using Microsoft.EntityFrameworkCore.Migrations;

namespace K2GGTT.Migrations
{
    public partial class AddColumnHotTech : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gubun",
                table: "HotTech",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gubun",
                table: "HotTech");
        }
    }
}
