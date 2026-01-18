using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOESTKC.Data;
using SHOESTKC.CSDL;

namespace SHOESTKC.Controllers
{
    public class MaKhuyenMaiController : Controller
    {
        private readonly CSDLShoesShopDbContext _context;

        public MaKhuyenMaiController(CSDLShoesShopDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // GET: Danh sách mã khuyến mãi
        // ==========================================
        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            int pageSize = 10;

            var query = _context.MaKhuyenMai.AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                query = query.Where(mk =>
                    mk.MaCode.ToLower().Contains(search.ToLower()));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Đảm bảo page hợp lệ
            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var maKhuyenMai = await query
                .OrderByDescending(mk => mk.NgayTao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotPages = totalPages;
            ViewBag.TotItems = totalItems;
            ViewBag.Search = search;

            // Trả về view trong thư mục Admin
            return View("~/Views/Admin/MaKhuyenMai.cshtml", maKhuyenMai);
        }

        // ==========================================
        // GET: Thêm mã khuyến mãi
        // ==========================================
        public IActionResult Create()
        {
            return View("~/Views/Admin/ThemMaKhuyenMai.cshtml");
        }

        // POST: Thêm mã khuyến mãi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaKhuyenMai maKhuyenMai)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra mã code đã tồn tại
                var existingCode = await _context.MaKhuyenMai
                    .FirstOrDefaultAsync(mk => mk.MaCode == maKhuyenMai.MaCode);

                if (existingCode != null)
                {
                    ModelState.AddModelError("MaCode", "Mã code đã tồn tại");
                    return View("~/Views/Admin/ThemMaKhuyenMai.cshtml", maKhuyenMai);
                }

                // Kiểm tra ngày
                if (maKhuyenMai.NgayKetThuc <= maKhuyenMai.NgayBatDau)
                {
                    ModelState.AddModelError(
                        "NgayKetThuc",
                        "Ngày kết thúc phải sau ngày bắt đầu"
                    );
                    return View("~/Views/Admin/ThemMaKhuyenMai.cshtml", maKhuyenMai);
                }

                maKhuyenMai.NgayTao = DateTime.Now;
                maKhuyenMai.SoLanDaDung = 0;

                _context.Add(maKhuyenMai);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Thêm mã khuyến mãi thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/ThemMaKhuyenMai.cshtml", maKhuyenMai);
        }

        // ==========================================
        // GET: Sửa mã khuyến mãi
        // ==========================================
        public async Task<IActionResult> Edit(int id)
        {
            var maKhuyenMai = await _context.MaKhuyenMai.FindAsync(id);
            if (maKhuyenMai == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/SuaMaKhuyenMai.cshtml", maKhuyenMai);
        }

        // POST: Sửa mã khuyến mãi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaKhuyenMai maKhuyenMai)
        {
            if (id != maKhuyenMai.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra mã code trùng (trừ chính nó)
                    var existingCode = await _context.MaKhuyenMai
                        .FirstOrDefaultAsync(mk =>
                            mk.MaCode == maKhuyenMai.MaCode && mk.Id != id);

                    if (existingCode != null)
                    {
                        ModelState.AddModelError("MaCode", "Mã code đã tồn tại");
                        return View("~/Views/Admin/SuaMaKhuyenMai.cshtml", maKhuyenMai);
                    }

                    // Kiểm tra ngày
                    if (maKhuyenMai.NgayKetThuc <= maKhuyenMai.NgayBatDau)
                    {
                        ModelState.AddModelError(
                            "NgayKetThuc",
                            "Ngày kết thúc phải sau ngày bắt đầu"
                        );
                        return View("~/Views/Admin/SuaMaKhuyenMai.cshtml", maKhuyenMai);
                    }

                    _context.Update(maKhuyenMai);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Cập nhật mã khuyến mãi thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await MaKhuyenMaiExists(maKhuyenMai.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            return View("~/Views/Admin/SuaMaKhuyenMai.cshtml", maKhuyenMai);
        }

        // ==========================================
        // POST: Xóa mã khuyến mãi
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var maKhuyenMai = await _context.MaKhuyenMai.FindAsync(id);
                if (maKhuyenMai == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy mã khuyến mãi" });
                }

                // Kiểm tra có đơn hàng sử dụng không
                var coDonHang = await _context.DonHang
                    .AnyAsync(dh => dh.MaKhuyenMaiId == id);

                if (coDonHang)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Không thể xóa mã khuyến mãi đã được sử dụng"
                    });
                }

                _context.MaKhuyenMai.Remove(maKhuyenMai);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Xóa mã khuyến mãi thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // ==========================================
        // Bật / Tắt trạng thái
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var maKhuyenMai = await _context.MaKhuyenMai.FindAsync(id);
                if (maKhuyenMai == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy mã khuyến mãi" });
                }

                maKhuyenMai.TrangThai = !maKhuyenMai.TrangThai;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        private async Task<bool> MaKhuyenMaiExists(int id)
        {
            return await _context.MaKhuyenMai.AnyAsync(e => e.Id == id);
        }
    }
}
