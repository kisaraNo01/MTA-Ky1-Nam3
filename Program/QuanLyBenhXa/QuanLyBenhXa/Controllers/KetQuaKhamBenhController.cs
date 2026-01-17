using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using QuanLyBenhXa.Models;
using System.Text.Json;

namespace QuanLyBenhXa.Controllers
{
    [Authorize(Roles = "Admin,BacSi")]
    public class KetQuaKhamBenhController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public KetQuaKhamBenhController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: KetQuaKhamBenh/Create?hosoId=5&phongKham=DaKhoa
        public async Task<IActionResult> Create(int? hosoId, string phongKham)
        {
            if (hosoId == null || string.IsNullOrEmpty(phongKham))
            {
                return NotFound();
            }

            var hoso = _context.HoSoKhamBenhs
                .Include(h => h.BenhNhan)
                .FirstOrDefault(m => m.Id == hosoId);

            if (hoso == null)
            {
                return NotFound();
            }

            // Authorization Check
            if (!User.IsInRole("Admin"))
            {
                var user = await _userManager.GetUserAsync(User);
                var bacSi = await _context.BacSis.FindAsync(user?.BacSiId);
                
                if (bacSi == null) return Forbid();

                if (phongKham == "Đa khoa")
                {
                    // For General Exam: Allow if the doctor is from "Đa khoa" clinic.
                    // Previously restricted to only the assigned "BacSiPhuTrach", but any General Doctor should be able to pick up the exam.
                    if (bacSi.PhongKham != "Đa khoa") return Forbid();
                }
                else
                {
                    // For Specialist: Must allow if user's clinic matches requested clinic
                    if (bacSi.PhongKham != phongKham) return Forbid();
                }
            }

            // Fetch General Exam Result (if applicable)
            if (phongKham != "Đa khoa")
            {
                var daKhoaResult = await _context.KetQuaKhamBenhs
                    .FirstOrDefaultAsync(k => k.HoSoKhamBenhId == hosoId && k.TenPhongKham == "Đa khoa");

                if (daKhoaResult != null)
                {
                    ViewBag.KetQuaDaKhoa = JsonSerializer.Deserialize<Dictionary<string, string>>(daKhoaResult.KetQua);
                }
            }
            
            ViewBag.HoSoId = hosoId;
            ViewBag.BenhNhan = hoso.BenhNhan;
            ViewBag.TrieuChung = hoso.TrieuChung;
            ViewBag.BenhNhanName = hoso.BenhNhan?.HoTen;
            ViewBag.PhongKham = phongKham;
            
            // Get list of Doctor names for dropdown, filtered by the current Clinic (PhongKham)
            var bacSiQuery = _context.BacSis.Where(b => b.PhongKham == phongKham);
            var bacSis = bacSiQuery.Select(b => b.Ten).ToList();
            
            // Pre-select current doctor if applicable
            string? defaultBacSi = null;
            if (User.IsInRole("BacSi"))
            {
                 var currentUser = await _userManager.GetUserAsync(User);
                 var currentBacSi = await _context.BacSis.FindAsync(currentUser?.BacSiId);
                 if (currentBacSi != null && currentBacSi.PhongKham == phongKham)
                 {
                     defaultBacSi = currentBacSi.Ten;
                 }
            }
            
            ViewBag.BacSiList = new SelectList(bacSis, defaultBacSi);

            var model = new KetQuaKhamBenh
            {
                NgayKham = DateTime.Now
            };

            return View(model);
        }

        // POST: KetQuaKhamBenh/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int HoSoKhamBenhId, string TenPhongKham, DateTime NgayKham, string BacSiThucHien, IFormCollection form)
        {
            // Authorization Check (Repeat check for safety)
            if (!User.IsInRole("Admin"))
            {
                var user = await _userManager.GetUserAsync(User);
                 var hosoCheck = await _context.HoSoKhamBenhs.AsNoTracking().FirstOrDefaultAsync(h => h.Id == HoSoKhamBenhId);
                 var bacSi = await _context.BacSis.FindAsync(user?.BacSiId);
                 
                 if (bacSi == null || hosoCheck == null) return Forbid();

                 if (TenPhongKham == "Đa khoa") 
                 { 
                     // Allow any General Doctor
                     if (bacSi.PhongKham != "Đa khoa") return Forbid(); 
                 }
                 else 
                 { 
                     if (bacSi.PhongKham != TenPhongKham) return Forbid(); 
                 }
            }

            // Create a dictionary to hold dynamic fields
            var resultData = new Dictionary<string, string>();

            foreach (var key in form.Keys)
            {
                // Exclude standard fields and anti-forgery token from JSON
                if (key != "HoSoKhamBenhId" && key != "TenPhongKham" && key != "NgayKham" && key != "BacSiThucHien" && key != "__RequestVerificationToken")
                {
                    resultData[key] = form[key];
                }
            }

            var ketQuaKhamBenh = new KetQuaKhamBenh
            {
                HoSoKhamBenhId = HoSoKhamBenhId,
                TenPhongKham = TenPhongKham,
                NgayKham = NgayKham,
                BacSiThucHien = BacSiThucHien,
                KetQua = JsonSerializer.Serialize(resultData)
            };

            if (ModelState.IsValid)
            {
                _context.Add(ketQuaKhamBenh);

                // Update Workflow requested clinics if General Exam
                // Update Workflow: General Doctor no longer assigns PhongYeuCau.
                // Logic moved to Attending Doctor (HoSoKhamBenhController.UpdatePhongYeuCau).
                /* 
                if (TenPhongKham == "Đa khoa" && form.ContainsKey("PhongYeuCau"))
                {
                    // Logic Removed
                } 
                */

                await _context.SaveChangesAsync();
                // Redirect back to Patient Details via HoSo lookup
                 var hoso = await _context.HoSoKhamBenhs.FindAsync(HoSoKhamBenhId);
                 return RedirectToAction("Details", "BenhNhan", new { id = hoso?.BenhNhanId });
            }
             
            // Reload view data if fail
             var existingHoso = _context.HoSoKhamBenhs.Include(h => h.BenhNhan).FirstOrDefault(m => m.Id == HoSoKhamBenhId);
            ViewBag.HoSoId = HoSoKhamBenhId;
             ViewBag.BenhNhanName = existingHoso?.BenhNhan?.HoTen;
            ViewBag.PhongKham = TenPhongKham;
            ViewBag.BacSiList = new SelectList(_context.BacSis.Select(b => b.Ten).ToList(), BacSiThucHien);

            return View(ketQuaKhamBenh);
        }

        // GET: KetQuaKhamBenh/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ketQua = await _context.KetQuaKhamBenhs
                .Include(k => k.HoSoKhamBenh)
                .ThenInclude(h => h.BenhNhan)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (ketQua == null) return NotFound();

            return View(ketQua);
        }
    }
}
