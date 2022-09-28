using Microsoft.EntityFrameworkCore.Migrations;

namespace eAgenda.Infra.Orm.Migrations
{
    public partial class AlterandoCampoLocalParaLocalizacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Local",
                table: "TBCompromisso",
                newName: "Localizacao");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Localizacao",
                table: "TBCompromisso",
                newName: "Local");
        }
    }
}
