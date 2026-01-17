using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhXa.Models
{
    public class Thuoc
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên thuốc là bắt buộc")]
        [Display(Name = "Tên thuốc")]
        [StringLength(100, ErrorMessage = "Tên thuốc không quá 100 ký tự")]
        public string TenThuoc { get; set; } = string.Empty;

        [Display(Name = "Nhà sản xuất")]
        [StringLength(100, ErrorMessage = "Nhà sản xuất không quá 100 ký tự")]
        public string NhaSanXuat { get; set; } = string.Empty;

        [Display(Name = "Đơn vị tính")]
        [StringLength(20, ErrorMessage = "Đơn vị tính không quá 20 ký tự")]
        public string DonViTinh { get; set; } = string.Empty;

        [Display(Name = "Số lượng tồn kho")]
        [Range(0, 1000000, ErrorMessage = "Số lượng tồn kho phải từ 0 đến 1,000,000")]
        public int SoLuongTon { get; set; }

        [Display(Name = "Đơn giá")]
        [Range(0, 1000000000, ErrorMessage = "Đơn giá phải từ 0 đến 1,000,000,000")]
        public decimal DonGia { get; set; }

        [Display(Name = "Hàm lượng")]
        [StringLength(50, ErrorMessage = "Hàm lượng không quá 50 ký tự")]
        public string HamLuong { get; set; } = string.Empty;

        [Display(Name = "Cách dùng")]
        [StringLength(200, ErrorMessage = "Cách dùng không quá 200 ký tự")]
        public string CachDung { get; set; } = string.Empty;
    }
}
