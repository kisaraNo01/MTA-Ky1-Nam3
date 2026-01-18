using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SHOESTKC.CSDL;

[Table("DanhGia")]
public partial class DanhGia
{
    [Key]
    public int Id { get; set; }

    public int NguoiDungId { get; set; }

    public int SanPhamId { get; set; }

    public int DonHangId { get; set; }

    public int SoSao { get; set; }

    public string? NoiDung { get; set; }

    public string? DanhSachAnh { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [ForeignKey("DonHangId")]
    [InverseProperty("DanhGia")]
    public virtual DonHang DonHang { get; set; } = null!;

    [ForeignKey("NguoiDungId")]
    [InverseProperty("DanhGia")]
    public virtual NguoiDung NguoiDung { get; set; } = null!;

    [ForeignKey("SanPhamId")]
    [InverseProperty("DanhGia")]
    public virtual SanPham SanPham { get; set; } = null!;
}
