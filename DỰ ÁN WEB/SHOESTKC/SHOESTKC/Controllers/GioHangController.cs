using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOESTKC.Data;
using SHOESTKC.CSDL;

namespace SHOESTKC.Controllers
{
    /// <summary>
    /// Controller quản lý giỏ hàng
    /// - Người dùng ĐĂNG NHẬP: Lưu vào DATABASE (như Shopee, Lazada)
    /// - Người dùng CHƯA ĐĂNG NHẬP: Lưu vào SESSION (tạm thời)
    /// - Khi đăng nhập: Merge giỏ hàng từ session vào database
    /// </summary>
    public class GioHangController : Controller
    {
        private readonly CSDLShoesShopDbContext _context;

        public GioHangController(CSDLShoesShopDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Xem giỏ hàng
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để xem giỏ hàng!";
                return RedirectToAction("Login", "Auth");
            }

            // Lấy giỏ hàng từ DATABASE
            var cartItems = await _context.GioHang
                .Include(g => g.BienThe)
                    .ThenInclude(b => b.SanPham)
                .Include(g => g.BienThe)
                    .ThenInclude(b => b.SizeNavigation)
                .Include(g => g.BienThe)
                    .ThenInclude(b => b.MauSacNavigation)
                .Where(g => g.NguoiDungId == userId.Value)
                .ToListAsync();

            decimal tongTien = 0;
            foreach (var item in cartItems)
            {
                decimal giaGoc = item.BienThe.SanPham.GiaGoc;
                decimal chenhLech = item.BienThe.ChenhLechGia ?? 0;
                tongTien += (giaGoc + chenhLech) * item.SoLuong;
            }

            ViewBag.TongTien = tongTien;
            ViewBag.PhiVanChuyen = 30000m;
            ViewBag.TongThanhToan = tongTien + ViewBag.PhiVanChuyen;

            return View(cartItems);
        }

