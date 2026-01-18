using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOESTKC.Data;
using SHOESTKC.CSDL;

namespace SHOESTKC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CSDLShoesShopDbContext _context;

        public CategoryController(CSDLShoesShopDbContext context)
        {
            _context = context;
        }

        // ===================== DANH SÁCH DANH MỤC =====================
        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            const int pageSize = 10;
            
            var query = _context.DanhMuc
                .Include(dm => dm.SanPham)
                .AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(dm => 
                    dm.TenDanhMuc.Contains(search) || 
                    dm.MoTa.Contains(search) ||
                    dm.Slug.Contains(search));
            }

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var danhMucs = await query
                .OrderByDescending(dm => dm.NgayTao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.Search = search;

            return View(danhMucs);
        }

        // ===================== CHI TIẾT DANH MỤC =====================
        public async Task<IActionResult> Details(int id)
        {
            var danhMuc = await _context.DanhMuc
                .Include(dm => dm.SanPham)
                    .ThenInclude(sp => sp.BienThe)
                .FirstOrDefaultAsync(dm => dm.Id == id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // ===================== THÊM DANH MỤC =====================
        [HttpGet]
        public IActionResult Create()
        {
            return View("ThemDanhMuc");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhMuc danhMuc)
        {
            if (!ModelState.IsValid)
            {
                return View("ThemDanhMuc", danhMuc);
            }

            // Tự động tạo slug từ tên danh mục
            if (string.IsNullOrEmpty(danhMuc.Slug))
            {
                danhMuc.Slug = GenerateSlug(danhMuc.TenDanhMuc);
            }

            // Kiểm tra slug đã tồn tại
            bool slugExists = await _context.DanhMuc
                .AnyAsync(dm => dm.Slug == danhMuc.Slug);

            if (slugExists)
            {
                ModelState.AddModelError("Slug", "Slug đã tồn tại, vui lòng chọn slug khác!");
                return View("ThemDanhMuc", danhMuc);
            }

            danhMuc.NgayTao = DateTime.Now;

            _context.DanhMuc.Add(danhMuc);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thêm danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }

        // ===================== SỬA DANH MỤC =====================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var danhMuc = await _context.DanhMuc.FindAsync(id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            return View("SuaDanhMuc", danhMuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DanhMuc danhMuc)
        {
            if (id != danhMuc.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View("SuaDanhMuc", danhMuc);
            }

            // Tạo slug nếu để trống
            if (string.IsNullOrEmpty(danhMuc.Slug))
            {
                danhMuc.Slug = GenerateSlug(danhMuc.TenDanhMuc);
            }

            // Kiểm tra slug trùng (trừ chính nó)
            bool slugExists = await _context.DanhMuc
                .AnyAsync(dm => dm.Slug == danhMuc.Slug && dm.Id != id);

            if (slugExists)
            {
                ModelState.AddModelError("Slug", "Slug đã tồn tại, vui lòng chọn slug khác!");
                return View("SuaDanhMuc", danhMuc);
            }

            try
            {
                _context.Update(danhMuc);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DanhMucExists(danhMuc.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        // ===================== XÓA DANH MỤC =====================
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var danhMuc = await _context.DanhMuc
                    .Include(dm => dm.SanPham)
                    .FirstOrDefaultAsync(dm => dm.Id == id);

                if (danhMuc == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy danh mục!" });
                }

                // Không cho xóa nếu đang có sản phẩm
                if (danhMuc.SanPham != null && danhMuc.SanPham.Any())
                {
                    return Json(new 
                    { 
                        success = false, 
                        message = "Không thể xóa danh mục đang có sản phẩm!" 
                    });
                }

                _context.DanhMuc.Remove(danhMuc);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Xóa danh mục thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // ===================== HELPER =====================
        private async Task<bool> DanhMucExists(int id)
        {
            return await _context.DanhMuc.AnyAsync(e => e.Id == id);
        }

        // Tạo slug từ tên danh mục
        private string GenerateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            text = text.ToLowerInvariant();

            string[] vietChars = 
            { 
                "áàạảãâấầậẩẫăắằặẳẵ", 
                "éèẹẻẽêếềệểễ", 
                "íìịỉĩ", 
                "óòọỏõôốồộổỗơớờợởỡ", 
                "úùụủũưứừựửữ", 
                "ýỳỵỷỹ", 
                "đ" 
            };

            string[] replaceChars = { "a", "e", "i", "o", "u", "y", "d" };

            for (int i = 0; i < vietChars.Length; i++)
            {
                foreach (char c in vietChars[i])
                {
                    text = text.Replace(c.ToString(), replaceChars[i]);
                }
            }

            text = System.Text.RegularExpressions.Regex.Replace(text, @"[^a-z0-9\s-]", "");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", "-");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"-+", "-");

            return text.Trim('-');
        }
    }
}
