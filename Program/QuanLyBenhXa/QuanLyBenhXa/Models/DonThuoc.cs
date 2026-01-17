using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBenhXa.Models
{
    public class DonThuoc
    {
        [Key]
        public int Id { get; set; }

        public int HoSoKhamBenhId { get; set; }
        
        [ForeignKey("HoSoKhamBenhId")]
        public virtual HoSoKhamBenh? HoSoKhamBenh { get; set; }

        public int ThuocId { get; set; }
        
        [ForeignKey("ThuocId")]
        public virtual Thuoc? Thuoc { get; set; }

        [Display(Name = "Số lượng")]
        [Range(1, 1000, ErrorMessage = "Số lượng phải từ 1 đến 1,000")]
        public int SoLuong { get; set; }

        [Display(Name = "Cách dùng")]
        [StringLength(200, ErrorMessage = "Cách dùng không quá 200 ký tự")]
        public string CachDung { get; set; } = string.Empty;
    }
}
