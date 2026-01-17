using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhXa.Models;

namespace QuanLyBenhXa.Controllers
{
    [Authorize(Roles = "BacSi")]
    public class DoctorDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorDashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.BacSiId == null)
            {
                return View("Error", new { message = "Tài khoản không liên kết với Bác sĩ." });
            }

            var bacSi = await _context.BacSis.FindAsync(user.BacSiId);
            if (bacSi == null) return NotFound();

            ViewBag.BacSiName = bacSi.Ten;
            ViewBag.PhongKham = bacSi.PhongKham;

            // 1. My Records (Patients assigned to this doctor as "Physician in Charge")
            var myRecords = await _context.HoSoKhamBenhs
                .Include(h => h.BenhNhan)
                .Include(h => h.KetQuaKhamBenhs)
                .Where(h => h.BacSiPhuTrach == bacSi.Ten) // Note: Using Name string match is fragile but consistent with current model. Id match would be better if HoSo bad BacSiId.
                .OrderByDescending(h => h.NgayKham)
                .ToListAsync();

            // 2. Room Requests (Patients referred to this doctor's clinic)
            // Logic update:
            // - If Doctor is "Đa khoa" (General): They should see ALL records that haven't had a General Exam yet (implicit request).
            // - Other Specialists: Only see records where their clinic is explicitly in PhongYeuCau.

            List<HoSoKhamBenh>? allActiveRecords;

            if (bacSi.PhongKham == "Đa khoa")
            {
                // Get all active records that DON'T have a "Đa khoa" result yet (and are NOT concluded)
                // Also exclude records where this doctor is ALREADY the BacSiPhuTrach (they are in MyRecords)
                allActiveRecords = await _context.HoSoKhamBenhs
                    .Include(h => h.BenhNhan)
                    .Include(h => h.KetQuaKhamBenhs)
                    .Where(h => string.IsNullOrEmpty(h.KetLuan)) // Only active
                    .OrderByDescending(h => h.NgayKham)
                    .ToListAsync();
                
                // Filter in memory for complex check
                allActiveRecords = allActiveRecords
                    .Where(h => !h.KetQuaKhamBenhs.Any(k => k.TenPhongKham == "Đa khoa"))
                    .Where(h => h.BacSiPhuTrach != bacSi.Ten) // Avoid duplication with MyRecords
                    .ToList();
            }
            else
            {
                // Specialist Logic (Existing)
                // Filter: PhongYeuCau contains doctor's PhongKham
                 allActiveRecords = await _context.HoSoKhamBenhs
                    .Include(h => h.BenhNhan)
                    .Include(h => h.KetQuaKhamBenhs)
                    // Handle null PhongYeuCau just in case
                    .Where(h => h.PhongYeuCau != null && h.PhongYeuCau.Contains(bacSi.PhongKham))
                    .Where(h => string.IsNullOrEmpty(h.KetLuan))
                    .OrderByDescending(h => h.NgayKham)
                    .ToListAsync();

                 // Further filter: Not yet done
                 allActiveRecords = allActiveRecords
                    .Where(h => !h.KetQuaKhamBenhs.Any(k => k.TenPhongKham == bacSi.PhongKham))
                    .ToList();
            }

            var roomRequests = allActiveRecords;

            var viewModel = new DoctorDashboardViewModel
            {
                MyRecords = myRecords,
                RoomRequests = roomRequests
            };

            return View(viewModel);
        }
    }

    public class DoctorDashboardViewModel
    {
        public List<HoSoKhamBenh> MyRecords { get; set; } = new List<HoSoKhamBenh>();
        public List<HoSoKhamBenh> RoomRequests { get; set; } = new List<HoSoKhamBenh>();
    }
}
