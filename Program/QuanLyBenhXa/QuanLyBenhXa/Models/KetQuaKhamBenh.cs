using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBenhXa.Models
{
    public class KetQuaKhamBenh
    {
        [Key]
        public int Id { get; set; }

        public int HoSoKhamBenhId { get; set; }

        [ForeignKey("HoSoKhamBenhId")]
        public virtual HoSoKhamBenh? HoSoKhamBenh { get; set; }

        [Display(Name = "Tên phòng khám")]
        [StringLength(100, ErrorMessage = "Tên phòng khám không quá 100 ký tự")]
        public string TenPhongKham { get; set; } = string.Empty;

        [Display(Name = "Kết quả (JSON)")]
        public string KetQua { get; set; } = "{}"; // Stores JSON data

        [DataType(DataType.Date)]
        [Display(Name = "Ngày khám")]
        public DateTime NgayKham { get; set; } = DateTime.Now;

        [Display(Name = "Bác sĩ thực hiện")]
        [StringLength(100, ErrorMessage = "Bác sĩ thực hiện không quá 100 ký tự")]
        public string BacSiThucHien { get; set; } = string.Empty;
    }
}
