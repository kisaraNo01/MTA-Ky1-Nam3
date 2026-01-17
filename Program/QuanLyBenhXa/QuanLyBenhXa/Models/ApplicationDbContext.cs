using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace QuanLyBenhXa.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BenhNhan> BenhNhans { get; set; }
        public DbSet<Thuoc> Thuocs { get; set; }
        public DbSet<BacSi> BacSis { get; set; }
        public DbSet<HoSoKhamBenh> HoSoKhamBenhs { get; set; }
        public DbSet<KetQuaKhamBenh> KetQuaKhamBenhs { get; set; }
        public DbSet<DonThuoc> DonThuocs { get; set; }
    }
}
