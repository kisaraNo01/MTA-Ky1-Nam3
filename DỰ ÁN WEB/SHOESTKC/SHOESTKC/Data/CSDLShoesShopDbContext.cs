using Microsoft.EntityFrameworkCore;
using SHOESTKC.CSDL;

namespace SHOESTKC.Data
{
    public partial class CSDLShoesShopDbContext : DbContext
    {
        public CSDLShoesShopDbContext() { }
        
        public CSDLShoesShopDbContext(DbContextOptions<CSDLShoesShopDbContext> options) : base(options) { }

        public virtual DbSet<BienThe> BienThe { get; set; }
        public virtual DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
        public virtual DbSet<DanhGia> DanhGia { get; set; }
        public virtual DbSet<DanhMuc> DanhMuc { get; set; }
        public virtual DbSet<DonHang> DonHang { get; set; }
        public virtual DbSet<GioHang> GioHang { get; set; }
        public virtual DbSet<MaKhuyenMai> MaKhuyenMai { get; set; }
        public virtual DbSet<MauSac> MauSacs { get; set; }
        public virtual DbSet<NguoiDung> NguoiDung { get; set; }
        public virtual DbSet<SanPham> SanPham { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connection string is configured in Program.cs via dependency injection
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình cho bảng DonHang có trigger
            // EF Core không thể dùng OUTPUT clause khi bảng có trigger
            modelBuilder.Entity<DonHang>()
                .ToTable(tb => tb.UseSqlOutputClause(false));

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
