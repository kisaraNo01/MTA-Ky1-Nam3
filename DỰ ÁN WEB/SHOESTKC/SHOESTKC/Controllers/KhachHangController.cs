using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOESTKC.Data;
using SHOESTKC.CSDL;
using SHOESTKC.Models;

// Alias để phân biệt rõ ràng
using NguoiDungEntity = SHOESTKC.CSDL.NguoiDung;

namespace SHOESTKC.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly CSDLShoesShopDbContext _context;

        public KhachHangController(CSDLShoesShopDbContext context)
        {
            _context = context;
        }

        // GET: Thông tin cá nhân (cho khách hàng đăng nhập)
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var nguoiDung = await _context.NguoiDung
                .Include(nd => nd.DonHang)
                    .ThenInclude(dh => dh.ChiTietDonHang)
                .Include(nd => nd.DanhGia)
                .FirstOrDefaultAsync(nd => nd.Id == userId);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        // GET: Danh sách khách hàng với phân trang
        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            int pageSize = 10;

            var query = _context.NguoiDung
                .Where(nd => nd.VaiTro == "khach_hang")
                .AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(nd =>
                    nd.HoTen.Contains(search) ||
                    nd.Email.Contains(search) ||
                    nd.SoDienThoai.Contains(search));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var khachHang = await query
                .OrderByDescending(nd => nd.NgayTao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.Search = search;

            return View(khachHang);
        }

        // GET: Chi tiết khách hàng
        public async Task<IActionResult> Details(int id)
        {
            var khachHang = await _context.NguoiDung
                .Include(nd => nd.DonHang)
                .Include(nd => nd.DanhGia)
                .FirstOrDefaultAsync(nd => nd.Id == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: Thêm khách hàng
        public IActionResult Create()
        {
            return View();
        }

        // POST: Thêm khách hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,MatKhau,HoTen,SoDienThoai,DiaChi")] NguoiDungEntity nguoiDung)
        {
            // Kiểm tra email đã tồn tại
            var existingEmail = await _context.NguoiDung
                .FirstOrDefaultAsync(nd => nd.Email == nguoiDung.Email);

            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại trong hệ thống");
                return View(nguoiDung);
            }

            if (string.IsNullOrEmpty(nguoiDung.MatKhau))
            {
                ModelState.AddModelError("MatKhau", "Vui lòng nhập mật khẩu");
                return View(nguoiDung);
            }

            nguoiDung.VaiTro = "khach_hang";
            nguoiDung.NgayTao = DateTime.Now;
            nguoiDung.NgayCapNhat = DateTime.Now;

            _context.Add(nguoiDung);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thêm khách hàng thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Sửa khách hàng
        public async Task<IActionResult> Edit(int id)
        {
            var khachHang = await _context.NguoiDung.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        // POST: Sửa khách hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NguoiDungEntity nguoiDung)
        {
            if (id != nguoiDung.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra email trùng (trừ chính nó)
                    var existingEmail = await _context.NguoiDung
                        .FirstOrDefaultAsync(nd => nd.Email == nguoiDung.Email && nd.Id != id);

                    if (existingEmail != null)
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại trong hệ thống");
                        return View(nguoiDung);
                    }

                    nguoiDung.NgayCapNhat = DateTime.Now;
                    _context.Update(nguoiDung);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Cập nhật khách hàng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await NguoiDungExists(nguoiDung.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(nguoiDung);
        }

        // POST: Cập nhật thông tin cá nhân
        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });
                }

                var nguoiDung = await _context.NguoiDung.FindAsync(userId);
                if (nguoiDung == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng" });
                }

                nguoiDung.HoTen = request.HoTen;
                nguoiDung.SoDienThoai = request.SoDienThoai;
                nguoiDung.DiaChi = request.DiaChi;
                nguoiDung.NgayCapNhat = DateTime.Now;

                _context.Update(nguoiDung);
                await _context.SaveChangesAsync();

                // Cập nhật session
                HttpContext.Session.SetString("UserName", nguoiDung.HoTen);

                return Json(new { success = true, message = "Cập nhật thông tin thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // POST: Đổi mật khẩu
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });
                }

                var nguoiDung = await _context.NguoiDung.FindAsync(userId);
                if (nguoiDung == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng" });
                }

                // Kiểm tra mật khẩu hiện tại
                if (nguoiDung.MatKhau != request.CurrentPassword)
                {
                    return Json(new { success = false, message = "Mật khẩu hiện tại không đúng!" });
                }

                // Cập nhật mật khẩu mới
                nguoiDung.MatKhau = request.NewPassword;
                nguoiDung.NgayCapNhat = DateTime.Now;

                _context.Update(nguoiDung);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // POST: Xóa khách hàng

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var khachHang = await _context.NguoiDung.FindAsync(id);
                if (khachHang == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy khách hàng" });
                }

                // Kiểm tra có đơn hàng không
                var coDonHang = await _context.DonHang.AnyAsync(dh => dh.NguoiDungId == id);
                if (coDonHang)
                {
                    return Json(new { success = false, message = "Không thể xóa khách hàng đã có đơn hàng" });
                }

                _context.NguoiDung.Remove(khachHang);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Xóa khách hàng thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        private async Task<bool> NguoiDungExists(int id)
        {
            return await _context.NguoiDung.AnyAsync(e => e.Id == id);
        }
    }
}
