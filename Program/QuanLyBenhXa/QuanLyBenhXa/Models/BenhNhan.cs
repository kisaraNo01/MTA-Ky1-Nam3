using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhXa.Models
{
    public class BenhNhan
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [Display(Name = "Họ và tên")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên phải từ 2 đến 100 ký tự")]
        public string HoTen { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime NgaySinh { get; set; }

        [Display(Name = "Giới tính")]
        [StringLength(10, ErrorMessage = "Giới tính không quá 10 ký tự")]
        public string GioiTinh { get; set; } = string.Empty;

        [Display(Name = "Số BHYT")]
        [StringLength(20, ErrorMessage = "Số BHYT không quá 20 ký tự")]
        public string SoBHYT { get; set; } = string.Empty;

        [Display(Name = "Đơn vị")]
        [StringLength(100, ErrorMessage = "Đơn vị không quá 100 ký tự")]
        public string DonVi { get; set; } = string.Empty;

        [Display(Name = "Cấp bậc")]
        [StringLength(50, ErrorMessage = "Cấp bậc không quá 50 ký tự")]
        public string CapBac { get; set; } = string.Empty;

        [Display(Name = "Chức vụ")]
        [StringLength(50, ErrorMessage = "Chức vụ không quá 50 ký tự")]
        public string ChucVu { get; set; } = string.Empty;

        public virtual ICollection<HoSoKhamBenh> HoSoKhamBenhs { get; set; } = new List<HoSoKhamBenh>();
    }
}
