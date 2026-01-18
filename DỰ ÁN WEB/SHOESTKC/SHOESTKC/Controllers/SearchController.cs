using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOESTKC.Data;

namespace SHOESTKC.Controllers
{
    public class SearchController : Controller
    {
        private readonly CSDLShoesShopDbContext _context;

        public SearchController(CSDLShoesShopDbContext context)
        {
            _context = context;
        }

        // Tìm kiếm sản phẩm - bao gồm cả thông tin ẩn
        public async Task<IActionResult> Index(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("Index", "Home");
            }

            // Tìm kiếm toàn diện:
            // - Tên sản phẩm (hiển thị)
            // - Hãng (hiển thị)
            // - Mô tả (ẩn - người dùng không nhìn thấy trên trang chủ)
            // - Tên danh mục (hiển thị nhưng có thể tìm theo từ khóa)
            // - Size, màu sắc từ biến thể (ẩn - chỉ hiện khi vào chi tiết)
            var SanPham = await _context.SanPham
                .Include(s => s.DanhMuc)
                .Include(s => s.BienThe)
                .Where(s => s.TrangThai == true &&
                    (s.TenSanPham.Contains(q) ||
                     (s.Hang ?? "").Contains(q) ||
                     (s.MoTa ?? "").Contains(q) ||  // Tìm theo mô tả (thông tin ẩn)
                     s.DanhMuc.TenDanhMuc.Contains(q) ||
                     s.BienThe.Any(bt => bt.Size.Contains(q)) ||  // Tìm theo size (thông tin ẩn)
                     s.BienThe.Any(bt => bt.MauSac.Contains(q)) ||  // Tìm theo màu sắc (thông tin ẩn)
                     s.BienThe.Any(bt => (bt.MaSku ?? "").Contains(q))))  // Tìm theo mã SKU (thông tin ẩn)
                .OrderByDescending(s => s.NgayTao)
                .ToListAsync();

            ViewBag.Keyword = q;
            ViewBag.ResultCount = SanPham.Count;

            return View(SanPham);
        }

        // API tìm kiếm gợi ý (autocomplete)
        [HttpGet]
        public async Task<IActionResult> Suggest(string term)
        {
            if (string.IsNullOrEmpty(term) || term.Length < 2)
            {
                return Json(new List<object>());
            }

            var suggestions = await _context.SanPham
                .Where(s => s.TrangThai == true && s.TenSanPham.Contains(term))
                .Take(5)
                .Select(s => new
                {
                    label = s.TenSanPham,
                    value = s.TenSanPham,
                    id = s.Id,
                    image = s.AnhChinh,
                    price = s.GiaGoc
                })
                .ToListAsync();

            return Json(suggestions);
        }
    }
}
