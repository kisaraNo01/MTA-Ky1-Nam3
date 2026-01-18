using System.ComponentModel.DataAnnotations;

namespace SHOESTKC.CSDL
{
    [MetadataType(typeof(MaKhuyenMaiMetadata))]
    public partial class MaKhuyenMai
    {
    }

    public class MaKhuyenMaiMetadata
    {
        [Required(ErrorMessage = "Vui lòng nhập mã code")]
        [StringLength(50, ErrorMessage = "Mã code không được vượt quá 50 ký tự")]
        [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Mã code chỉ được chứa chữ IN HOA và số (ví dụ: SALE2024)")]
        [Display(Name = "Mã code")]
        public string MaCode { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại khuyến mãi")]
        [Display(Name = "Loại khuyến mãi")]
        public string LoaiKm { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá trị giảm")]
        [Range(0.01, 999999999, ErrorMessage = "Giá trị giảm phải lớn hơn 0")]
        [Display(Name = "Giá trị giảm")]
        public decimal GiaTriGiam { get; set; }

        [Range(0, 999999999, ErrorMessage = "Giá trị đơn tối thiểu không hợp lệ")]
        [Display(Name = "Giá trị đơn tối thiểu")]
        public decimal? GiaTriDonToiThieu { get; set; }

        [Range(1, 999999, ErrorMessage = "Số lần dùng tối đa phải từ 1 đến 999,999")]
        [Display(Name = "Số lần dùng tối đa")]
        public int? SoLanDungToiDa { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime NgayBatDau { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày kết thúc")]
        [Display(Name = "Ngày kết thúc")]
        public DateTime NgayKetThuc { get; set; }

        [Display(Name = "Trạng thái")]
        public bool? TrangThai { get; set; }
    }
}
