using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOESTKC.Data;

namespace SHOESTKC.Controllers
{
    public class HomeController : Controller
    {
        private readonly CSDLShoesShopDbContext _context;

        public HomeController(CSDLShoesShopDbContext context)
        {
            _context = context;
        }

        // Trang chủ
        public async Task<IActionResult> Index()
        {
            // Lấy danh mục
            var danhMucs = await _context.DanhMuc.ToListAsync();
            ViewBag.DanhMucs = danhMucs;

            // Lấy sản phẩm mới nhất (12 sản phẩm)
            var sanPhamMoi = await _context.SanPham
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.BienThe)
                .Where(sp => sp.TrangThai == true)
                .OrderByDescending(sp => sp.NgayTao)
                .Take(12)
                .ToListAsync();

            return View(sanPhamMoi);
        }

        // Xem sản phẩm theo danh mục
        public async Task<IActionResult> Category(
            int id,
            int page = 1,
            string keyword = "",
            decimal? giaMin = null,
            decimal? giaMax = null,
            string sortBy = "")
        {
            int pageSize = 12;

            // Validate và giới hạn giá trị giá
            const decimal MAX_PRICE = 999999999m; // 999 triệu
            const decimal MIN_PRICE = 0m;

            if (giaMin.HasValue)
            {
                if (giaMin.Value < MIN_PRICE || giaMin.Value > MAX_PRICE)
                {
                    TempData["Warning"] = "Giá tối thiểu không hợp lệ. Vui lòng nhập từ 0đ đến 999,999,999đ";
                    giaMin = null;
                }
            }

            if (giaMax.HasValue)
            {
                if (giaMax.Value < MIN_PRICE || giaMax.Value > MAX_PRICE)
                {
                    TempData["Warning"] = "Giá tối đa không hợp lệ. Vui lòng nhập từ 0đ đến 999,999,999đ";
                    giaMax = null;
                }
            }

            // Lấy thông tin danh mục
            var danhMuc = await _context.DanhMuc.FindAsync(id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            ViewBag.DanhMuc = danhMuc;
            ViewBag.DanhMucs = await _context.DanhMuc.ToListAsync();

            // Query sản phẩm theo danh mục
            var query = _context.SanPham
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.BienThe)
                .Where(sp => sp.DanhMucId == id && sp.TrangThai == true)
                .AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(sp =>
                    sp.TenSanPham.Contains(keyword) ||
                    sp.Hang.Contains(keyword) ||
                    sp.MoTa.Contains(keyword));
            }

            // Lọc theo giá (chỉ khi giá trị hợp lệ)
            if (giaMin.HasValue)
            {
                query = query.Where(sp => sp.GiaGoc >= giaMin.Value);
            }
            if (giaMax.HasValue)
            {
                query = query.Where(sp => sp.GiaGoc <= giaMax.Value);
            }

            // Sắp xếp
            query = sortBy switch
            {
                "price_asc" => query.OrderBy(sp => sp.GiaGoc),
                "price_desc" => query.OrderByDescending(sp => sp.GiaGoc),
                "name_asc" => query.OrderBy(sp => sp.TenSanPham),
                "name_desc" => query.OrderByDescending(sp => sp.TenSanPham),
                _ => query.OrderByDescending(sp => sp.NgayTao)
            };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var sanPham = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = pageSize;
            ViewBag.Keyword = keyword;
            ViewBag.GiaMin = giaMin;
            ViewBag.GiaMax = giaMax;
            ViewBag.SortBy = sortBy;

            return View(sanPham);
        }

        // Chi tiết sản phẩm
        public async Task<IActionResult> ChiTiet(int id)
        {
            var sanPham = await _context.SanPham
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.BienThe)
                .Include(sp => sp.DanhGia)
                    .ThenInclude(dg => dg.NguoiDung)
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            // Sản phẩm liên quan (cùng danh mục)
            var sanPhamLienQuan = await _context.SanPham
                .Include(sp => sp.BienThe)
                .Where(sp =>
                    sp.DanhMucId == sanPham.DanhMucId &&
                    sp.Id != id &&
                    sp.TrangThai == true)
                .Take(4)
                .ToListAsync();

            ViewBag.SanPhamLienQuan = sanPhamLienQuan;

            return View(sanPham);
        }

        // Tìm kiếm tổng hợp
        public async Task<IActionResult> Search(
            string keyword = "",
            int page = 1,
            decimal? giaMin = null,
            decimal? giaMax = null,
            int? danhMucId = null)
        {
            int pageSize = 12;

            // Validate và giới hạn giá trị giá
            const decimal MAX_PRICE = 999999999m;
            const decimal MIN_PRICE = 0m;

            if (giaMin.HasValue)
            {
                if (giaMin.Value < MIN_PRICE || giaMin.Value > MAX_PRICE)
                {
                    TempData["Warning"] = "Giá tối thiểu không hợp lệ. Vui lòng nhập từ 0đ đến 999,999,999đ";
                    giaMin = null;
                }
            }

            if (giaMax.HasValue)
            {
                if (giaMax.Value < MIN_PRICE || giaMax.Value > MAX_PRICE)
                {
                    TempData["Warning"] = "Giá tối đa không hợp lệ. Vui lòng nhập từ 0đ đến 999,999,999đ";
                    giaMax = null;
                }
            }

            var query = _context.SanPham
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.BienThe)
                .Where(sp => sp.TrangThai == true)
                .AsQueryable();

            // Tìm theo từ khóa
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(sp =>
                    sp.TenSanPham.Contains(keyword) ||
                    sp.Hang.Contains(keyword) ||
                    sp.MoTa.Contains(keyword));
            }

            // Lọc theo danh mục
            if (danhMucId.HasValue && danhMucId.Value > 0)
            {
                query = query.Where(sp => sp.DanhMucId == danhMucId.Value);
            }

            // Lọc theo giá (chỉ khi giá trị hợp lệ)
            if (giaMin.HasValue)
            {
                query = query.Where(sp => sp.GiaGoc >= giaMin.Value);
            }
            if (giaMax.HasValue)
            {
                query = query.Where(sp => sp.GiaGoc <= giaMax.Value);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var sanPham = await query
                .OrderByDescending(sp => sp.NgayTao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = pageSize;
            ViewBag.Keyword = keyword;
            ViewBag.GiaMin = giaMin;
            ViewBag.GiaMax = giaMax;
            ViewBag.DanhMucId = danhMucId;
            ViewBag.DanhMucs = await _context.DanhMuc.ToListAsync();

            return View(sanPham);
        }
    }
}
