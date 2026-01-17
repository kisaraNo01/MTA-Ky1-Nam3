using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using QuanLyBenhXa.Models;

namespace QuanLyBenhXa.Controllers
{
    [Authorize]
    public class HoSoKhamBenhController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HoSoKhamBenhController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: HoSoKhamBenh/Index (Admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var hoSoKhamBenhs = await _context.HoSoKhamBenhs
                .Include(h => h.BenhNhan)
                .OrderByDescending(h => h.NgayKham)
                .ToListAsync();
            return View(hoSoKhamBenhs);
        }

        // GET: HoSoKhamBenh/Create?benhNhanId=5
        [Authorize(Roles = "Admin,NhanVien")]
        public IActionResult Create(int? benhNhanId)
        {
            if (benhNhanId == null)
            {
                return NotFound();
            }

            var benhNhan = _context.BenhNhans.Find(benhNhanId);
            if (benhNhan == null)
            {
                return NotFound();
            }

            ViewBag.BenhNhanId = benhNhanId;
            ViewBag.BenhNhanName = benhNhan.HoTen;
            ViewBag.BenhNhan = benhNhan; // Pass full object
            
            // Get list of Doctor names for dropdown
            var bacSis = _context.BacSis.Select(b => b.Ten).ToList();
            ViewBag.BacSiList = new SelectList(bacSis);

            var model = new HoSoKhamBenh
            {
                NgayKham = DateTime.Now
            };

            return View(model);
        }

        // POST: HoSoKhamBenh/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> Create([Bind("Id,BenhNhanId,NgayKham,BacSiPhuTrach,TrieuChung")] HoSoKhamBenh hoSoKhamBenh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoSoKhamBenh);
                await _context.SaveChangesAsync();
                // Redirect back to Patient Details
                return RedirectToAction("Details", "BenhNhan", new { id = hoSoKhamBenh.BenhNhanId });
            }

            // Reload data if validation fails
            var benhNhan = _context.BenhNhans.Find(hoSoKhamBenh.BenhNhanId);
            ViewBag.BenhNhanId = hoSoKhamBenh.BenhNhanId;
            ViewBag.BenhNhanName = benhNhan?.HoTen;
            ViewBag.BacSiList = new SelectList(_context.BacSis.Select(b => b.Ten).ToList(), hoSoKhamBenh.BacSiPhuTrach);

            return View(hoSoKhamBenh);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,BacSi")]
        public async Task<IActionResult> UpdateKetLuan(int id, string ketLuan)
        {
            var hoso = await _context.HoSoKhamBenhs.FindAsync(id);
            if (hoso == null)
            {
                return NotFound();
            }

            // Check if user is the responsible doctor (skip check for Admin)
            if (!User.IsInRole("Admin"))
            {
                var user = await _userManager.GetUserAsync(User);
                var bacSi = await _context.BacSis.FindAsync(user?.BacSiId);
                // Simple name check as per current model design
                if (bacSi == null || hoso.BacSiPhuTrach != bacSi.Ten)
                {
                    return Forbid();
                }
            }

            hoso.KetLuan = ketLuan ?? string.Empty;
            _context.Update(hoso);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "BenhNhan", new { id = hoso.BenhNhanId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,BacSi")]
        public async Task<IActionResult> UpdatePhongYeuCau(int id, string[] phongYeuCau)
        {
            var hoso = await _context.HoSoKhamBenhs.FindAsync(id);
            if (hoso == null)
            {
                return NotFound();
            }

            // Check if user is the responsible doctor (skip check for Admin)
            if (!User.IsInRole("Admin"))
            {
                var user = await _userManager.GetUserAsync(User);
                var bacSi = await _context.BacSis.FindAsync(user?.BacSiId);
                // Simple name check as per current model design
                if (bacSi == null || hoso.BacSiPhuTrach != bacSi.Ten)
                {
                    return Forbid();
                }
            }

            hoso.PhongYeuCau = phongYeuCau != null ? string.Join(", ", phongYeuCau) : string.Empty;
            _context.Update(hoso);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "BenhNhan", new { id = hoso.BenhNhanId });
        }
    }
}
