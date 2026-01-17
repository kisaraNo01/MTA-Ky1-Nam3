using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using QuanLyBenhXa.Models;

namespace QuanLyBenhXa.Controllers
{
    [Authorize(Roles = "Admin,BacSi")]
    public class DonThuocController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonThuocController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DonThuoc/Create?hosoId=5
        public async Task<IActionResult> Create(int? hosoId)
        {
            if (hosoId == null) return NotFound();

            var hoso = _context.HoSoKhamBenhs
                .Include(h => h.BenhNhan)
                .FirstOrDefault(h => h.Id == hosoId);

            if (hoso == null) return NotFound();

            // Check if user is the responsible doctor (skip check for Admin)
            if (!User.IsInRole("Admin"))
            {
                var user = await _userManager.GetUserAsync(User);
                var bacSi = await _context.BacSis.FindAsync(user?.BacSiId);
                // Simple name check as per current model design
                if (bacSi == null || hoso.BacSiPhuTrach != bacSi.Ten) return Forbid();
            }

            ViewBag.HoSoId = hosoId;
            ViewBag.BenhNhanName = hoso.BenhNhan?.HoTen;
            ViewBag.ThuocId = new SelectList(_context.Thuocs, "Id", "TenThuoc");
            
            return View();
        }

        // POST: DonThuoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoSoKhamBenhId,ThuocId,SoLuong,CachDung")] DonThuoc donThuoc)
        {
            if (ModelState.IsValid)
            {
                // Check Access
                if (!User.IsInRole("Admin"))
                {
                    var user = await _userManager.GetUserAsync(User);
                    var hosoCheck = await _context.HoSoKhamBenhs.AsNoTracking().FirstOrDefaultAsync(h => h.Id == donThuoc.HoSoKhamBenhId);
                    var bacSi = await _context.BacSis.FindAsync(user?.BacSiId);
                    if (bacSi == null || hosoCheck == null || hosoCheck.BacSiPhuTrach != bacSi.Ten) return Forbid();
                }

                _context.Add(donThuoc);
                await _context.SaveChangesAsync();
                
                 var hoso = await _context.HoSoKhamBenhs.FindAsync(donThuoc.HoSoKhamBenhId);
                 return RedirectToAction("Details", "BenhNhan", new { id = hoso?.BenhNhanId });
            }
            
            var existingHoso = await _context.HoSoKhamBenhs.Include(h => h.BenhNhan).FirstOrDefaultAsync(h => h.Id == donThuoc.HoSoKhamBenhId);
            ViewBag.HoSoId = donThuoc.HoSoKhamBenhId;
            ViewBag.BenhNhanName = existingHoso?.BenhNhan?.HoTen;
            ViewBag.ThuocId = new SelectList(_context.Thuocs, "Id", "TenThuoc", donThuoc.ThuocId);
            
            return View(donThuoc);
        }

        // POST: DonThuoc/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var donThuoc = await _context.DonThuocs.FindAsync(id);
            if (donThuoc != null)
            {
                // Check Access
                if (!User.IsInRole("Admin"))
                {
                    var user = await _userManager.GetUserAsync(User);
                    var hosoCheck = await _context.HoSoKhamBenhs.AsNoTracking().FirstOrDefaultAsync(h => h.Id == donThuoc.HoSoKhamBenhId);
                    var bacSi = await _context.BacSis.FindAsync(user?.BacSiId);
                    if (bacSi == null || hosoCheck == null || hosoCheck.BacSiPhuTrach != bacSi.Ten) return Forbid();
                }

                var hosoId = donThuoc.HoSoKhamBenhId;
                _context.DonThuocs.Remove(donThuoc);
                await _context.SaveChangesAsync();
                
                var hoso = await _context.HoSoKhamBenhs.FindAsync(hosoId);
                return RedirectToAction("Details", "BenhNhan", new { id = hoso?.BenhNhanId });
            }
            return NotFound();
        }
    }
}
