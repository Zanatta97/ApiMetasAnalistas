using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiMetasAnalistas.Migrations
{
    /// <inheritdoc />
    public partial class altera_nomes_tabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_analysts_regions_RegiaoId",
                table: "analysts");

            migrationBuilder.DropForeignKey(
                name: "FK_holidays_regions_RegiaoId",
                table: "holidays");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_analysts_AnalystId",
                table: "tickets");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tickets",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "DataFechamento",
                table: "tickets",
                newName: "data_fechamento");

            migrationBuilder.RenameColumn(
                name: "AnalystId",
                table: "tickets",
                newName: "analista_id");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_AnalystId",
                table: "tickets",
                newName: "IX_tickets_analista_id");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "regions",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "regions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "holidays",
                newName: "descricao");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "holidays",
                newName: "data");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "holidays",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RegiaoId",
                table: "holidays",
                newName: "regiao_id");

            migrationBuilder.RenameIndex(
                name: "IX_holidays_RegiaoId",
                table: "holidays",
                newName: "IX_holidays_regiao_id");

            migrationBuilder.RenameColumn(
                name: "Usuario",
                table: "analysts",
                newName: "usuario");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "analysts",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "analysts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RegiaoId",
                table: "analysts",
                newName: "regiao_id");

            migrationBuilder.RenameColumn(
                name: "MetaDiaria",
                table: "analysts",
                newName: "meta_diaria");

            migrationBuilder.RenameIndex(
                name: "IX_analysts_RegiaoId",
                table: "analysts",
                newName: "IX_analysts_regiao_id");

            migrationBuilder.AddForeignKey(
                name: "FK_analysts_regions_regiao_id",
                table: "analysts",
                column: "regiao_id",
                principalTable: "regions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_holidays_regions_regiao_id",
                table: "holidays",
                column: "regiao_id",
                principalTable: "regions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_analysts_analista_id",
                table: "tickets",
                column: "analista_id",
                principalTable: "analysts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_analysts_regions_regiao_id",
                table: "analysts");

            migrationBuilder.DropForeignKey(
                name: "FK_holidays_regions_regiao_id",
                table: "holidays");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_analysts_analista_id",
                table: "tickets");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tickets",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "data_fechamento",
                table: "tickets",
                newName: "DataFechamento");

            migrationBuilder.RenameColumn(
                name: "analista_id",
                table: "tickets",
                newName: "AnalystId");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_analista_id",
                table: "tickets",
                newName: "IX_tickets_AnalystId");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "regions",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "regions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "descricao",
                table: "holidays",
                newName: "Descricao");

            migrationBuilder.RenameColumn(
                name: "data",
                table: "holidays",
                newName: "Data");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "holidays",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "regiao_id",
                table: "holidays",
                newName: "RegiaoId");

            migrationBuilder.RenameIndex(
                name: "IX_holidays_regiao_id",
                table: "holidays",
                newName: "IX_holidays_RegiaoId");

            migrationBuilder.RenameColumn(
                name: "usuario",
                table: "analysts",
                newName: "Usuario");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "analysts",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "analysts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "regiao_id",
                table: "analysts",
                newName: "RegiaoId");

            migrationBuilder.RenameColumn(
                name: "meta_diaria",
                table: "analysts",
                newName: "MetaDiaria");

            migrationBuilder.RenameIndex(
                name: "IX_analysts_regiao_id",
                table: "analysts",
                newName: "IX_analysts_RegiaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_analysts_regions_RegiaoId",
                table: "analysts",
                column: "RegiaoId",
                principalTable: "regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_holidays_regions_RegiaoId",
                table: "holidays",
                column: "RegiaoId",
                principalTable: "regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_analysts_AnalystId",
                table: "tickets",
                column: "AnalystId",
                principalTable: "analysts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
