using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOESTKC.CSDL;
using SHOESTKC.Data;

namespace SHOESTKC.Controllers
{
    public class SizeController : Controller
    {
        private readonly CSDLShoesShopDbContext _context;

        public SizeController(CSDLShoesShopDbContext context)
        {
            _context = context;
        }

        // GET: Size
        public async Task<IActionResult> Index()
        {
            var sizes = await _context.Sizes
                .Include(s => s.BienThe) // Include để đếm số biến thể
                .OrderBy(s => s.ThuTu)
                .ToListAsync();
            return View(sizes);
        }

        // GET: Size/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes
                .Include(s => s.BienThe)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (size == null)
            {
                return NotFound();
            }

            // View Details not implemented/needed, reusing Edit
            return RedirectToAction(nameof(Edit), new { id = id });
        }

        // GET: Size/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Size/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenSize,ThuTu,TrangThai")] Size size)
        {
            if (ModelState.IsValid)
            {
                size.NgayTao = DateTime.Now;
                size.NgayCapNhat = DateTime.Now;
                size.TrangThai = size.TrangThai ?? true;

                _context.Add(size);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Đã thêm size mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(size);
        }

        // GET: Size/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes
                .Include(s => s.BienThe) // Include để hiển thị số biến thể
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }

        // POST: Size/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenSize,ThuTu,TrangThai,NgayTao")] Size size)
        {
            if (id != size.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    size.NgayCapNhat = DateTime.Now;
                    _context.Update(size);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Đã cập nhật size thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(size.Id))
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
            return View(size);
        }

        // GET: Size/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes
                .Include(s => s.BienThe)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        // POST: Size/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var size = await _context.Sizes
                .Include(s => s.BienThe)
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (size == null)
            {
                return NotFound();
            }

            // Kiểm tra xem size có đang được sử dụng không
            if (size.BienThe.Any())
            {
                TempData["Error"] = $"Không thể xóa size '{size.TenSize}' vì đang có {size.BienThe.Count} biến thể sử dụng!";
                return RedirectToAction(nameof(Index));
            }

            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Đã xóa size thành công!";
            return RedirectToAction(nameof(Index));
        }

        private bool SizeExists(int id)
        {
            return _context.Sizes.Any(e => e.Id == id);
        }
    }
}
