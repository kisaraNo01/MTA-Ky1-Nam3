-- ===================================
-- SCRIPT TẠO CSDL SHOP GIÀY - SQL SERVER (TỐI ƯU)
-- ===================================

-- Tạo Database
CREATE DATABASE ShopGiay;
GO

USE ShopGiay;
GO

-- ===================================
-- 1. BẢNG NguoiDung (Users)
-- ===================================
CREATE TABLE NguoiDung (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    MatKhau NVARCHAR(255) NOT NULL,
    HoTen NVARCHAR(255) NOT NULL,
    SoDienThoai NVARCHAR(20),
    DiaChi NVARCHAR(500),
    VaiTro NVARCHAR(20) NOT NULL CHECK (VaiTro IN ('khach_hang', 'admin')),
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE()
);
GO

-- ===================================
-- 2. BẢNG DanhMuc (Categories)
-- ===================================
CREATE TABLE DanhMuc (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc NVARCHAR(255) NOT NULL,
    MoTa NVARCHAR(500),
    Slug NVARCHAR(255) NOT NULL UNIQUE,
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- ===================================
-- 3. BẢNG SanPham (Products)
-- ===================================
CREATE TABLE SanPham (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DanhMucId INT NOT NULL,
    TenSanPham NVARCHAR(255) NOT NULL,
    Hang NVARCHAR(100),
    MoTa NVARCHAR(MAX),
    GiaGoc DECIMAL(18,2) NOT NULL,
    AnhChinh NVARCHAR(500),
    DanhSachAnh NVARCHAR(MAX) CHECK (ISJSON(DanhSachAnh) > 0 OR DanhSachAnh IS NULL),
    TrangThai BIT DEFAULT 1, -- 1: active, 0: inactive
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (DanhMucId) REFERENCES DanhMuc(Id)
);
GO

-- ===================================
-- 4. BẢNG BienThe (Product Variants)
-- ===================================
CREATE TABLE BienThe (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SanPhamId INT NOT NULL,
    Size NVARCHAR(10) NOT NULL,
    MauSac NVARCHAR(50) NOT NULL,
    MaSKU NVARCHAR(50) NOT NULL UNIQUE,
    SoLuongTon INT NOT NULL DEFAULT 0,
    ChenhLechGia DECIMAL(18,2) DEFAULT 0,
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SanPhamId) REFERENCES SanPham(Id)
);
GO

-- ===================================
-- 5. BẢNG GioHang (Cart Items)
-- ===================================
CREATE TABLE GioHang (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NguoiDungId INT NOT NULL,
    BienTheId INT NOT NULL,
    SoLuong INT NOT NULL DEFAULT 1,
    NgayThem DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (NguoiDungId) REFERENCES NguoiDung(Id),
    FOREIGN KEY (BienTheId) REFERENCES BienThe(Id)
);
GO

-- ===================================
-- 6. BẢNG MaKhuyenMai (Vouchers)
-- ===================================
CREATE TABLE MaKhuyenMai (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MaCode NVARCHAR(50) NOT NULL UNIQUE,
    LoaiKM NVARCHAR(20) NOT NULL CHECK (LoaiKM IN ('phan_tram', 'mien_phi_ship')),
    GiaTriGiam DECIMAL(18,2) NOT NULL,
    GiaTriDonToiThieu DECIMAL(18,2) DEFAULT 0,
    SoLanDungToiDa INT DEFAULT 100,
    SoLanDaDung INT DEFAULT 0,
    NgayBatDau DATETIME NOT NULL,
    NgayKetThuc DATETIME NOT NULL,
    TrangThai BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE(),
    CHECK (NgayKetThuc > NgayBatDau)
);
GO

-- ===================================
-- 7. BẢNG DonHang (Orders)
-- ===================================
CREATE TABLE DonHang (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NguoiDungId INT NOT NULL,
    MaKhuyenMaiId INT NULL,
    MaDonHang NVARCHAR(50) NOT NULL UNIQUE,
    TamTinh DECIMAL(18,2) NOT NULL,
    TienGiam DECIMAL(18,2) DEFAULT 0,
    PhiVanChuyen DECIMAL(18,2) DEFAULT 30000,
    TongTien DECIMAL(18,2) NOT NULL,
    DiaChiGiao NVARCHAR(500) NOT NULL,
    SoDienThoai NVARCHAR(20) NOT NULL,
    PhuongThucThanhToan NVARCHAR(50) NOT NULL CHECK (PhuongThucThanhToan IN ('COD', 'chuyen_khoan', 'the_tin_dung', 'vi_dien_tu')),
    TrangThaiThanhToan NVARCHAR(20) NOT NULL DEFAULT 'chua_thanh_toan' CHECK (TrangThaiThanhToan IN ('chua_thanh_toan', 'da_thanh_toan', 'that_bai')),
    TrangThaiDonHang NVARCHAR(20) NOT NULL DEFAULT 'cho_xac_nhan' CHECK (TrangThaiDonHang IN ('cho_xac_nhan', 'da_xac_nhan', 'dang_giao', 'da_giao', 'da_huy')),
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (NguoiDungId) REFERENCES NguoiDung(Id),
    FOREIGN KEY (MaKhuyenMaiId) REFERENCES MaKhuyenMai(Id)
);
GO

-- Trigger tự động cập nhật NgayCapNhat khi đơn hàng thay đổi
CREATE TRIGGER trg_UpdateDonHang
ON DonHang
AFTER UPDATE
AS
BEGIN
    UPDATE DonHang
    SET NgayCapNhat = GETDATE()
    WHERE Id IN (SELECT Id FROM inserted);
END;
GO

-- ===================================
-- 8. BẢNG ChiTietDonHang (Order Items)
-- ===================================
CREATE TABLE ChiTietDonHang (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DonHangId INT NOT NULL,
    BienTheId INT NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    ThanhTien DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (DonHangId) REFERENCES DonHang(Id),
    FOREIGN KEY (BienTheId) REFERENCES BienThe(Id)
);
GO

-- ===================================
-- 9. BẢNG DanhGia (Reviews)
-- ===================================
CREATE TABLE DanhGia (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NguoiDungId INT NOT NULL,
    SanPhamId INT NOT NULL,
    DonHangId INT NOT NULL,
    SoSao INT NOT NULL CHECK (SoSao BETWEEN 1 AND 5),
    NoiDung NVARCHAR(MAX),
    DanhSachAnh NVARCHAR(MAX) CHECK (ISJSON(DanhSachAnh) > 0 OR DanhSachAnh IS NULL),
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (NguoiDungId) REFERENCES NguoiDung(Id),
    FOREIGN KEY (SanPhamId) REFERENCES SanPham(Id),
    FOREIGN KEY (DonHangId) REFERENCES DonHang(Id)
);
GO

-- ===================================
-- THÔNG BÁO KẾT QUẢ
-- ===================================
PRINT N'✓ Tạo CSDL ShopGiay thành công!';
PRINT N'✓ Cấu trúc bảng và ràng buộc đã tối ưu theo chuẩn PascalCase!';
GO
