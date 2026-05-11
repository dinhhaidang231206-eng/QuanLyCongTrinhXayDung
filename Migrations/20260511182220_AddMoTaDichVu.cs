using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCongTrinhXayDung.Migrations
{
    /// <inheritdoc />
    public partial class AddMoTaDichVu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MoTa",
                table: "DichVus",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoTa",
                table: "DichVus");
        }
    }
}
