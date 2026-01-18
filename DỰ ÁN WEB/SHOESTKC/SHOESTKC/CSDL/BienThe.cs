using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SHOESTKC.CSDL;

[Table("BienThe")]
[Index("MaSku", Name = "UQ__BienThe__318FCC49C203367B", IsUnique = true)]
public partial class BienThe
{
    [Key]
    public int Id { get; set; }

    public int SanPhamId { get; set; }

    [StringLength(10)]
    public string Size { get; set; } = null!;

    [StringLength(50)]
    public string MauSac { get; set; } = null!;

    [Column("MaSKU")]
    [StringLength(50)]
    public string MaSku { get; set; } = null!;

    public int SoLuongTon { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ChenhLechGia { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    public int? SizeId { get; set; }

    public int? MauSacId { get; set; }

    [InverseProperty("BienThe")]
    public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; } = new List<ChiTietDonHang>();

    [InverseProperty("BienThe")]
    public virtual ICollection<GioHang> GioHang { get; set; } = new List<GioHang>();

    [ForeignKey("MauSacId")]
    [InverseProperty("BienThe")]
    public virtual MauSac? MauSacNavigation { get; set; }

    [ForeignKey("SanPhamId")]
    [InverseProperty("BienThe")]
    public virtual SanPham SanPham { get; set; } = null!;

    [ForeignKey("SizeId")]
    [InverseProperty("BienThe")]
    public virtual Size? SizeNavigation { get; set; }
}
