using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhXa.Models;

namespace QuanLyBenhXa.Controllers
{
    [Authorize(Roles = "Admin,NhanVien")]
    public class ThuocController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThuocController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Thuoc
        public async Task<IActionResult> Index()
        {
            return View(await _context.Thuocs.ToListAsync());
        }

        // GET: Thuoc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuocs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thuoc == null)
            {
                return NotFound();
            }

            return View(thuoc);
        }

        // POST: Thuoc/NhapThuoc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NhapThuoc(int id, int soLuongNhap)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null)
            {
                return NotFound();
            }

            if (soLuongNhap > 0)
            {
                thuoc.SoLuongTon += soLuongNhap;
                _context.Update(thuoc);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã nhập thêm {soLuongNhap} {thuoc.DonViTinh} {thuoc.TenThuoc}.";
            }
            else
            {
                TempData["Error"] = "Số lượng nhập phải lớn hơn 0!";
            }

            return RedirectToAction(nameof(Details), new { id = thuoc.Id });
        }


        // GET: Thuoc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Thuoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenThuoc,NhaSanXuat,DonViTinh,SoLuongTon,DonGia,HamLuong,CachDung")] Thuoc thuoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thuoc);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã thêm thuốc mới: " + thuoc.TenThuoc;
                return RedirectToAction(nameof(Index));
            }
            return View(thuoc);
        }

        // GET: Thuoc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null)
            {
                return NotFound();
            }
            return View(thuoc);
        }

        // POST: Thuoc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenThuoc,NhaSanXuat,DonViTinh,SoLuongTon,DonGia,HamLuong,CachDung")] Thuoc thuoc)
        {
            if (id != thuoc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thuoc);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật thành công thuốc: " + thuoc.TenThuoc;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThuocExists(thuoc.Id))
                    {
                        TempData["Error"] = "Không tìm thấy thuốc!";
                         return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(thuoc);
        }

        // GET: Thuoc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuocs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thuoc == null)
            {
                return NotFound();
            }

            return View(thuoc);
        }

        // POST: Thuoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc != null)
            {
                _context.Thuocs.Remove(thuoc);
                TempData["Success"] = "Đã xóa thuốc: " + thuoc.TenThuoc;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThuocExists(int id)
        {
            return _context.Thuocs.Any(e => e.Id == id);
        }
    }
}
