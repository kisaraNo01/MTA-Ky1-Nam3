using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBenhXa.Models
{
    public class HoSoKhamBenh
    {
        [Key]
        public int Id { get; set; }

        public int BenhNhanId { get; set; }

        [ForeignKey("BenhNhanId")]
        public virtual BenhNhan? BenhNhan { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày khám")]
        public DateTime NgayKham { get; set; } = DateTime.Now;

        [Display(Name = "Bác sĩ phụ trách")]
        [StringLength(100, ErrorMessage = "Bác sĩ phụ trách không quá 100 ký tự")]
        public string BacSiPhuTrach { get; set; } = string.Empty;

        [Display(Name = "Triệu chứng")]
        [StringLength(1000, ErrorMessage = "Triệu chứng không quá 1000 ký tự")]
        public string TrieuChung { get; set; } = string.Empty;

        [Display(Name = "Kết luận")]
        [StringLength(1000, ErrorMessage = "Kết luận không quá 1000 ký tự")]
        public string KetLuan { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Phòng yêu cầu không quá 200 ký tự")]
        public string PhongYeuCau { get; set; } = string.Empty; // Comma-separated list of required clinics

        [Display(Name = "Đã thanh toán")]
        public bool DaThanhToan { get; set; } = false;

        [Display(Name = "Tổng tiền")]
        [Range(0, 10000000000, ErrorMessage = "Tổng tiền không hợp lệ")]
        public decimal TongTien { get; set; } = 0;

        public virtual ICollection<KetQuaKhamBenh> KetQuaKhamBenhs { get; set; } = new List<KetQuaKhamBenh>();
        public virtual ICollection<DonThuoc> DonThuocs { get; set; } = new List<DonThuoc>();
    }
}
