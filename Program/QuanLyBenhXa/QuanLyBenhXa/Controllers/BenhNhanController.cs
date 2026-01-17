using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using QuanLyBenhXa.Models;

namespace QuanLyBenhXa.Controllers
{
    [Authorize(Roles = "Admin, NhanVien, BacSi")]
    public class BenhNhanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BenhNhanController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<string> GetCapBacList()
        {
            var ranks = new List<string>();
            
            // Military ranks (Binh nhì -> Đại tướng)
            var militaryRanks = new[] 
            { 
                "Binh nhì", "Binh nhất", "Hạ sĩ", "Trung sĩ", "Thượng sĩ",
                "Chuẩn úy", "Thiếu úy", "Trung úy", "Thượng úy", " Đại úy",
                "Thiếu tá", "Trung tá", "Thượng tá", "Đại tá",
                "Thiếu tướng", "Trung tướng", "Thượng tướng", "Đại tướng"
            };
            ranks.AddRange(militaryRanks);

            // Professional ranks (CN) (Thiếu úy CN -> Đại tá CN)
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

        // GET: BenhNhan
        public async Task<IActionResult> Index()
        {
            return View(await _context.BenhNhans.ToListAsync());
        }

        // GET: BenhNhan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var benhNhan = await _context.BenhNhans
                .Include(b => b.HoSoKhamBenhs)
                    .ThenInclude(h => h.KetQuaKhamBenhs)
                .Include(b => b.HoSoKhamBenhs)
                    .ThenInclude(h => h.DonThuocs)
                        .ThenInclude(d => d.Thuoc)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (benhNhan == null) return NotFound();

            return View(benhNhan);
        }

        // GET: BenhNhan/Create
        public IActionResult Create()
        {
            ViewBag.CapBacList = new SelectList(GetCapBacList());
            return View();
        }

        // POST: BenhNhan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HoTen,NgaySinh,GioiTinh,SoBHYT,DonVi,CapBac,ChucVu")] BenhNhan benhNhan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(benhNhan);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã thêm bệnh nhân: " + benhNhan.HoTen;
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CapBacList = new SelectList(GetCapBacList(), benhNhan.CapBac);
            return View(benhNhan);
        }

        // GET: BenhNhan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var benhNhan = await _context.BenhNhans.FindAsync(id);
            if (benhNhan == null) return NotFound();

            ViewBag.CapBacList = new SelectList(GetCapBacList(), benhNhan.CapBac);
            return View(benhNhan);
        }

        // POST: BenhNhan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HoTen,NgaySinh,GioiTinh,SoBHYT,DonVi,CapBac,ChucVu")] BenhNhan benhNhan)
        {
            if (id != benhNhan.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(benhNhan);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật thành công bệnh nhân: " + benhNhan.HoTen;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BenhNhanExists(benhNhan.Id)) 
                    {
                        TempData["Error"] = "Không tìm thấy bệnh nhân!";
                        return NotFound();
                    }
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CapBacList = new SelectList(GetCapBacList(), benhNhan.CapBac);
            return View(benhNhan);
        }

        // GET: BenhNhan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var benhNhan = await _context.BenhNhans.FirstOrDefaultAsync(m => m.Id == id);
            if (benhNhan == null) return NotFound();

            return View(benhNhan);
        }

        // POST: BenhNhan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var benhNhan = await _context.BenhNhans.FindAsync(id);
            if (benhNhan != null)
            {
                _context.BenhNhans.Remove(benhNhan);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa bệnh nhân: " + benhNhan.HoTen;
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BenhNhanExists(int id)
        {
            return _context.BenhNhans.Any(e => e.Id == id);
        }
    }
}
