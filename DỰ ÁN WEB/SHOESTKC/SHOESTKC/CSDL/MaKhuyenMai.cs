using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SHOESTKC.CSDL;

[Table("MaKhuyenMai")]
[Index("MaCode", Name = "UQ__MaKhuyen__152C7C5CBB561BCB", IsUnique = true)]
public partial class MaKhuyenMai
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string MaCode { get; set; } = null!;

    [Column("LoaiKM")]
    [StringLength(20)]
    public string LoaiKm { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal GiaTriGiam { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? GiaTriDonToiThieu { get; set; }

    public int? SoLanDungToiDa { get; set; }

    public int? SoLanDaDung { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime NgayBatDau { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime NgayKetThuc { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaKhuyenMai")]
    public virtual ICollection<DonHang> DonHang { get; set; } = new List<DonHang>();
}
