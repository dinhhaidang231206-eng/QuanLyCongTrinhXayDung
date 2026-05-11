using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCongTrinhXayDung.Migrations
{
    /// <inheritdoc />
    public partial class AddLoaiDichVuKhac : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoaiDichVuKhac",
                table: "DichVus",
                type: "TEXT",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoaiDichVuKhac",
                table: "DichVus");
        }
    }
}
