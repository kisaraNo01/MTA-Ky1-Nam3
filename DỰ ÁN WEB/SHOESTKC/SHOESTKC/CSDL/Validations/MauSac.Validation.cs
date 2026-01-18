using System.ComponentModel.DataAnnotations;

namespace SHOESTKC.CSDL
{
    [MetadataType(typeof(MauSacMetadata))]
    public partial class MauSac
    {
    }

    public class MauSacMetadata
    {
        [Required(ErrorMessage = "Vui lòng nhập tên màu")]
        [StringLength(50, ErrorMessage = "Tên màu không được vượt quá 50 ký tự")]
        [Display(Name = "Tên màu")]
        public string TenMau { get; set; }

        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "Mã màu hex phải có định dạng #RRGGBB (ví dụ: #FF0000)")]
        [Display(Name = "Mã màu Hex")]
        public string? MaHex { get; set; }

        [Display(Name = "Trạng thái")]
        public bool? TrangThai { get; set; }
    }
}
