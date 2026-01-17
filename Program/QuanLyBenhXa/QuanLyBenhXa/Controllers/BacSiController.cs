using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using QuanLyBenhXa.Models;

namespace QuanLyBenhXa.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BacSiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BacSiController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<string> GetCapBacList()
        {
            // Reusing the list from BenhNhan as requested + relevant additions if any.
            // Assuming same list logic for consistency.
             var ranks = new List<string>();
            
            // Military ranks
            var militaryRanks = new[] 
            { 
                "Binh nhì", "Binh nhất", "Hạ sĩ", "Trung sĩ", "Thượng sĩ",
                "Chuẩn úy", "Thiếu úy", "Trung úy", "Thượng úy", " Đại úy",
                "Thiếu tá", "Trung tá", "Thượng tá", "Đại tá",
                "Thiếu tướng", "Trung tướng", "Thượng tướng", "Đại tướng"
            };
            ranks.AddRange(militaryRanks);

            // Professional ranks
            var professionalRanks = new[]
            {
                "Thiếu úy CN", "Trung úy CN", "Thượng úy CN", "Đại úy CN",
                "Thiếu tá CN", "Trung tá CN", "Thượng tá CN", "Đại tá CN"
            };
            ranks.AddRange(professionalRanks);

             // Civilian
            ranks.Add("SV dân sự");

            return ranks;
        }

        private List<string> GetPhongKhamList()
        {
            return new List<string> { "Đa khoa", "Tim mạch", "Da liễu", "Xét nghiệm", "Siêu âm" };
        }

        // GET: BacSi
        public async Task<IActionResult> Index()
        {
            return View(await _context.BacSis.ToListAsync());
        }

        // GET: BacSi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var bacSi = await _context.BacSis.FirstOrDefaultAsync(m => m.Id == id);
            if (bacSi == null) return NotFound();

            return View(bacSi);
        }

        // GET: BacSi/Create
        public IActionResult Create()
        {
            ViewBag.CapBacList = new SelectList(GetCapBacList());
            ViewBag.PhongKhamList = new SelectList(GetPhongKhamList());
            return View();
        }

        // POST: BacSi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ten,CapBac,ChucVu,PhongKham,NgaySinh,GioiTinh")] BacSi bacSi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bacSi);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã thêm bác sĩ: " + bacSi.Ten;
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CapBacList = new SelectList(GetCapBacList(), bacSi.CapBac);
             ViewBag.PhongKhamList = new SelectList(GetPhongKhamList(), bacSi.PhongKham);
            return View(bacSi);
        }

        // GET: BacSi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var bacSi = await _context.BacSis.FindAsync(id);
            if (bacSi == null) return NotFound();

            ViewBag.CapBacList = new SelectList(GetCapBacList(), bacSi.CapBac);
            ViewBag.PhongKhamList = new SelectList(GetPhongKhamList(), bacSi.PhongKham);
            return View(bacSi);
        }

        // POST: BacSi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten,CapBac,ChucVu,PhongKham,NgaySinh,GioiTinh")] BacSi bacSi)
        {
            if (id != bacSi.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bacSi);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật thành công bác sĩ: " + bacSi.Ten;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BacSiExists(bacSi.Id))
                    {
                        TempData["Error"] = "Không tìm thấy bác sĩ!";
                        return NotFound();
                    } 
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CapBacList = new SelectList(GetCapBacList(), bacSi.CapBac);
            ViewBag.PhongKhamList = new SelectList(GetPhongKhamList(), bacSi.PhongKham);
            return View(bacSi);
        }

        // GET: BacSi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var bacSi = await _context.BacSis.FirstOrDefaultAsync(m => m.Id == id);
            if (bacSi == null) return NotFound();

            return View(bacSi);
        }

        // POST: BacSi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bacSi = await _context.BacSis.FindAsync(id);
            if (bacSi != null)
            {
                _context.BacSis.Remove(bacSi);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa bác sĩ: " + bacSi.Ten;
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BacSiExists(int id)
        {
            return _context.BacSis.Any(e => e.Id == id);
        }
    }
}
