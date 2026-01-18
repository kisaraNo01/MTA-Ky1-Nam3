using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOESTKC.Data;
using SHOESTKC.CSDL;

namespace SHOESTKC.Controllers
{
    public class OrderController : Controller
    {
        private readonly CSDLShoesShopDbContext _context;

        public OrderController(CSDLShoesShopDbContext context)
        {
            _context = context;
        }

        // GET: Trang thanh toán
        public async Task<IActionResult> Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để thanh toán!";
                return RedirectToAction("Login", "Auth");
            }

            // Lấy giỏ hàng
            var cartItems = await _context.GioHang
                .Include(g => g.BienThe)
                    .ThenInclude(b => b.SanPham)
                .Where(g => g.NguoiDungId == userId.Value)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "GioHang");
            }

            // Tính tổng tiền
            decimal tongTien = 0;
            foreach (var item in cartItems)
            {
                decimal giaGoc = item.BienThe.SanPham.GiaGoc;
                decimal chenhLech = item.BienThe.ChenhLechGia ?? 0;
                tongTien += (giaGoc + chenhLech) * item.SoLuong;
            }

            // Lấy thông tin người dùng
            var user = await _context.NguoiDung.FindAsync(userId.Value);

            ViewBag.CartItems = cartItems;
            ViewBag.TongTien = tongTien;
            ViewBag.PhiVanChuyen = 30000m;
            ViewBag.TongThanhToan = tongTien + 30000m;
            ViewBag.UserInfo = user;

            return View();
        }

        // POST: Áp dụng mã khuyến mãi
        [HttpPost]
        public async Task<IActionResult> ApplyPromoCode(string maCode)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });

            if (string.IsNullOrWhiteSpace(maCode))
                return Json(new { success = false, message = "Vui lòng nhập mã khuyến mãi!" });

            // Tìm mã khuyến mãi
            var promo = await _context.MaKhuyenMai
                .FirstOrDefaultAsync(m => m.MaCode == maCode.Trim());

            if (promo == null)
                return Json(new { success = false, message = "Mã khuyến mãi không tồn tại!" });

            // Kiểm tra trạng thái
            if (promo.TrangThai == false)
                return Json(new { success = false, message = "Mã khuyến mãi đã bị vô hiệu hóa!" });

            // Kiểm tra thời gian
            var now = DateTime.Now;
            if (now < promo.NgayBatDau)
                return Json(new { success = false, message = "Mã khuyến mãi chưa có hiệu lực!" });

            if (now > promo.NgayKetThuc)
                return Json(new { success = false, message = "Mã khuyến mãi đã hết hạn!" });

            // Kiểm tra số lần sử dụng
            if (promo.SoLanDungToiDa.HasValue && promo.SoLanDaDung >= promo.SoLanDungToiDa)
                return Json(new { success = false, message = "Mã khuyến mãi đã hết lượt sử dụng!" });

            // Tính tổng tiền giỏ hàng
            var cartItems = await _context.GioHang
                .Include(g => g.BienThe)
                    .ThenInclude(b => b.SanPham)
                .Where(g => g.NguoiDungId == userId.Value)
                .ToListAsync();

            decimal tongTien = 0;
            foreach (var item in cartItems)
            {
                decimal giaGoc = item.BienThe.SanPham.GiaGoc;
                decimal chenhLech = item.BienThe.ChenhLechGia ?? 0;
                tongTien += (giaGoc + chenhLech) * item.SoLuong;
            }

            // Kiểm tra giá trị đơn tối thiểu
            if (promo.GiaTriDonToiThieu.HasValue && tongTien < promo.GiaTriDonToiThieu)
                return Json(new
                {
                    success = false,
                    message = $"Đơn hàng tối thiểu {promo.GiaTriDonToiThieu.Value:N0}đ để sử dụng mã này!"
                });

            // Tính tiền giảm
            decimal tienGiam = 0;
            if (promo.LoaiKm == "PhanTram")
            {
                tienGiam = tongTien * promo.GiaTriGiam / 100;
            }
            else if (promo.LoaiKm == "TienMat")
            {
                tienGiam = promo.GiaTriGiam;
            }

            decimal phiVanChuyen = 30000m;
            decimal tongThanhToan = tongTien + phiVanChuyen - tienGiam;
            if (tongThanhToan < 0) tongThanhToan = 0;

            return Json(new
            {
                success = true,
                message = "Áp dụng mã khuyến mãi thành công!",
                promoId = promo.Id,
                promoCode = promo.MaCode,
                loaiKm = promo.LoaiKm,
                giaTriGiam = promo.GiaTriGiam,
                tienGiam = tienGiam,
                tongTien = tongTien,
                phiVanChuyen = phiVanChuyen,
                tongThanhToan = tongThanhToan
            });
        }

        // POST: Đặt hàng
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string diaChiGiao, string soDienThoai,
            string phuongThucThanhToan, int? maKhuyenMaiId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập!";
                return RedirectToAction("Login", "Auth");
            }

            // Validate input
            if (string.IsNullOrWhiteSpace(diaChiGiao))
            {
                TempData["Error"] = "Vui lòng nhập địa chỉ giao hàng!";
                return RedirectToAction("Checkout");
            }

            if (string.IsNullOrWhiteSpace(soDienThoai))
            {
                TempData["Error"] = "Vui lòng nhập số điện thoại!";
                return RedirectToAction("Checkout");
            }

            if (string.IsNullOrWhiteSpace(phuongThucThanhToan))
            {
                TempData["Error"] = "Vui lòng chọn phương thức thanh toán!";
                return RedirectToAction("Checkout");
            }

            // Lấy giỏ hàng
            var cartItems = await _context.GioHang
                .Include(g => g.BienThe)
                    .ThenInclude(b => b.SanPham)
                .Where(g => g.NguoiDungId == userId.Value)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "GioHang");
            }

            // Kiểm tra tồn kho
            foreach (var item in cartItems)
            {
                if (item.BienThe.SoLuongTon < item.SoLuong)
                {
                    TempData["Error"] = $"Sản phẩm {item.BienThe.SanPham.TenSanPham} không đủ hàng!";
                    return RedirectToAction("Checkout");
                }
            }

            // Tính tổng tiền
            decimal tamTinh = 0;
            foreach (var item in cartItems)
            {
                decimal giaGoc = item.BienThe.SanPham.GiaGoc;
                decimal chenhLech = item.BienThe.ChenhLechGia ?? 0;
                tamTinh += (giaGoc + chenhLech) * item.SoLuong;
            }

            decimal tienGiam = 0;
            decimal phiVanChuyen = 30000m;

            // Áp dụng mã khuyến mãi nếu có
            if (maKhuyenMaiId.HasValue)
            {
                var promo = await _context.MaKhuyenMai.FindAsync(maKhuyenMaiId.Value);
                if (promo != null && promo.TrangThai == true)
                {
                    if (promo.LoaiKm == "PhanTram")
                    {
                        tienGiam = tamTinh * promo.GiaTriGiam / 100;
                    }
                    else if (promo.LoaiKm == "TienMat")
                    {
                        tienGiam = promo.GiaTriGiam;
                    }

                    // Cập nhật số lần đã dùng
                    promo.SoLanDaDung = (promo.SoLanDaDung ?? 0) + 1;
                }
            }

            decimal tongTien = tamTinh + phiVanChuyen - tienGiam;
            if (tongTien < 0) tongTien = 0;

            // Tạo mã đơn hàng
            string maDonHang = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // Sử dụng tiếng Việt có dấu
            string trangThaiThanhToan = phuongThucThanhToan == "COD" ? "Chưa thanh toán" : "Đã thanh toán";
            string trangThaiDonHang = "Chờ xác nhận";

            // Tạo đơn hàng
            var donHang = new DonHang
            {
                NguoiDungId = userId.Value,
                MaKhuyenMaiId = maKhuyenMaiId,
                MaDonHang = maDonHang,
                TamTinh = tamTinh,
                TienGiam = tienGiam > 0 ? tienGiam : null,
                PhiVanChuyen = phiVanChuyen,
                TongTien = tongTien,
                DiaChiGiao = diaChiGiao,
                SoDienThoai = soDienThoai,
                PhuongThucThanhToan = phuongThucThanhToan,
                TrangThaiThanhToan = trangThaiThanhToan, // ✅ FIXED: "Chưa thanh toán" hoặc "Đã thanh toán"
                TrangThaiDonHang = trangThaiDonHang,     // ✅ FIXED: "Chờ xác nhận"
                NgayTao = DateTime.Now,
                NgayCapNhat = DateTime.Now
            };

            _context.DonHang.Add(donHang);
            await _context.SaveChangesAsync();

            // Tạo chi tiết đơn hàng và cập nhật tồn kho
            foreach (var item in cartItems)
            {
                decimal giaGoc = item.BienThe.SanPham.GiaGoc;
                decimal chenhLech = item.BienThe.ChenhLechGia ?? 0;
                decimal giaBan = giaGoc + chenhLech;

                var chiTiet = new ChiTietDonHang
                {
                    DonHangId = donHang.Id,
                    BienTheId = item.BienTheId,
                    SoLuong = item.SoLuong,
                    DonGia = giaBan,
                    ThanhTien = giaBan * item.SoLuong
                };

                _context.ChiTietDonHang.Add(chiTiet);

                // Giảm tồn kho
                item.BienThe.SoLuongTon -= item.SoLuong;
            }

            // Xóa giỏ hàng
            _context.GioHang.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đặt hàng thành công!";
            return RedirectToAction("OrderSuccess", new { id = donHang.Id });
        }

        // GET: Trang xác nhận đơn hàng thành công
        public async Task<IActionResult> OrderSuccess(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var donHang = await _context.DonHang
                .Include(d => d.ChiTietDonHang)
                    .ThenInclude(c => c.BienThe)
                        .ThenInclude(b => b.SanPham)
                .Include(d => d.MaKhuyenMai)
                .FirstOrDefaultAsync(d => d.Id == id && d.NguoiDungId == userId.Value);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // GET: Danh sách đơn hàng của khách
        public async Task<IActionResult> MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập!";
                return RedirectToAction("Login", "Auth");
            }

            var orders = await _context.DonHang
                .Include(d => d.ChiTietDonHang)
                    .ThenInclude(c => c.BienThe)
                        .ThenInclude(b => b.SanPham)
                .Where(d => d.NguoiDungId == userId.Value)
                .OrderByDescending(d => d.NgayTao)
                .ToListAsync();

            return View(orders);
        }

        // GET: Chi tiết đơn hàng
        public async Task<IActionResult> OrderDetail(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var donHang = await _context.DonHang
                .Include(d => d.ChiTietDonHang)
                    .ThenInclude(c => c.BienThe)
                        .ThenInclude(b => b.SanPham)
                .Include(d => d.MaKhuyenMai)
                .FirstOrDefaultAsync(d => d.Id == id && d.NguoiDungId == userId.Value);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }
    }
}