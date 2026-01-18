using SHOESTKC.CSDL;
using System.ComponentModel.DataAnnotations;

namespace SHOESTKC.Models
{
    public class NguoiDung
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string HoTen { get; set; } = string.Empty;

        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [StringLength(500)]
        public string? DiaChi { get; set; }

        [Required]
        public string VaiTro { get; set; } = "khach_hang";

        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<GioHang>? GioHangs { get; set; }
        public virtual ICollection<DonHang>? DonHangs { get; set; }
        public virtual ICollection<DanhGia>? DanhGia { get; set; }
    }
}