using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHOESTKC.Data;
using SHOESTKC.CSDL;

namespace SHOESTKC.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly CSDLShoesShopDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SanPhamController(CSDLShoesShopDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // ========================================
        // GET: Danh sách sản phẩm
        // ========================================
        public async Task<IActionResult> Index(int page = 1, string keyword = "", int? danhMucId = null,
            decimal? giaMin = null, decimal? giaMax = null)
        {
            int pageSize = 10;

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

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(sp =>
                    sp.TenSanPham.Contains(keyword) ||
                    (sp.Hang ?? "").Contains(keyword) ||
                    (sp.MoTa ?? "").Contains(keyword));
            }

            if (danhMucId.HasValue && danhMucId.Value > 0)
                query = query.Where(sp => sp.DanhMucId == danhMucId.Value);

            if (giaMin.HasValue)
                query = query.Where(sp => sp.GiaGoc >= giaMin.Value);

            if (giaMax.HasValue)
                query = query.Where(sp => sp.GiaGoc <= giaMax.Value);

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
            ViewBag.DanhMucId = danhMucId;
            ViewBag.GiaMin = giaMin;
            ViewBag.GiaMax = giaMax;

            // Dropdown danh mục
            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMuc.ToListAsync(),
                "Id",
                "TenDanhMuc",
                danhMucId
            );

            return View("~/Views/Admin/SanPham.cshtml", sanPham);
        }

        // ========================================
        // GET: Chi tiết sản phẩm
        // ========================================
        public async Task<IActionResult> Details(int id)
        {
            var sanPham = await _context.SanPham
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.BienThe)
                .Include(sp => sp.DanhGia)
                    .ThenInclude(dg => dg.NguoiDung)
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (sanPham == null)
                return NotFound();

            return View("~/Views/Admin/ChiTietSanPham.cshtml", sanPham);
        }

        // ========================================
        // GET: Thêm sản phẩm
        // ========================================
        public async Task<IActionResult> Create()
        {
            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMuc.ToListAsync(),
                "Id",
                "TenDanhMuc"
            );

            return View("~/Views/Admin/ThemSanPham.cshtml", new SanPham());
        }

        // POST: Thêm sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPham sanPham, IFormFile? anhChinh)
        {
            sanPham.TrangThai = Request.Form["TrangThai"].ToString().Contains("true");
            ModelState.Remove("TrangThai");

            // Validation định dạng ảnh
            if (anhChinh != null && anhChinh.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(anhChinh.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    TempData["Error"] = "Định dạng file không hợp lệ! Vui lòng chọn file ảnh (jpg, jpeg, png, gif, webp).";
                    ViewBag.DanhMucs = new SelectList(
                        await _context.DanhMuc.ToListAsync(),
                        "Id",
                        "TenDanhMuc",
                        sanPham.DanhMucId
                    );
                    return View("~/Views/Admin/ThemSanPham.cshtml", sanPham);
                }

                // Kiểm tra kích thước file (tối đa 5MB)
                if (anhChinh.Length > 5 * 1024 * 1024)
                {
                    TempData["Error"] = "Kích thước file quá lớn! Vui lòng chọn file nhỏ hơn 5MB.";
                    ViewBag.DanhMucs = new SelectList(
                        await _context.DanhMuc.ToListAsync(),
                        "Id",
                        "TenDanhMuc",
                        sanPham.DanhMucId
                    );
                    return View("~/Views/Admin/ThemSanPham.cshtml", sanPham);
                }
            }

            // Validation giá sản phẩm
            if (sanPham.GiaGoc < 1000)
            {
                TempData["Error"] = "Giá sản phẩm phải từ 1,000đ trở lên!";
                ViewBag.DanhMucs = new SelectList(
                    await _context.DanhMuc.ToListAsync(),
                    "Id",
                    "TenDanhMuc",
                    sanPham.DanhMucId
                );
                return View("~/Views/Admin/ThemSanPham.cshtml", sanPham);
            }

            if (sanPham.GiaGoc > 999999999)
            {
                TempData["Error"] = "Giá sản phẩm không được vượt quá 999,999,999đ!";
                ViewBag.DanhMucs = new SelectList(
                    await _context.DanhMuc.ToListAsync(),
                    "Id",
                    "TenDanhMuc",
                    sanPham.DanhMucId
                );
                return View("~/Views/Admin/ThemSanPham.cshtml", sanPham);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (anhChinh != null && anhChinh.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        Directory.CreateDirectory(uploadsFolder);

                        string fileName = Guid.NewGuid() + "_" + anhChinh.FileName;
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using var stream = new FileStream(filePath, FileMode.Create);
                        await anhChinh.CopyToAsync(stream);

                        sanPham.AnhChinh = "/images/" + fileName;
                    }
                    else
                    {
                        sanPham.AnhChinh = "/images/default-product.jpg";
                    }

                    sanPham.NgayTao = DateTime.Now;
                    sanPham.NgayCapNhat = DateTime.Now;

                    _context.Add(sanPham);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi khi thêm sản phẩm: " + ex.Message;
                }
            }
            else
            {
                // Hiển thị lỗi validation
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Vui lòng kiểm tra lại thông tin: " + string.Join(", ", errors);
            }

            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMuc.ToListAsync(),
                "Id",
                "TenDanhMuc",
                sanPham.DanhMucId
            );

            return View("~/Views/Admin/ThemSanPham.cshtml", sanPham);
        }

        // ========================================
        // POST: Xóa sản phẩm
        // ========================================
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var sanPham = await _context.SanPham
                    .Include(sp => sp.BienThe)
                    .FirstOrDefaultAsync(sp => sp.Id == id);

                if (sanPham == null)
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });

                var coTrongDonHang = await _context.ChiTietDonHang
                    .AnyAsync(ct => sanPham.BienThe.Select(bt => bt.Id).Contains(ct.BienTheId));

                if (coTrongDonHang)
                    return Json(new { success = false, message = "Không thể xóa sản phẩm đã có đơn hàng" });

                _context.BienThe.RemoveRange(sanPham.BienThe);
                _context.SanPham.Remove(sanPham);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Xóa sản phẩm thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        private async Task<bool> SanPhamExists(int id)
        {
            return await _context.SanPham.AnyAsync(e => e.Id == id);
        }
    }
}
