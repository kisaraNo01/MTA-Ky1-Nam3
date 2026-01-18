using System.ComponentModel.DataAnnotations;

namespace SHOESTKC.CSDL
{
    [MetadataType(typeof(DanhMucMetadata))]
    public partial class DanhMuc
    {
    }

    public class DanhMucMetadata
    {
        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [StringLength(255, ErrorMessage = "Tên danh mục không được vượt quá 255 ký tự")]
        [Display(Name = "Tên danh mục")]
        public string TenDanhMuc { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập slug")]
        [StringLength(255, ErrorMessage = "Slug không được vượt quá 255 ký tự")]
        [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug chỉ được chứa chữ thường, số và dấu gạch ngang (ví dụ: giay-the-thao)")]
        [Display(Name = "Slug (URL)")]
        public string Slug { get; set; }
    }
}
