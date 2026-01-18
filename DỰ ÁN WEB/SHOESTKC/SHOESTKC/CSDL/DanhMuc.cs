using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SHOESTKC.CSDL;

[Table("DanhMuc")]
[Index("Slug", Name = "UQ__DanhMuc__BC7B5FB69241B69C", IsUnique = true)]
public partial class DanhMuc
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string TenDanhMuc { get; set; } = null!;

    [StringLength(500)]
    public string? MoTa { get; set; }

    [StringLength(255)]
    public string Slug { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("DanhMuc")]
    public virtual ICollection<SanPham> SanPham { get; set; } = new List<SanPham>();
}
