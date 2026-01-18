using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHOESTKC.Data;
using SHOESTKC.CSDL;

namespace SHOESTKC.Controllers
{
    public class AdminController : Controller
    {
        private readonly CSDLShoesShopDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(
            CSDLShoesShopDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // ===================== DASHBOARD =====================
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalSanPham = await _context.SanPham.CountAsync();
            ViewBag.TotalDatHang = await _context.DonHang.CountAsync();
            ViewBag.TotalKhachHang = await _context.NguoiDung
                .Where(u => u.VaiTro == "khach_hang")
                .CountAsync();

            ViewBag.TotalRevenue = await _context.DonHang
                .Where(d => d.TrangThaiThanhToan == "da_thanh_toan")
                .SumAsync(d => (decimal?)d.TongTien) ?? 0;

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            ViewBag.MonthlyRevenue = await _context.DonHang
                .Where(d =>
                    d.NgayTao.HasValue &&
                    d.NgayTao.Value.Month == currentMonth &&
                    d.NgayTao.Value.Year == currentYear &&
                    d.TrangThaiThanhToan == "da_thanh_toan")
                .SumAsync(d => (decimal?)d.TongTien) ?? 0;

            ViewBag.MonthlyOrders = await _context.DonHang
                .Where(d =>
                    d.NgayTao.HasValue &&
                    d.NgayTao.Value.Month == currentMonth &&
                    d.NgayTao.Value.Year == currentYear)
                .CountAsync();

            var recentDatHang = await _context.DonHang
                .Include(d => d.NguoiDung)
                .OrderByDescending(d => d.NgayTao)
                .Take(10)
                .ToListAsync();

            return View(recentDatHang);
        }

        // ===================== QUẢN LÝ SẢN PHẨM =====================
        public async Task<IActionResult> SanPham(
            string search,
            int? danhMucId,
            decimal? giaMin,
            decimal? giaMax,
            int page = 1)
        {
            const int pageSize = 10;

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
                .Include(s => s.DanhMuc)
                .Include(s => s.BienThe)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    s.TenSanPham.Contains(search) ||
                    s.Hang.Contains(search) ||
                    s.MoTa.Contains(search));
                ViewBag.Search = search;
            }

            if (danhMucId.HasValue && danhMucId > 0)
            {
                query = query.Where(s => s.DanhMucId == danhMucId);
                ViewBag.DanhMucId = danhMucId;
            }

            if (giaMin.HasValue)
            {
                query = query.Where(s => s.GiaGoc >= giaMin);
                ViewBag.GiaMin = giaMin;
            }

