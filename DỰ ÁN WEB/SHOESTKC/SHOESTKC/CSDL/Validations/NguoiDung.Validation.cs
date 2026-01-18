using System.ComponentModel.DataAnnotations;

namespace SHOESTKC.CSDL
{
    [MetadataType(typeof(NguoiDungMetadata))]
    public partial class NguoiDung
    {
    }

    public class NguoiDungMetadata
    {
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(255, ErrorMessage = "Email không được vượt quá 255 ký tự")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Mật khẩu phải có ít nhất 3 ký tự")]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(255, ErrorMessage = "Họ tên không được vượt quá 255 ký tự")]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        [Display(Name = "Số điện thoại")]
        public string? SoDienThoai { get; set; }

        [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }
    }
}
