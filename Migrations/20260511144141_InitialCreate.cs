using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCongTrinhXayDung.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChucVus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenChucVu = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucVus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CongTrinhs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenCongTrinh = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    DiaDiem = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ThoiGianBatDau = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ThoiGianKetThuc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ThoiGianHoanThanh = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NguoiPhuTrach = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    NguoiGiamSat = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    TrangThai = table.Column<int>(type: "INTEGER", nullable: false),
                    ChiPhiDuKien = table.Column<decimal>(type: "TEXT", nullable: false),
                    ChiPhiThucTe = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongTrinhs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThanhViens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    HoTen = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    TenDangNhap = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    MatKhau = table.Column<string>(type: "TEXT", nullable: false),
                    ChucVuId = table.Column<int>(type: "INTEGER", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TrangThai = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhViens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThanhViens_ChucVus_ChucVuId",
                        column: x => x.ChucVuId,
                        principalTable: "ChucVus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GiaiDoans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenGiaiDoan = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    CongTrinhId = table.Column<int>(type: "INTEGER", nullable: false),
                    ThoiGianBatDau = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ThoiGianKetThuc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ThoiGianHoanThanh = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TrangThai = table.Column<int>(type: "INTEGER", nullable: false),
                    ChiPhiDuKien = table.Column<decimal>(type: "TEXT", nullable: false),
                    ChiPhiThucTe = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiaiDoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiaiDoans_CongTrinhs_CongTrinhId",
                        column: x => x.CongTrinhId,
                        principalTable: "CongTrinhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResetMatKhaus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Token = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ThanhVienId = table.Column<int>(type: "INTEGER", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HanSuDung = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DaSuDung = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetMatKhaus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResetMatKhaus_ThanhViens_ThanhVienId",
                        column: x => x.ThanhVienId,
                        principalTable: "ThanhViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaoCaoTienDos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GiaiDoanId = table.Column<int>(type: "INTEGER", nullable: false),
                    NgayBaoCao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NoiDung = table.Column<string>(type: "TEXT", nullable: false),
                    HinhAnh = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    NguoiBaoCao = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaoTienDos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaoCaoTienDos_GiaiDoans_GiaiDoanId",
                        column: x => x.GiaiDoanId,
                        principalTable: "GiaiDoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DichVus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LoaiDichVu = table.Column<int>(type: "INTEGER", nullable: false),
                    GiaiDoanId = table.Column<int>(type: "INTEGER", nullable: false),
                    NhaCungCap = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DonGia = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoLuong = table.Column<double>(type: "REAL", nullable: false),
                    ThoiGianBatDau = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ThoiGianKetThuc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TrangThai = table.Column<int>(type: "INTEGER", nullable: false),
                    MucDichSuDung = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DichVus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DichVus_GiaiDoans_GiaiDoanId",
                        column: x => x.GiaiDoanId,
                        principalTable: "GiaiDoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MayMocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenMay = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    GiaiDoanId = table.Column<int>(type: "INTEGER", nullable: false),
                    SoLuong = table.Column<int>(type: "INTEGER", nullable: false),
                    DonGia = table.Column<decimal>(type: "TEXT", nullable: false),
                    ThoiGianBatDau = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ThoiGianKetThuc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TrangThai = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MayMocs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MayMocs_GiaiDoans_GiaiDoanId",
                        column: x => x.GiaiDoanId,
                        principalTable: "GiaiDoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhanCongs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HoTen = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    GiaiDoanId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChucVuId = table.Column<int>(type: "INTEGER", nullable: false),
                    Luong = table.Column<decimal>(type: "TEXT", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanCongs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhanCongs_ChucVus_ChucVuId",
                        column: x => x.ChucVuId,
                        principalTable: "ChucVus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NhanCongs_GiaiDoans_GiaiDoanId",
                        column: x => x.GiaiDoanId,
                        principalTable: "GiaiDoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VatLieus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenVatLieu = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    GiaiDoanId = table.Column<int>(type: "INTEGER", nullable: false),
                    SoLuong = table.Column<double>(type: "REAL", nullable: false),
                    DonGia = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VatLieus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VatLieus_GiaiDoans_GiaiDoanId",
                        column: x => x.GiaiDoanId,
                        principalTable: "GiaiDoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChamCongs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NhanCongId = table.Column<int>(type: "INTEGER", nullable: false),
                    Ngay = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SoCong = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChamCongs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChamCongs_NhanCongs_NhanCongId",
                        column: x => x.NhanCongId,
                        principalTable: "NhanCongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoTienDos_GiaiDoanId",
                table: "BaoCaoTienDos",
                column: "GiaiDoanId");

            migrationBuilder.CreateIndex(
                name: "IX_ChamCongs_NhanCongId",
                table: "ChamCongs",
                column: "NhanCongId");

            migrationBuilder.CreateIndex(
                name: "IX_DichVus_GiaiDoanId",
                table: "DichVus",
                column: "GiaiDoanId");

            migrationBuilder.CreateIndex(
                name: "IX_GiaiDoans_CongTrinhId",
                table: "GiaiDoans",
                column: "CongTrinhId");

            migrationBuilder.CreateIndex(
                name: "IX_MayMocs_GiaiDoanId",
                table: "MayMocs",
                column: "GiaiDoanId");

            migrationBuilder.CreateIndex(
                name: "IX_NhanCongs_ChucVuId",
                table: "NhanCongs",
                column: "ChucVuId");

            migrationBuilder.CreateIndex(
                name: "IX_NhanCongs_GiaiDoanId",
                table: "NhanCongs",
                column: "GiaiDoanId");

            migrationBuilder.CreateIndex(
                name: "IX_ResetMatKhaus_ThanhVienId",
                table: "ResetMatKhaus",
                column: "ThanhVienId");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhViens_ChucVuId",
                table: "ThanhViens",
                column: "ChucVuId");

            migrationBuilder.CreateIndex(
                name: "IX_VatLieus_GiaiDoanId",
                table: "VatLieus",
                column: "GiaiDoanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaoCaoTienDos");

            migrationBuilder.DropTable(
                name: "ChamCongs");

            migrationBuilder.DropTable(
                name: "DichVus");

            migrationBuilder.DropTable(
                name: "MayMocs");

            migrationBuilder.DropTable(
                name: "ResetMatKhaus");

            migrationBuilder.DropTable(
                name: "VatLieus");

            migrationBuilder.DropTable(
                name: "NhanCongs");

            migrationBuilder.DropTable(
                name: "ThanhViens");

            migrationBuilder.DropTable(
                name: "GiaiDoans");

            migrationBuilder.DropTable(
                name: "ChucVus");

            migrationBuilder.DropTable(
                name: "CongTrinhs");
        }
    }
}
