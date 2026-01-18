using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SHOESTKC.CSDL;

[Table("SanPham")]
public partial class SanPham
{
    [Key]
    public int Id { get; set; }

    public int DanhMucId { get; set; }

    [StringLength(255)]
    public string TenSanPham { get; set; } = null!;

    [StringLength(100)]
    public string? Hang { get; set; }

    public string? MoTa { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal GiaGoc { get; set; }

    [StringLength(500)]
    public string? AnhChinh { get; set; }

    public string? DanhSachAnh { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayCapNhat { get; set; }

    [InverseProperty("SanPham")]
    public virtual ICollection<BienThe> BienThe { get; set; } = new List<BienThe>();

    [InverseProperty("SanPham")]
    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();

    [ForeignKey("DanhMucId")]
    [InverseProperty("SanPham")]
    public virtual DanhMuc DanhMuc { get; set; } = null!;
}
