using System.ComponentModel.DataAnnotations;

namespace SHOESTKC.CSDL
{
    [MetadataType(typeof(SanPhamMetadata))]
    public partial class SanPham
    {
    }

    public class SanPhamMetadata
    {
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string TenSanPham { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public int DanhMucId { get; set; }

        [StringLength(100, ErrorMessage = "Tên hãng không được vượt quá 100 ký tự")]
        [Display(Name = "Hãng")]
        public string? Hang { get; set; }

        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
        [Range(1000, 999999999, ErrorMessage = "Giá sản phẩm phải từ 1,000đ đến 999,999,999đ")]
        [Display(Name = "Giá gốc")]
        public decimal GiaGoc { get; set; }

        [Display(Name = "Ảnh chính")]
        public string? AnhChinh { get; set; }

        [Display(Name = "Trạng thái")]
        public bool? TrangThai { get; set; }
    }
}
