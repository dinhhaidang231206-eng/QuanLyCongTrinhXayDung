using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCongTrinhXayDung.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBaoCaoTienDoV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NguoiBaoCao",
                table: "BaoCaoTienDos");

            migrationBuilder.AddColumn<int>(
                name: "ThanhVienId",
                table: "BaoCaoTienDos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TieuDe",
                table: "BaoCaoTienDos",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TraLoi",
                table: "BaoCaoTienDos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UuTien",
                table: "BaoCaoTienDos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoTienDos_ThanhVienId",
                table: "BaoCaoTienDos",
                column: "ThanhVienId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaoCaoTienDos_ThanhViens_ThanhVienId",
                table: "BaoCaoTienDos",
                column: "ThanhVienId",
                principalTable: "ThanhViens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaoCaoTienDos_ThanhViens_ThanhVienId",
                table: "BaoCaoTienDos");

            migrationBuilder.DropIndex(
                name: "IX_BaoCaoTienDos_ThanhVienId",
                table: "BaoCaoTienDos");

            migrationBuilder.DropColumn(
                name: "ThanhVienId",
                table: "BaoCaoTienDos");

            migrationBuilder.DropColumn(
                name: "TieuDe",
                table: "BaoCaoTienDos");

            migrationBuilder.DropColumn(
                name: "TraLoi",
                table: "BaoCaoTienDos");

            migrationBuilder.DropColumn(
                name: "UuTien",
                table: "BaoCaoTienDos");

            migrationBuilder.AddColumn<string>(
                name: "NguoiBaoCao",
                table: "BaoCaoTienDos",
                type: "TEXT",
                maxLength: 255,
                nullable: true);
        }
    }
}
