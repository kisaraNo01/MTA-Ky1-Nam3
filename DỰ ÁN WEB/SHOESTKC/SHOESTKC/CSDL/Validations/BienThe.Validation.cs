using System.ComponentModel.DataAnnotations;

namespace SHOESTKC.CSDL
{
    [MetadataType(typeof(BienTheMetadata))]
    public partial class BienThe
    {
    }

    public class BienTheMetadata
    {
        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Sản phẩm")]
        public int SanPhamId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập size")]
        [StringLength(10, ErrorMessage = "Size không được vượt quá 10 ký tự")]
        [Display(Name = "Size")]
        public string Size { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập màu sắc")]
        [StringLength(50, ErrorMessage = "Màu sắc không được vượt quá 50 ký tự")]
        [Display(Name = "Màu sắc")]
        public string MauSac { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã SKU")]
        [StringLength(50, ErrorMessage = "Mã SKU không được vượt quá 50 ký tự")]
        [Display(Name = "Mã SKU")]
        public string MaSku { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng tồn")]
        [Range(0, 999999, ErrorMessage = "Số lượng tồn phải từ 0 đến 999,999")]
        [Display(Name = "Số lượng tồn")]
        public int SoLuongTon { get; set; }

        [Range(-999999999, 999999999, ErrorMessage = "Chênh lệch giá phải từ -999,999,999đ đến 999,999,999đ")]
        [Display(Name = "Chênh lệch giá")]
        public decimal? ChenhLechGia { get; set; }
    }
}
