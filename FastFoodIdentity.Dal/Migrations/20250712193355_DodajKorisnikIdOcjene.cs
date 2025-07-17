using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFoodIdentity.Dal.Migrations
{
    public partial class DodajKorisnikIdOcjene : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KorisnikId",
                table: "OcjenaJela",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "OcjenaJela");
        }
    }
}
