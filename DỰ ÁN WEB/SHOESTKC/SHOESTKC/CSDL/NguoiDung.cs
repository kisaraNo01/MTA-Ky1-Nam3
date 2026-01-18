using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SHOESTKC.CSDL;

[Table("NguoiDung")]
[Index("Email", Name = "UQ__NguoiDun__A9D105342071EA08", IsUnique = true)]
public partial class NguoiDung
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string MatKhau { get; set; } = null!;

    [StringLength(255)]
    public string HoTen { get; set; } = null!;

    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [StringLength(500)]
    public string? DiaChi { get; set; }

    [StringLength(20)]
    public string VaiTro { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayCapNhat { get; set; }

    [InverseProperty("NguoiDung")]
    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();

    [InverseProperty("NguoiDung")]
    public virtual ICollection<DonHang> DonHang { get; set; } = new List<DonHang>();

    [InverseProperty("NguoiDung")]
    public virtual ICollection<GioHang> GioHang { get; set; } = new List<GioHang>();
}
