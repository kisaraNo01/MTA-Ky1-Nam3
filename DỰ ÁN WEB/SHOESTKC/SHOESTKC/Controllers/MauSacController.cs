using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOESTKC.CSDL;
using SHOESTKC.Data;

namespace SHOESTKC.Controllers
{
    public class MauSacController : Controller
    {
        private readonly CSDLShoesShopDbContext _context;

        public MauSacController(CSDLShoesShopDbContext context)
        {
            _context = context;
        }

        // GET: MauSac
        public async Task<IActionResult> Index()
        {
            var mauSacs = await _context.MauSacs
                .Include(m => m.BienThe) // Include để đếm số biến thể
                .OrderBy(m => m.TenMau)
                .ToListAsync();
            return View(mauSacs);
        }

        // GET: MauSac/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mauSac = await _context.MauSacs
                .Include(m => m.BienThe)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (mauSac == null)
            {
                return NotFound();
            }

            return View(mauSac);
        }

        // GET: MauSac/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MauSac/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenMau,MaHex,TrangThai")] MauSac mauSac)
        {
            if (ModelState.IsValid)
            {
                mauSac.NgayTao = DateTime.Now;
                mauSac.NgayCapNhat = DateTime.Now;
                mauSac.TrangThai = mauSac.TrangThai ?? true;

                _context.Add(mauSac);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Đã thêm màu sắc mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(mauSac);
        }

        // GET: MauSac/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mauSac = await _context.MauSacs
                .Include(m => m.BienThe) // Include để hiển thị số biến thể
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (mauSac == null)
            {
                return NotFound();
            }
            return View(mauSac);
        }

        // POST: MauSac/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenMau,MaHex,TrangThai,NgayTao")] MauSac mauSac)
        {
            if (id != mauSac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    mauSac.NgayCapNhat = DateTime.Now;
                    _context.Update(mauSac);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Đã cập nhật màu sắc thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MauSacExists(mauSac.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mauSac);
        }

        // GET: MauSac/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mauSac = await _context.MauSacs
                .Include(m => m.BienThe)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (mauSac == null)
            {
                return NotFound();
            }

            return View(mauSac);
        }

        // POST: MauSac/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mauSac = await _context.MauSacs
                .Include(m => m.BienThe)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (mauSac == null)
            {
                return NotFound();
            }

            // Kiểm tra xem màu sắc có đang được sử dụng không
            if (mauSac.BienThe.Any())
            {
                TempData["Error"] = $"Không thể xóa màu '{mauSac.TenMau}' vì đang có {mauSac.BienThe.Count} biến thể sử dụng!";
                return RedirectToAction(nameof(Index));
            }

            _context.MauSacs.Remove(mauSac);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Đã xóa màu sắc thành công!";
            return RedirectToAction(nameof(Index));
        }

        private bool MauSacExists(int id)
        {
            return _context.MauSacs.Any(e => e.Id == id);
        }
    }
}


