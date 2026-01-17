using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBenhXa.Helpers;
using QuanLyBenhXa.Models;

namespace QuanLyBenhXa.Controllers
{
    [Authorize(Roles = "Admin,NhanVien")]
    public class ThanhToanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThanhToanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List unpaid concluded records
        public async Task<IActionResult> Index()
        {
            var unpaidRecords = await _context.HoSoKhamBenhs
                .Include(h => h.BenhNhan)
                .Where(h => !h.DaThanhToan && !string.IsNullOrEmpty(h.KetLuan) && h.DonThuocs.Any())
                .OrderByDescending(h => h.NgayKham)
                .ToListAsync();

            return View(unpaidRecords);
        }

        // View Invoice
        public async Task<IActionResult> Invoice(int id)
        {
            var hoso = await _context.HoSoKhamBenhs
                .Include(h => h.BenhNhan)
                .Include(h => h.KetQuaKhamBenhs)
                .Include(h => h.DonThuocs)
                .ThenInclude(d => d.Thuoc)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hoso == null) return NotFound();

            var viewModel = new InvoiceViewModel
            {
                HoSo = hoso
            };

            // Calculate Exam Fees
            foreach (var kq in hoso.KetQuaKhamBenhs)
            {
                var price = ClinicPriceHelper.GetPrice(kq.TenPhongKham);
                viewModel.ExamFees.Add(new FeeItem { Name = $"Khám {kq.TenPhongKham}", Amount = price });
                viewModel.TotalExamFee += price;
            }

            // Calculate Medicine Cost
            foreach (var dt in hoso.DonThuocs)
            {
                var cost = dt.SoLuong * dt.Thuoc.DonGia;
                viewModel.MedicineCosts.Add(new FeeItem { Name = $"{dt.Thuoc.TenThuoc} (x{dt.SoLuong})", Amount = cost });
                viewModel.TotalMedicineCost += cost;
            }

            viewModel.TotalAmount = viewModel.TotalExamFee + viewModel.TotalMedicineCost;

            // Military Insurance Logic: Free if CapBac is present
            if (!string.IsNullOrEmpty(hoso.BenhNhan?.CapBac))
            {
                viewModel.IsFree = true;
                viewModel.TotalAmount = 0;
            }

            return View(viewModel);
        }

        // Confirm Payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id, decimal totalAmount)
        {
            var hoso = await _context.HoSoKhamBenhs.FindAsync(id);
            if (hoso == null) return NotFound();

            hoso.DaThanhToan = true;
            hoso.TongTien = totalAmount;

            _context.Update(hoso);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

    public class InvoiceViewModel
    {
        public HoSoKhamBenh HoSo { get; set; }
        public List<FeeItem> ExamFees { get; set; } = new List<FeeItem>();
        public List<FeeItem> MedicineCosts { get; set; } = new List<FeeItem>();
        public decimal TotalExamFee { get; set; }
        public decimal TotalMedicineCost { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsFree { get; set; }
    }

    public class FeeItem
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}
