using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SHOESTKC.CSDL;

[Table("MauSac")]
[Index("TenMau", Name = "UQ__MauSac__332F6A919FC4529C", IsUnique = true)]
public partial class MauSac
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string TenMau { get; set; } = null!;

    [StringLength(7)]
    public string? MaHex { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayCapNhat { get; set; }

    [InverseProperty("MauSacNavigation")]
    public virtual ICollection<BienThe> BienThe { get; set; } = new List<BienThe>();
}
