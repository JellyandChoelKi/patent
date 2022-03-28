using Microsoft.EntityFrameworkCore.Migrations;

namespace K2GGTT.Migrations
{
    public partial class AddColumnMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Jobtitle",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "Jobtitle",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Member");
        }
    }
}
