using System.ComponentModel.DataAnnotations;

namespace SHOESTKC.CSDL
{
    [MetadataType(typeof(SizeMetadata))]
    public partial class Size
    {
    }

    public class SizeMetadata
    {
        [Required(ErrorMessage = "Vui lòng nhập tên size")]
        [StringLength(20, ErrorMessage = "Tên size không được vượt quá 20 ký tự")]
        [Display(Name = "Tên size")]
        public string TenSize { get; set; }

        [Range(1, 999, ErrorMessage = "Thứ tự phải từ 1 đến 999")]
        [Display(Name = "Thứ tự")]
        public int? ThuTu { get; set; }

        [Display(Name = "Trạng thái")]
        public bool? TrangThai { get; set; }
    }
}
