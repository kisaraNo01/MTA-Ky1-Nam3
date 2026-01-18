using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SHOESTKC.CSDL;

[Table("Size")]
[Index("TenSize", Name = "UQ__Size__C86AACB9D66D91A2", IsUnique = true)]
public partial class Size
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    public string TenSize { get; set; } = null!;

    public int? ThuTu { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayCapNhat { get; set; }

    [InverseProperty("SizeNavigation")]
    public virtual ICollection<BienThe> BienThe { get; set; } = new List<BienThe>();
}