        /// <summary>
        /// POST: Thêm vào giỏ hàng
        /// Nút mua hàng chứa ID của BienThe (biến thể sản phẩm)
        /// Mặc định bấm nút mua thì mua 1 cái
        /// Nếu sản phẩm đã có trong giỏ thì UPDATE số lượng, chưa có thì THÊM MỚI
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Add(int bienTheId, int soLuong = 1)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, requireLogin = true, message = "Vui lòng đăng nhập để mua hàng!" });

            // Validate số lượng
            if (soLuong <= 0)
            {
                return Json(new { success = false, message = "Số lượng phải lớn hơn 0!" });
            }

            // Kiểm tra số lượng có vượt quá giới hạn không (tối đa 999)
            if (soLuong > 999)
            {
                return Json(new { success = false, message = "Số lượng không được vượt quá 999!" });
            }

            // DEBUG: Log received ID
            Console.WriteLine($"[GioHang/Add] Received bienTheId: {bienTheId}");
            
            var bienThe = await _context.BienThe
                .Include(b => b.SanPham)
                .FirstOrDefaultAsync(b => b.Id == bienTheId);

            // DEBUG: Log query result
            if (bienThe != null)
                Console.WriteLine($"[GioHang/Add] Found: Id={bienThe.Id}, MauSac={bienThe.MauSac}, Size={bienThe.Size}");
            else
                Console.WriteLine($"[GioHang/Add] NOT FOUND for ID: {bienTheId}");

            if (bienThe == null)
                return Json(new { success = false, message = $"Sản phẩm không tồn tại! (BienTheId: {bienTheId})" });

            // Kiểm tra sản phẩm có đang hiển thị không
            if (bienThe.SanPham.TrangThai != true)
            {
                return Json(new { success = false, message = "Sản phẩm này hiện không còn bán!" });
            }

            // Kiểm tra số lượng tồn kho
            if (bienThe.SoLuongTon < soLuong)
                return Json(new { success = false, message = $"Kho chỉ còn {bienThe.SoLuongTon} sản phẩm!" });

            // Kiểm tra sản phẩm đã có trong giỏ chưa
            var existingItem = await _context.GioHang
                .FirstOrDefaultAsync(g => g.NguoiDungId == userId.Value && g.BienTheId == bienTheId);

            if (existingItem != null)
            {
                // ĐÃ CÓ SẢN PHẨM → UPDATE SỐ LƯỢNG
                int soLuongMoi = existingItem.SoLuong + soLuong;
                
                if (soLuongMoi > bienThe.SoLuongTon)
                    return Json(new { 
                        success = false, 
                        message = $"Số lượng trong giỏ ({existingItem.SoLuong}) + số lượng thêm ({soLuong}) vượt quá tồn kho ({bienThe.SoLuongTon})!" 
                    });

                if (soLuongMoi > 999)
                    return Json(new { success = false, message = "Số lượng trong giỏ không được vượt quá 999!" });

                existingItem.SoLuong = soLuongMoi;
                existingItem.NgayThem = DateTime.Now;
            }
            else
            {
                // CHƯA CÓ SẢN PHẨM → THÊM MỚI
                _context.GioHang.Add(new GioHang
                {
                    NguoiDungId = userId.Value,
                    BienTheId = bienTheId,
                    SoLuong = soLuong,
                    NgayThem = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();

            var cartCount = await _context.GioHang
                .Where(g => g.NguoiDungId == userId.Value)
                .SumAsync(g => g.SoLuong);

            return Json(new { success = true, message = "Đã thêm vào giỏ hàng!", cartCount });
        }

        /// <summary>
        /// POST: Cập nhật số lượng
        /// Kiểm tra số lượng không vượt quá tồn kho và giới hạn
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int soLuong)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });

            // Validate số lượng
            if (soLuong <= 0)
                return Json(new { success = false, message = "Số lượng phải lớn hơn 0!" });

            // Kiểm tra số lượng có vượt quá giới hạn không
            if (soLuong > 999)
                return Json(new { success = false, message = "Số lượng không được vượt quá 999!" });

            var gioHang = await _context.GioHang
                .Include(g => g.BienThe)
                .ThenInclude(b => b.SanPham)
                .FirstOrDefaultAsync(g => g.Id == id && g.NguoiDungId == userId.Value);

            if (gioHang == null)
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });

            // Kiểm tra tồn kho
            if (soLuong > gioHang.BienThe.SoLuongTon)
                return Json(new { success = false, message = $"Kho chỉ còn {gioHang.BienThe.SoLuongTon} sản phẩm!" });

            gioHang.SoLuong = soLuong;
            await _context.SaveChangesAsync();

            var cartItems = await _context.GioHang
                .Include(g => g.BienThe)
                    .ThenInclude(b => b.SanPham)
                .Where(g => g.NguoiDungId == userId.Value)
                .ToListAsync();

            decimal tongTienMoi = 0;
            foreach (var item in cartItems)
            {
                decimal giaGoc = item.BienThe.SanPham.GiaGoc;
                decimal chenhLech = item.BienThe.ChenhLechGia ?? 0;
                tongTienMoi += (giaGoc + chenhLech) * item.SoLuong;
            }

            return Json(new
            {
                success = true,
                message = "Cập nhật thành công",
                tongTien = tongTienMoi,
                tongThanhToan = tongTienMoi + 30000
            });
        }

        /// <summary>
        /// POST: Xóa sản phẩm
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });

            var gioHang = await _context.GioHang
                .FirstOrDefaultAsync(g => g.Id == id && g.NguoiDungId == userId.Value);

            if (gioHang == null)
                return Json(new { success = false, message = "Không tìm thấy sản phẩm!" });

            _context.GioHang.Remove(gioHang);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã xóa sản phẩm!" });
        }

        /// <summary>
        /// POST: Xóa toàn bộ giỏ hàng
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false });

            var items = await _context.GioHang
                .Where(g => g.NguoiDungId == userId.Value)
                .ToListAsync();

            _context.GioHang.RemoveRange(items);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Giỏ hàng đã được làm trống!" });
        }

        /// <summary>
        /// GET: Lấy số lượng sản phẩm trong giỏ
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { count = 0 });

            var count = await _context.GioHang
                .Where(g => g.NguoiDungId == userId.Value)
                .SumAsync(g => g.SoLuong);

            return Json(new { count });
        }
    }
}
