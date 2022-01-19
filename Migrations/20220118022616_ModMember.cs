using Microsoft.EntityFrameworkCore.Migrations;

namespace K2GGTT.Migrations
{
    public partial class ModMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Addr1",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Addr2",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyRegistrationNumber",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gubun",
                table: "Member",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Representative",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxInvoiceOfficer",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zipcode",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Addr1",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "Addr2",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "CompanyRegistrationNumber",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "Gubun",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "Representative",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "TaxInvoiceOfficer",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "Zipcode",
                table: "Member");
        }
    }
}
