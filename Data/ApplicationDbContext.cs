using Microsoft.EntityFrameworkCore;
using QuanLyCongTrinhXayDung.Models;

namespace QuanLyCongTrinhXayDung.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CongTrinh> CongTrinhs { get; set; } = null!;
        public DbSet<GiaiDoan> GiaiDoans { get; set; } = null!;
        public DbSet<ChucVu> ChucVus { get; set; } = null!;
        public DbSet<NhanCong> NhanCongs { get; set; } = null!;
        public DbSet<VatLieu> VatLieus { get; set; } = null!;
        public DbSet<DichVu> DichVus { get; set; } = null!;
        public DbSet<MayMoc> MayMocs { get; set; } = null!;
        public DbSet<ChamCong> ChamCongs { get; set; } = null!;
        public DbSet<BaoCaoTienDo> BaoCaoTienDos { get; set; }
        public DbSet<HinhAnhBaoCao> HinhAnhBaoCaos { get; set; } = null!;
        public DbSet<ThanhVien> ThanhViens { get; set; } = null!;
        public DbSet<ResetMatKhau> ResetMatKhaus { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Configure cascade deletes or fluent API
            modelBuilder.Entity<GiaiDoan>()
                .HasOne(g => g.CongTrinh)
                .WithMany(c => c.GiaiDoans)
                .HasForeignKey(g => g.CongTrinhId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<NhanCong>()
                .HasOne(n => n.GiaiDoan)
                .WithMany(g => g.NhanCongs)
                .HasForeignKey(n => n.GiaiDoanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VatLieu>()
                .HasOne(v => v.GiaiDoan)
                .WithMany(g => g.VatLieus)
                .HasForeignKey(v => v.GiaiDoanId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<DichVu>()
                .HasOne(d => d.GiaiDoan)
                .WithMany(g => g.DichVus)
                .HasForeignKey(d => d.GiaiDoanId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<MayMoc>()
                .HasOne(m => m.GiaiDoan)
                .WithMany(g => g.MayMocs)
                .HasForeignKey(m => m.GiaiDoanId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<ChamCong>()
                .HasOne(c => c.NhanCong)
                .WithMany(n => n.ChamCongs)
                .HasForeignKey(c => c.NhanCongId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

