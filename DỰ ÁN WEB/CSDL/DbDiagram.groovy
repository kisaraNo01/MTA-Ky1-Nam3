Project CSDLShoesShop {
  database_type: 'SQL Server'
  Note: 'Sơ đồ cơ sở dữ liệu cho hệ thống bán giày trực tuyến. Sử dụng PascalCase cho tên bảng và cột.'
}

// Bảng: Người Dùng
Table NguoiDung {
  Id INT [pk, increment, note: 'Khóa chính, tự tăng']
  Email NVARCHAR(255) [unique, not null]
  MatKhau NVARCHAR(255) [not null]
  HoTen NVARCHAR(255)
  SoDienThoai NVARCHAR(20)
  DiaChi NVARCHAR(500)
  VaiTro NVARCHAR(20) [note: 'Admin, KhachHang']
  NgayTao DATETIME
  NgayCapNhat DATETIME
}

// Bảng: Danh Mục
Table DanhMuc {
  Id INT [pk, increment]
  TenDanhMuc NVARCHAR(255) [not null]
  MoTa NVARCHAR(500)
  Slug NVARCHAR(255) [unique, not null]
  NgayTao DATETIME
}

// Bảng: Sản Phẩm
Table SanPham {
  Id INT [pk, increment]
  DanhMucId INT [not null]
  TenSanPham NVARCHAR(255) [not null]
  Hang NVARCHAR(100)
  MoTa NVARCHAR(MAX)
  GiaGoc DECIMAL(18,2) [note: 'Giá niêm yết/Gốc']
  AnhChinh NVARCHAR(500)
  DanhSachAnh NVARCHAR(MAX) [note: 'Lưu dạng JSON hoặc chuỗi phân tách']
  TrangThai BIT [note: '1: Còn bán, 0: Ngừng bán']
  NgayTao DATETIME
  NgayCapNhat DATETIME
}

// Bảng: Biến Thể (Size/Màu)
Table BienThe {
  Id INT [pk, increment]
  SanPhamId INT [not null]
  Size NVARCHAR(10) [not null]
  MauSac NVARCHAR(50) [not null]
  MaSku NVARCHAR(50) [unique, not null]
  SoLuongTon INT [not null]
  ChenhLechGia DECIMAL(18,2) [note: 'Giá biến thể chênh lệch so với GiaGoc của SanPham']
  NgayTao DATETIME
}

// Bảng: Giỏ Hàng (Thường là bảng tạm/phiên)
Table GioHang {
  Id INT [pk, increment]
  NguoiDungId INT [not null]
  BienTheId INT [not null]
  SoLuong INT [not null]
  NgayThem DATETIME
  // Unique Constraint (NguoiDungId, BienTheId)
}

// Bảng: Mã Khuyến Mãi (Voucher)
Table MaKhuyenMai {
  Id INT [pk, increment]
  MaCode NVARCHAR(50) [unique, not null]
  LoaiKm NVARCHAR(20) [note: 'PhanTram, TienMat']
  GiaTriGiam DECIMAL(18,2) [not null]
  GiaTriDonToiThieu DECIMAL(18,2)
  SoLanDungToiDa INT
  SoLanDaDung INT
  NgayBatDau DATETIME
  NgayKetThuc DATETIME
  TrangThai BIT [note: '1: Hoạt động, 0: Ngừng hoạt động']
  NgayTao DATETIME
}

// Bảng: Đơn Hàng
Table DonHang {
  Id INT [pk, increment]
  NguoiDungId INT [not null]
  MaKhuyenMaiId INT []
  MaDonHang NVARCHAR(50) [unique, not null]
  TamTinh DECIMAL(18,2) [note: 'Tổng tiền trước giảm giá và phí ship']
  TienGiam DECIMAL(18,2)
  PhiVanChuyen DECIMAL(18,2)
  TongTien DECIMAL(18,2) [not null]
  DiaChiGiao NVARCHAR(500)
  SoDienThoai NVARCHAR(20)
  PhuongThucThanhToan NVARCHAR(50)
  TrangThaiThanhToan NVARCHAR(20) [note: 'ChuaThanhToan, DaThanhToan']
  TrangThaiDonHang NVARCHAR(20) [note: 'ChoXacNhan, DangGiao, DaGiao, Huy']
  NgayTao DATETIME
  NgayCapNhat DATETIME
}

// Bảng: Chi Tiết Đơn Hàng
Table ChiTietDonHang {
  Id INT [pk, increment]
  DonHangId INT [not null]
  BienTheId INT [not null]
  SoLuong INT [not null]
  DonGia DECIMAL(18,2) [note: 'Giá bán tại thời điểm đặt hàng (GiaGoc + ChenhLechGia)']
  ThanhTien DECIMAL(18,2) [note: 'SoLuong * DonGia']
}

// Bảng: Đánh Giá
Table DanhGia {
  Id INT [pk, increment]
  NguoiDungId INT [not null]
  SanPhamId INT [not null]
  DonHangId INT [not null]
  SoSao INT [note: 'Từ 1 đến 5']
  NoiDung NVARCHAR(MAX)
  DanhSachAnh NVARCHAR(MAX)
  NgayTao DATETIME
  // Unique Constraint (NguoiDungId, DonHangId) - Để đảm bảo 1 người chỉ đánh giá 1 lần cho 1 đơn hàng/sản phẩm cụ thể.
}

// ----------------------------------------------------------------------------------------------------
// ĐỊNH NGHĨA MỐI QUAN HỆ (RELATIONSHIPS)
// ----------------------------------------------------------------------------------------------------

// 1. DanhMuc - SanPham (1-N)
Ref: DanhMuc.Id < SanPham.DanhMucId

// 2. SanPham - BienThe (1-N)
Ref: SanPham.Id < BienThe.SanPhamId

// 3. NguoiDung - GioHang (1-N)
Ref: NguoiDung.Id < GioHang.NguoiDungId

// 4. BienThe - GioHang (1-N)
Ref: BienThe.Id < GioHang.BienTheId

// 5. NguoiDung - DonHang (1-N)
Ref: NguoiDung.Id < DonHang.NguoiDungId

// 6. MaKhuyenMai - DonHang (1-N)
Ref: MaKhuyenMai.Id < DonHang.MaKhuyenMaiId

// 7. DonHang - ChiTietDonHang (1-N)
Ref: DonHang.Id < ChiTietDonHang.DonHangId

// 8. BienThe - ChiTietDonHang (1-N)
Ref: BienThe.Id < ChiTietDonHang.BienTheId

// 9. NguoiDung - DanhGia (1-N)
Ref: NguoiDung.Id < DanhGia.NguoiDungId

// 10. SanPham - DanhGia (1-N)
Ref: SanPham.Id < DanhGia.SanPhamId

// 11. DonHang - DanhGia (1-N)
Ref: DonHang.Id < DanhGia.DonHangId