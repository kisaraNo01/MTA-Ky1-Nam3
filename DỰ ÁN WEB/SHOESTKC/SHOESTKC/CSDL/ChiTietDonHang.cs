using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SHOESTKC.CSDL;

[Table("ChiTietDonHang")]
public partial class ChiTietDonHang
{
    [Key]
    public int Id { get; set; }

    public int DonHangId { get; set; }

    public int BienTheId { get; set; }

    public int SoLuong { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DonGia { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ThanhTien { get; set; }

    [ForeignKey("BienTheId")]
    [InverseProperty("ChiTietDonHang")]
    public virtual BienThe BienThe { get; set; } = null!;

    [ForeignKey("DonHangId")]
    [InverseProperty("ChiTietDonHang")]
    public virtual DonHang DonHang { get; set; } = null!;
}