            if (giaMax.HasValue)
            {
                query = query.Where(s => s.GiaGoc <= giaMax);
                ViewBag.GiaMax = giaMax;
            }

            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMuc.ToListAsync(),
                "Id",
                "TenDanhMuc",
                danhMucId);

            int totalItems = await query.CountAsync();

            var sanPhamList = await query
                .OrderByDescending(s => s.NgayTao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotItems = totalItems;
            ViewBag.TotPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.PageStart = (page - 1) * pageSize;

            return View(sanPhamList);
        }

        // ===================== CHI TIẾT SẢN PHẨM =====================
        [HttpGet]
        public async Task<IActionResult> ChiTietSanPham(int id)
        {
            var sanPham = await _context.SanPham
                .Include(s => s.DanhMuc)
                .Include(s => s.BienThe)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sanPham == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm!";
                return RedirectToAction(nameof(SanPham));
            }

            return View(sanPham);
        }

        // ===================== SỬA SẢN PHẨM =====================
        [HttpGet]
        public async Task<IActionResult> SuaSanPham(int id)
        {
            var sanPham = await _context.SanPham
                .Include(s => s.BienThe)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sanPham == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm!";
                return RedirectToAction(nameof(SanPham));
            }

            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMuc.ToListAsync(),
                "Id",
                "TenDanhMuc",
                sanPham.DanhMucId);

            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuaSanPham(int id, SanPham model, IFormFile? AnhChinh)
        {
            if (id != model.Id)
            {
                TempData["Error"] = "ID sản phẩm không khớp!";
                return RedirectToAction(nameof(SanPham));
            }

            var existingSanPham = await _context.SanPham.FindAsync(id);
            if (existingSanPham == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm!";
                return RedirectToAction(nameof(SanPham));
            }

            // ===== VALIDATION =====
            
            // Validation giá sản phẩm
            if (model.GiaGoc < 1000)
            {
                TempData["Error"] = "Giá sản phẩm phải từ 1,000đ trở lên!";
                ViewBag.DanhMucs = new SelectList(
                    await _context.DanhMuc.ToListAsync(),
                    "Id",
                    "TenDanhMuc",
                    model.DanhMucId);
                return View(model);
            }

            if (model.GiaGoc > 999999999)
            {
                TempData["Error"] = "Giá sản phẩm không được vượt quá 999,999,999đ!";
                ViewBag.DanhMucs = new SelectList(
                    await _context.DanhMuc.ToListAsync(),
                    "Id",
                    "TenDanhMuc",
                    model.DanhMucId);
                return View(model);
            }

            // Validation định dạng ảnh
            if (AnhChinh != null && AnhChinh.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(AnhChinh.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    TempData["Error"] = "Định dạng file không hợp lệ! Vui lòng chọn file ảnh (jpg, jpeg, png, gif, webp).";
                    ViewBag.DanhMucs = new SelectList(
                        await _context.DanhMuc.ToListAsync(),
                        "Id",
                        "TenDanhMuc",
                        model.DanhMucId);
                    return View(model);
                }

                // Kiểm tra kích thước file (tối đa 5MB)
                if (AnhChinh.Length > 5 * 1024 * 1024)
                {
                    TempData["Error"] = "Kích thước file quá lớn! Vui lòng chọn file nhỏ hơn 5MB.";
                    ViewBag.DanhMucs = new SelectList(
                        await _context.DanhMuc.ToListAsync(),
                        "Id",
                        "TenDanhMuc",
                        model.DanhMucId);
                    return View(model);
                }
            }

            try
            {
                if (AnhChinh != null && AnhChinh.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingSanPham.AnhChinh))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingSanPham.AnhChinh.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var fileName = Path.GetFileNameWithoutExtension(AnhChinh.FileName);
                    var extension = Path.GetExtension(AnhChinh.FileName);
                    var newFileName = $"{fileName}_{DateTime.Now.Ticks}{extension}";
                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    var filePath = Path.Combine(uploadPath, newFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await AnhChinh.CopyToAsync(fileStream);
                    }

                    existingSanPham.AnhChinh = $"/images/{newFileName}";
                }

                existingSanPham.TenSanPham = model.TenSanPham;
                existingSanPham.DanhMucId = model.DanhMucId;
                existingSanPham.Hang = model.Hang;
                existingSanPham.GiaGoc = model.GiaGoc;
                existingSanPham.MoTa = model.MoTa;
                existingSanPham.TrangThai = model.TrangThai;
                existingSanPham.NgayCapNhat = DateTime.Now;

                _context.SanPham.Update(existingSanPham);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction(nameof(SanPham));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
                ViewBag.DanhMucs = new SelectList(
                    await _context.DanhMuc.ToListAsync(),
                    "Id",
                    "TenDanhMuc",
                    model.DanhMucId);
                return View(model);
            }
        }

        // ===================== THÊM SẢN PHẨM =====================
        [HttpGet]
        public async Task<IActionResult> ThemSanPham()
        {
            ViewBag.DanhMucs = new SelectList(
                await _context.DanhMuc.ToListAsync(),
                "Id",
                "TenDanhMuc"
            );

            return View(new SanPham());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemSanPham(SanPham sanPham, IFormFile? anhChinh)
        {
            // Xử lý checkbox TrangThai - checkbox được check sẽ gửi "true", không check chỉ gửi "false" từ hidden input
            var trangThaiValues = Request.Form["TrangThai"].ToString();
            sanPham.TrangThai = trangThaiValues.Split(',').Contains("true");

            // Loại bỏ validation cho các navigation properties
            ModelState.Remove("TrangThai");
            ModelState.Remove("DanhMuc");
            ModelState.Remove("BienThe");
            ModelState.Remove("AnhChinh");

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
                    return View(sanPham);
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
                    return View(sanPham);
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
                return View(sanPham);
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
                return View(sanPham);
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
                    else if (string.IsNullOrEmpty(sanPham.AnhChinh))
                    {
                        sanPham.AnhChinh = "/images/default-product.jpg";
                    }

                    sanPham.NgayTao = DateTime.Now;
                    sanPham.NgayCapNhat = DateTime.Now;

                    _context.Add(sanPham);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction(nameof(SanPham));
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

            return View(sanPham);
        }

        // ===================== XÓA SẢN PHẨM =====================
        [HttpPost]
        public async Task<IActionResult> XoaSanPham(int id)
        {
            try
            {
                var sanPham = await _context.SanPham
                    .Include(sp => sp.BienThe)
                    .FirstOrDefaultAsync(sp => sp.Id == id);

                if (sanPham == null)
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });

                // Kiểm tra sản phẩm có trong đơn hàng không
                var bienTheIds = sanPham.BienThe.Select(bt => bt.Id).ToList();
                var coTrongDonHang = await _context.ChiTietDonHang
                    .AnyAsync(ct => bienTheIds.Contains(ct.BienTheId));

                if (coTrongDonHang)
                {
                    // Nếu đang hiển thị, chuyển sang ẩn (soft delete)
                    if (sanPham.TrangThai == true)
                    {
                        sanPham.TrangThai = false;
                        _context.Update(sanPham);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true, message = "Đã ẩn sản phẩm (sản phẩm có trong đơn hàng)" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không thể xóa sản phẩm đã có trong đơn hàng. Sản phẩm đã được ẩn." });
                    }
                }

                // Nếu đang hiển thị và chưa có trong đơn hàng -> Ẩn
                if (sanPham.TrangThai == true)
                {
                    sanPham.TrangThai = false;
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Đã ẩn sản phẩm thành công" });
                }
                else
                {
                    // Đã ẩn và không có trong đơn hàng -> Xóa vĩnh viễn
                    _context.BienThe.RemoveRange(sanPham.BienThe);
                    _context.SanPham.Remove(sanPham);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Đã xóa sản phẩm vĩnh viễn" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // ===================== CHI TIẾT HÓA ĐƠN =====================
        [HttpGet]
        public async Task<IActionResult> ChiTietHoaDon(int id)
        {
            var donHang = await _context.DonHang
                .Include(d => d.NguoiDung)
                .Include(d => d.ChiTietDonHang)
                    .ThenInclude(ct => ct.BienThe)
                        .ThenInclude(bt => bt.SanPham)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donHang == null)
            {
                TempData["Error"] = "Không tìm thấy hóa đơn!";
                return RedirectToAction(nameof(DatHang));
            }

            return View(donHang);
        }

        // ===================== QUẢN LÝ ĐƠN HÀNG =====================
        public async Task<IActionResult> DatHang(string search, string status, int page = 1)
        {
            const int pageSize = 10;

            var query = _context.DonHang
                .Include(d => d.NguoiDung)
                .AsQueryable();

            // Tìm kiếm theo mã đơn hàng hoặc tên khách hàng
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => 
                    d.MaDonHang.Contains(search) || 
                    d.NguoiDung.HoTen.Contains(search) ||
                    d.NguoiDung.Email.Contains(search)
                );
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(status))
            {
                string statusDisplay = status switch
                {
                    "cho_xac_nhan" => "Chờ xác nhận",
                    "da_xac_nhan" => "Đã xác nhận",
                    "dang_giao" => "Đang giao",
                    "da_giao" => "Đã giao",
                    "da_huy" => "Đã hủy",
                    _ => status
                };
                query = query.Where(d => d.TrangThaiDonHang == statusDisplay);
            }

            query = query.OrderByDescending(d => d.NgayTao);

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var donHangList = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // ViewBag data
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.PageStartIndex = (page - 1) * pageSize;

            return View(donHangList);
        }

        // ===================== CẬP NHẬT TRẠNG THÁI ĐƠN HÀNG =====================
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            try
            {
                // Trim và normalize
                status = status?.Trim();
                if (!string.IsNullOrEmpty(status))
                {
                    status = status.Normalize(System.Text.NormalizationForm.FormC);
                }
                
                if (string.IsNullOrEmpty(status))
                {
                    TempData["Error"] = "Trạng thái không được để trống!";
                    return RedirectToAction("ChiTietHoaDon", new { id });
                }

                // Sử dụng RAW SQL với parameter để đảm bảo encoding chính xác
                var sql = @"
                    UPDATE DonHang 
                    SET TrangThaiDonHang = @status, NgayCapNhat = @ngayCapNhat
                    WHERE Id = @id";

                var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql,
                    new Microsoft.Data.SqlClient.SqlParameter("@status", System.Data.SqlDbType.NVarChar) { Value = status },
                    new Microsoft.Data.SqlClient.SqlParameter("@ngayCapNhat", System.Data.SqlDbType.DateTime) { Value = DateTime.Now },
                    new Microsoft.Data.SqlClient.SqlParameter("@id", System.Data.SqlDbType.Int) { Value = id });

                if (rowsAffected == 0)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng!";
                    return RedirectToAction("ChiTietHoaDon", new { id });
                }

                TempData["Success"] = "Cập nhật trạng thái đơn hàng thành công!";
                
                // Kiểm tra xem có phải AJAX request không
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Cập nhật trạng thái đơn hàng thành công!" });
                }
                
                // Nếu là form POST thông thường, redirect về view chi tiết
                return RedirectToAction("ChiTietHoaDon", new { id });
            }
            catch (DbUpdateException dbEx)
            {
                var innerMsg = dbEx.InnerException?.Message ?? dbEx.Message;
                Console.WriteLine($"DbUpdateException: {innerMsg}");
                
                TempData["Error"] = "Lỗi database: " + innerMsg;
                
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Lỗi database: " + innerMsg });
                }
                
                return RedirectToAction("ChiTietHoaDon", new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                
                TempData["Error"] = "Lỗi: " + ex.Message;
                
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Lỗi: " + ex.Message });
                }
                
                return RedirectToAction("ChiTietHoaDon", new { id });
            }
        }
    }
}
