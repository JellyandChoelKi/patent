using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace K2GGTT.Migrations
{
    public partial class AddHotTech : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotTech",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Overview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackgroundAndUnmetNeed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TheSolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TechnologyEssence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationsAndAdvantages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DevelopmentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    References = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicantImgSrc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicantMajor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotTech", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotTech");
        }
    }
}
