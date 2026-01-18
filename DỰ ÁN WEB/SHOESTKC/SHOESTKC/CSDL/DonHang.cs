using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SHOESTKC.CSDL;

[Table("DonHang")]
[Index("MaDonHang", Name = "UQ__DonHang__129584AC88A93E26", IsUnique = true)]
public partial class DonHang
{
    [Key]
    public int Id { get; set; }

    public int NguoiDungId { get; set; }

    public int? MaKhuyenMaiId { get; set; }

    [StringLength(50)]
    public string MaDonHang { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TamTinh { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TienGiam { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PhiVanChuyen { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TongTien { get; set; }

    [StringLength(500)]
    public string DiaChiGiao { get; set; } = null!;

    [StringLength(20)]
    public string SoDienThoai { get; set; } = null!;

    [StringLength(50)]
    public string PhuongThucThanhToan { get; set; } = null!;

    [StringLength(20)]
    public string TrangThaiThanhToan { get; set; } = null!;

    [StringLength(20)]
    public string TrangThaiDonHang { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayCapNhat { get; set; }

    [InverseProperty("DonHang")]
    public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; } = new List<ChiTietDonHang>();

    [InverseProperty("DonHang")]
    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();

    [ForeignKey("MaKhuyenMaiId")]
    [InverseProperty("DonHang")]
    public virtual MaKhuyenMai? MaKhuyenMai { get; set; }

    [ForeignKey("NguoiDungId")]
    [InverseProperty("DonHang")]
    public virtual NguoiDung NguoiDung { get; set; } = null!;
}
