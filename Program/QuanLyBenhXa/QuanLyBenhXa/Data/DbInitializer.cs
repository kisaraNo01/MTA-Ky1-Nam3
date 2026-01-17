using Microsoft.AspNetCore.Identity;
using QuanLyBenhXa.Models;

namespace QuanLyBenhXa.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "NhanVien", "BacSi" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create Admin User
            var adminUser = await userManager.FindByEmailAsync("admin@quanlybenhxa.com");
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "admin@quanlybenhxa.com",
                    Email = "admin@quanlybenhxa.com",
                    FullName = "Administrator",
                    EmailConfirmed = true
                };
                var createPowerUser = await userManager.CreateAsync(newAdmin, "Admin@123");
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
            
            // Create Default NhanVien User
            var nvUser = await userManager.FindByEmailAsync("nhanvien@quanlybenhxa.com");
             if (nvUser == null)
            {
                var newNv = new ApplicationUser
                {
                    UserName = "nhanvien@quanlybenhxa.com",
                    Email = "nhanvien@quanlybenhxa.com",
                    FullName = "Nhân Viên Tiếp Nhận",
                    EmailConfirmed = true
                };
                var createNv = await userManager.CreateAsync(newNv, "Nhanvien@123");
                if (createNv.Succeeded)
                {
                    await userManager.AddToRoleAsync(newNv, "NhanVien");
                }
            }

            // Medicine Seeding
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            if (!context.Thuocs.Any())
            {
                var thuocs = new List<Thuoc>
                {
                    new Thuoc { TenThuoc = "Paracetamol 500mg", DonViTinh = "Viên", DonGia = 1000, SoLuongTon = 1000, HamLuong = "500mg", CachDung = "Uống sau ăn" },
                    new Thuoc { TenThuoc = "Amoxicillin 500mg", DonViTinh = "Viên", DonGia = 2500, SoLuongTon = 500, HamLuong = "500mg", CachDung = "Uống sau ăn" },
                    new Thuoc { TenThuoc = "Ibuprofen 400mg", DonViTinh = "Viên", DonGia = 1500, SoLuongTon = 600, HamLuong = "400mg", CachDung = "Uống sau ăn, khi đau" },
                    new Thuoc { TenThuoc = "Vitamin C 500mg", DonViTinh = "Viên", DonGia = 1200, SoLuongTon = 800, HamLuong = "500mg", CachDung = "Uống buổi sáng" },
                    new Thuoc { TenThuoc = "Berberin 100mg", DonViTinh = "Viên", DonGia = 500, SoLuongTon = 1000, HamLuong = "100mg", CachDung = "Uống khi đau bụng" },
                    new Thuoc { TenThuoc = "Omeprazol 20mg", DonViTinh = "Viên", DonGia = 2000, SoLuongTon = 400, HamLuong = "20mg", CachDung = "Uống trước ăn 30p" },
                    new Thuoc { TenThuoc = "Loratadin 10mg", DonViTinh = "Viên", DonGia = 1800, SoLuongTon = 500, HamLuong = "10mg", CachDung = "Uống khi dị ứng" },
                    new Thuoc { TenThuoc = "Panadol Extra", DonViTinh = "Viên", DonGia = 1500, SoLuongTon = 1000, HamLuong = "500mg + 65mg", CachDung = "Uống sau ăn" },
                    new Thuoc { TenThuoc = "Amlodipin 5mg", DonViTinh = "Viên", DonGia = 3000, SoLuongTon = 300, HamLuong = "5mg", CachDung = "Uống buổi sáng" },
                    new Thuoc { TenThuoc = "Metformin 500mg", DonViTinh = "Viên", DonGia = 2200, SoLuongTon = 400, HamLuong = "500mg", CachDung = "Uống trong hoặc sau ăn" }
                };
                context.Thuocs.AddRange(thuocs);
                await context.SaveChangesAsync();
            }

            // Create BacSi Entity and User (General)
            if (!context.BacSis.Any(b => b.Ten == "Bác sĩ Đa Khoa"))
            {
                var bacSiDk = new BacSi { Ten = "Bác sĩ Đa Khoa", CapBac = "Đại úy", ChucVu = "Bác sĩ", PhongKham = "Đa khoa", NgaySinh = DateTime.Now.AddYears(-30), GioiTinh = "Nam" };
                context.BacSis.Add(bacSiDk);
                await context.SaveChangesAsync();

                var dkUser = new ApplicationUser
                {
                    UserName = "dakhoa@quanlybenhxa.com",
                    Email = "dakhoa@quanlybenhxa.com",
                    FullName = "BS Đa Khoa",
                    EmailConfirmed = true,
                    BacSiId = bacSiDk.Id
                };
                var createDk = await userManager.CreateAsync(dkUser, "Bacsi@123");
                if (createDk.Succeeded) await userManager.AddToRoleAsync(dkUser, "BacSi");
            }

             // Create BacSi Entity and User (Specialist - Tim Mach)
            if (!context.BacSis.Any(b => b.Ten == "Bác sĩ Tim Mạch"))
            {
                var bacSiTm = new BacSi { Ten = "Bác sĩ Tim Mạch", CapBac = "Thiếu tá", ChucVu = "Bác sĩ", PhongKham = "Tim mạch", NgaySinh = DateTime.Now.AddYears(-35), GioiTinh = "Nữ" };
                context.BacSis.Add(bacSiTm);
                await context.SaveChangesAsync();

                var tmUser = new ApplicationUser
                {
                    UserName = "timmach@quanlybenhxa.com",
                    Email = "timmach@quanlybenhxa.com",
                    FullName = "BS Tim Mạch",
                    EmailConfirmed = true,
                    BacSiId = bacSiTm.Id
                };
                var createTm = await userManager.CreateAsync(tmUser, "Bacsi@123");
                if (createTm.Succeeded) await userManager.AddToRoleAsync(tmUser, "BacSi");
            }

            // Create BacSi Entity and User (Specialist - Da Lieu)
            if (!context.BacSis.Any(b => b.Ten == "Bác sĩ Da Liễu"))
            {
                var bacSiDl = new BacSi { Ten = "Bác sĩ Da Liễu", CapBac = "Đại úy", ChucVu = "Bác sĩ", PhongKham = "Da liễu", NgaySinh = DateTime.Now.AddYears(-32), GioiTinh = "Nữ" };
                context.BacSis.Add(bacSiDl);
                await context.SaveChangesAsync();

                var dlUser = new ApplicationUser
                {
                    UserName = "dalieu@quanlybenhxa.com",
                    Email = "dalieu@quanlybenhxa.com",
                    FullName = "BS Da Liễu",
                    EmailConfirmed = true,
                    BacSiId = bacSiDl.Id
                };
                var createDl = await userManager.CreateAsync(dlUser, "Bacsi@123");
                if (createDl.Succeeded) await userManager.AddToRoleAsync(dlUser, "BacSi");
            }

            // Create BacSi Entity and User (Specialist - Xet Nghiem)
            if (!context.BacSis.Any(b => b.Ten == "Bác sĩ Xét Nghiệm"))
            {
                var bacSiXn = new BacSi { Ten = "Bác sĩ Xét Nghiệm", CapBac = "Trung tá", ChucVu = "Bác sĩ", PhongKham = "Xét nghiệm", NgaySinh = DateTime.Now.AddYears(-40), GioiTinh = "Nam" };
                context.BacSis.Add(bacSiXn);
                await context.SaveChangesAsync();

                var xnUser = new ApplicationUser
                {
                    UserName = "xetnghiem@quanlybenhxa.com",
                    Email = "xetnghiem@quanlybenhxa.com",
                    FullName = "BS Xét Nghiệm",
                    EmailConfirmed = true,
                    BacSiId = bacSiXn.Id
                };
                var createXn = await userManager.CreateAsync(xnUser, "Bacsi@123");
                if (createXn.Succeeded) await userManager.AddToRoleAsync(xnUser, "BacSi");
            }

            // Create BacSi Entity and User (Specialist - Sieu Am)
            if (!context.BacSis.Any(b => b.Ten == "Bác sĩ Siêu Âm"))
            {
                var bacSiSa = new BacSi { Ten = "Bác sĩ Siêu Âm", CapBac = "Thiếu tá", ChucVu = "Bác sĩ", PhongKham = "Siêu âm", NgaySinh = DateTime.Now.AddYears(-38), GioiTinh = "Nữ" };
                context.BacSis.Add(bacSiSa);
                await context.SaveChangesAsync();

                var saUser = new ApplicationUser
                {
                    UserName = "sieuam@quanlybenhxa.com",
                    Email = "sieuam@quanlybenhxa.com",
                    FullName = "BS Siêu Âm",
                    EmailConfirmed = true,
                    BacSiId = bacSiSa.Id
                };
                var createSa = await userManager.CreateAsync(saUser, "Bacsi@123");
                if (createSa.Succeeded) await userManager.AddToRoleAsync(saUser, "BacSi");
            }

            // --- Additional Doctors (Batch 2) ---

            // General Doctor 2
            if (!context.BacSis.Any(b => b.Ten == "Bác sĩ Đa Khoa 2"))
            {
                var bacSiDk2 = new BacSi { Ten = "Bác sĩ Đa Khoa 2", CapBac = "Trung úy", ChucVu = "Bác sĩ", PhongKham = "Đa khoa", NgaySinh = DateTime.Now.AddYears(-28), GioiTinh = "Nữ" };
                context.BacSis.Add(bacSiDk2);
                await context.SaveChangesAsync();

                var dkUser2 = new ApplicationUser
                {
                    UserName = "dakhoa2@quanlybenhxa.com",
                    Email = "dakhoa2@quanlybenhxa.com",
                    FullName = "BS Đa Khoa 2",
                    EmailConfirmed = true,
                    BacSiId = bacSiDk2.Id
                };
                var createDk2 = await userManager.CreateAsync(dkUser2, "Bacsi@123");
                if (createDk2.Succeeded) await userManager.AddToRoleAsync(dkUser2, "BacSi");
            }

            // Cardiologist 2
            if (!context.BacSis.Any(b => b.Ten == "Bác sĩ Tim Mạch 2"))
            {
                var bacSiTm2 = new BacSi { Ten = "Bác sĩ Tim Mạch 2", CapBac = "Đại úy", ChucVu = "Bác sĩ", PhongKham = "Tim mạch", NgaySinh = DateTime.Now.AddYears(-33), GioiTinh = "Nam" };
                context.BacSis.Add(bacSiTm2);
                await context.SaveChangesAsync();

                var tmUser2 = new ApplicationUser
                {
                    UserName = "timmach2@quanlybenhxa.com",
                    Email = "timmach2@quanlybenhxa.com",
                    FullName = "BS Tim Mạch 2",
                    EmailConfirmed = true,
                    BacSiId = bacSiTm2.Id
                };
                var createTm2 = await userManager.CreateAsync(tmUser2, "Bacsi@123");
                if (createTm2.Succeeded) await userManager.AddToRoleAsync(tmUser2, "BacSi");
            }

            // Dermatologist 2
            if (!context.BacSis.Any(b => b.Ten == "Bác sĩ Da Liễu 2"))
            {
                var bacSiDl2 = new BacSi { Ten = "Bác sĩ Da Liễu 2", CapBac = "Thiếu tá", ChucVu = "Bác sĩ", PhongKham = "Da liễu", NgaySinh = DateTime.Now.AddYears(-45), GioiTinh = "Nam" };
                context.BacSis.Add(bacSiDl2);
                await context.SaveChangesAsync();

                var dlUser2 = new ApplicationUser
                {
                    UserName = "dalieu2@quanlybenhxa.com",
                    Email = "dalieu2@quanlybenhxa.com",
                    FullName = "BS Da Liễu 2",
                    EmailConfirmed = true,
                    BacSiId = bacSiDl2.Id
                };
                var createDl2 = await userManager.CreateAsync(dlUser2, "Bacsi@123");
                if (createDl2.Succeeded) await userManager.AddToRoleAsync(dlUser2, "BacSi");
            }

            // Lab Specialist 2
            if (!context.BacSis.Any(b => b.Ten == "Bác sĩ Xét Nghiệm 2"))
            {
                var bacSiXn2 = new BacSi { Ten = "Bác sĩ Xét Nghiệm 2", CapBac = "Thượng úy", ChucVu = "Bác sĩ", PhongKham = "Xét nghiệm", NgaySinh = DateTime.Now.AddYears(-29), GioiTinh = "Nữ" };
                context.BacSis.Add(bacSiXn2);
                await context.SaveChangesAsync();

                var xnUser2 = new ApplicationUser
                {
                    UserName = "xetnghiem2@quanlybenhxa.com",
                    Email = "xetnghiem2@quanlybenhxa.com",
                    FullName = "BS Xét Nghiệm 2",
                    EmailConfirmed = true,
                    BacSiId = bacSiXn2.Id
                };
                var createXn2 = await userManager.CreateAsync(xnUser2, "Bacsi@123");
                if (createXn2.Succeeded) await userManager.AddToRoleAsync(xnUser2, "BacSi");
            }

            // Ultrasound Specialist 2
            if (!context.BacSis.Any(b => b.Ten == "Bác sĩ Siêu Âm 2"))
            {
                var bacSiSa2 = new BacSi { Ten = "Bác sĩ Siêu Âm 2", CapBac = "Trung tá", ChucVu = "Bác sĩ", PhongKham = "Siêu âm", NgaySinh = DateTime.Now.AddYears(-42), GioiTinh = "Nam" };
                context.BacSis.Add(bacSiSa2);
                await context.SaveChangesAsync();

                var saUser2 = new ApplicationUser
                {
                    UserName = "sieuam2@quanlybenhxa.com",
                    Email = "sieuam2@quanlybenhxa.com",
                    FullName = "BS Siêu Âm 2",
                    EmailConfirmed = true,
                    BacSiId = bacSiSa2.Id
                };
                var createSa2 = await userManager.CreateAsync(saUser2, "Bacsi@123");
                if (createSa2.Succeeded) await userManager.AddToRoleAsync(saUser2, "BacSi");
            }
        }
    }
}
