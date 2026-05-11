using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCongTrinhXayDung.Migrations
{
    /// <inheritdoc />
    public partial class AddHinhAnhBaoCao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HinhAnh",
                table: "BaoCaoTienDos");

            migrationBuilder.CreateTable(
                name: "HinhAnhBaoCaos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BaoCaoTienDoId = table.Column<int>(type: "INTEGER", nullable: false),
                    DuongDan = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhBaoCaos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HinhAnhBaoCaos_BaoCaoTienDos_BaoCaoTienDoId",
                        column: x => x.BaoCaoTienDoId,
                        principalTable: "BaoCaoTienDos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhBaoCaos_BaoCaoTienDoId",
                table: "HinhAnhBaoCaos",
                column: "BaoCaoTienDoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HinhAnhBaoCaos");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnh",
                table: "BaoCaoTienDos",
                type: "TEXT",
                maxLength: 500,
                nullable: true);
        }
    }
}
