using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOESTKC.CSDL;
using SHOESTKC.Data;
using Microsoft.AspNetCore.Http;

namespace SHOESTKC.Controllers
{
    public class AuthController : Controller
    {
        private readonly CSDLShoesShopDbContext _context;

        public AuthController(CSDLShoesShopDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string matKhau)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matKhau))
            {
                TempData["Error"] = "Vui lòng nhập email và mật khẩu!";
                return View();
            }

            var user = await _context.NguoiDung
                .FirstOrDefaultAsync(u => u.Email == email && u.MatKhau == matKhau);

            if (user == null)
            {
                TempData["Error"] = "Email hoặc mật khẩu không đúng!";
                return View();
            }

            // Lưu session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.HoTen);
            HttpContext.Session.SetString("UserRole", user.VaiTro);

            TempData["Success"] = "Đăng nhập thành công!";

            if (user.VaiTro == "admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(NguoiDung model, string confirmPassword)
        {
            if (model.MatKhau != confirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Mật khẩu xác nhận không khớp!");
                return View(model);
            }

            var exists = await _context.NguoiDung.AnyAsync(u => u.Email == model.Email);
            if (exists)
            {
                TempData["Error"] = "Email đã tồn tại!";
                return View(model);
            }

            // Set default values
            model.VaiTro = "khach_hang";
            model.NgayTao = DateTime.Now;
            model.NgayCapNhat = DateTime.Now;

            _context.NguoiDung.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
        }

        // GET: /Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
