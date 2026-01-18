using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SHOESTKC.CSDL;

[Table("GioHang")]
public partial class GioHang
{
    [Key]
    public int Id { get; set; }

    public int NguoiDungId { get; set; }

    public int BienTheId { get; set; }

    public int SoLuong { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayThem { get; set; }

    [ForeignKey("BienTheId")]
    [InverseProperty("GioHang")]
    public virtual BienThe BienThe { get; set; } = null!;

    [ForeignKey("NguoiDungId")]
    [InverseProperty("GioHang")]
    public virtual NguoiDung NguoiDung { get; set; } = null!;
}
