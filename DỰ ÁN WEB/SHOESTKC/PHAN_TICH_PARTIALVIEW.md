# PHÂN TÍCH SỬ DỤNG PARTIALVIEW TRONG DỰ ÁN SHOESTKC

## 📋 TỔNG QUAN

Dự án **SHOESTKC** **CÓ SỬ DỤNG PartialView** trong ASP.NET MVC. PartialView được sử dụng để tái sử dụng code và tổ chức code tốt hơn.

---

## 🎯 CÁC PARTIALVIEW TRONG DỰ ÁN

### 1. **_ValidationScriptsPartial.cshtml**
- **Đường dẫn**: `Views/Shared/_ValidationScriptsPartial.cshtml`
- **Mục đích**: Chứa các script validation cho jQuery Validation
- **Loại**: Shared PartialView (dùng chung cho toàn bộ dự án)

---

## 📊 THỐNG KÊ SỬ DỤNG

### Tổng số lần sử dụng PartialView: **14 lần**

#### **Phương thức `@await Html.PartialAsync()`**: 7 lần
1. `Views/KhachHang/Create.cshtml` - dòng 65
2. `Views/Category/SuaDanhMuc.cshtml` - dòng 57
3. `Views/Category/ThemDanhMuc.cshtml` - dòng 54
4. `Views/Auth/Register.cshtml` - dòng 78
5. `Views/Admin/ThemMaKhuyenMai.cshtml` - dòng 102
6. `Views/Admin/ThemSanPham.cshtml` - dòng 139
7. `Views/Admin/SuaSanPham.cshtml` - dòng 170

#### **Phương thức `@{await Html.RenderPartialAsync()}`**: 7 lần
1. `Views/Size/Edit.cshtml` - dòng 105
2. `Views/Size/Create.cshtml` - dòng 101
3. `Views/SanPham/ThemSanPham.cshtml` - dòng 115
4. `Views/MauSac/Create.cshtml` - dòng 85
5. `Views/MauSac/Edit.cshtml` - dòng 103
6. `Views/KhachHang/Edit.cshtml` - dòng 86
7. `Views/Admin/SuaMaKhuyenMai.cshtml` - dòng 138

---

## 💡 VÍ DỤ CỤ THỂ

### **VÍ DỤ 1: Nội dung PartialView `_ValidationScriptsPartial.cshtml`**

```cshtml
<script src="~/lib/jquery-validation/dist/jquery.validate.min.js"></script>
<script src="~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"></script>
```

**Giải thích:**
- PartialView này chứa 2 thư viện JavaScript cho validation
- Được đặt trong thư mục `Views/Shared/` để dùng chung
- Tên bắt đầu bằng `_` (underscore) theo convention của ASP.NET MVC

---

### **VÍ DỤ 2: Sử dụng `@await Html.PartialAsync()` trong `Register.cshtml`**

**File**: `Views/Auth/Register.cshtml`

```cshtml
@model SHOESTKC.CSDL.NguoiDung

@{
    ViewData["Title"] = "Đăng ký tài khoản";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<div class="container d-flex justify-content-center align-items-center py-5">
    <div class="card shadow-lg" style="width: 100%; max-width: 550px;">
        <div class="card-body p-5">
            <div class="text-center mb-4">
                <h2 class="fw-bold">Đăng ký tài khoản</h2>
            </div>

            <form asp-action="Register" asp-controller="Auth" method="post">
                @Html.AntiForgeryToken()

                <div class="mb-3">
                    <label asp-for="HoTen" class="form-label">Họ và tên <span class="text-danger">*</span></label>
                    <input asp-for="HoTen" class="form-control" placeholder="VD: Nguyễn Văn A" required>
                    <span asp-validation-for="HoTen" class="text-danger"></span>
                </div>

                <div class="mb-3">
                    <label asp-for="Email" class="form-label">Email <span class="text-danger">*</span></label>
                    <input asp-for="Email" type="email" class="form-control" placeholder="VD: example@gmail.com" required>
                    <span asp-validation-for="Email" class="text-danger"></span>
                </div>

                <div class="mb-3">
                    <label asp-for="MatKhau" class="form-label">Mật khẩu <span class="text-danger">*</span></label>
                    <input asp-for="MatKhau" type="password" class="form-control" placeholder="Tối thiểu 6 ký tự" required>
                    <span asp-validation-for="MatKhau" class="text-danger"></span>
                </div>

                <button type="submit" class="btn btn-primary w-100 mb-3">
                    <i class="fas fa-user-plus"></i> Đăng ký
                </button>
            </form>
        </div>
    </div>
</div>

@section Scripts {
    @await Html.PartialAsync("_ValidationScriptsPartial")
}
```

**Giải thích:**
- Dòng 78: `@await Html.PartialAsync("_ValidationScriptsPartial")`
- Được đặt trong `@section Scripts` để load script validation
- Phương thức `PartialAsync()` trả về `IHtmlContent` và cần `await`
- Không cần chỉ định đường dẫn đầy đủ vì ASP.NET MVC tự động tìm trong `Views/Shared/`

---

### **VÍ DỤ 3: Sử dụng `@{await Html.RenderPartialAsync()}` trong `Create.cshtml`**

**File**: `Views/MauSac/Create.cshtml`

```cshtml
@model SHOESTKC.CSDL.MauSac

@{
    ViewData["Title"] = "Thêm Màu Sắc Mới";
    Layout = "~/Views/Shared/_AdminLayout.cshtml";
}

<div class="container-fluid">
    <div class="row mb-3">
        <div class="col-md-12">
            <h2><i class="fas fa-plus-circle"></i> Thêm Màu Sắc Mới</h2>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0">Thông Tin Màu Sắc</h5>
                </div>
                <div class="card-body">
                    <form asp-action="Create" method="post">
                        <div asp-validation-summary="ModelOnly" class="alert alert-danger"></div>
                        
                        <div class="mb-3">
                            <label for="TenMau" class="form-label">Tên Màu <span class="text-danger">*</span></label>
                            <input asp-for="TenMau" class="form-control" placeholder="VD: Đỏ, Xanh, Vàng..." />
                            <span asp-validation-for="TenMau" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            <label for="MaHex" class="form-label">Mã Màu Hex</label>
                            <input asp-for="MaHex" type="color" class="form-control form-control-color" value="#000000" />
                            <span asp-validation-for="MaHex" class="text-danger"></span>
                        </div>

                        <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                            <a asp-action="Index" class="btn btn-secondary">
                                <i class="fas fa-times"></i> Hủy
                            </a>
                            <button type="submit" class="btn btn-primary">
                                <i class="fas fa-save"></i> Lưu
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

**Giải thích:**
- Dòng 85: `@{await Html.RenderPartialAsync("_ValidationScriptsPartial");}`
- Được bao bọc trong `@{ }` vì `RenderPartialAsync()` trả về `void`
- Khác với `PartialAsync()`, phương thức này ghi trực tiếp vào output stream
- Hiệu suất tốt hơn một chút so với `PartialAsync()` nhưng ít linh hoạt hơn

---

### **VÍ DỤ 4: Sử dụng trong form thêm sản phẩm**

**File**: `Views/Admin/ThemSanPham.cshtml`

```cshtml
@model SHOESTKC.CSDL.SanPham

@{
    ViewData["Title"] = "Thêm Sản phẩm Mới";
    Layout = "~/Views/Shared/_AdminLayout.cshtml";
}

<div class="container-fluid">
    <form asp-action="ThemSanPham" asp-controller="Admin" method="post" enctype="multipart/form-data">
        @Html.AntiForgeryToken()

        <div class="row">
            <!-- Cột trái - Thông tin chính -->
            <div class="col-md-8">
                <div class="card mb-4">
                    <div class="card-header">
                        <h5><i class="fas fa-info-circle"></i> Thông tin cơ bản</h5>
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label asp-for="TenSanPham" class="form-label">Tên Sản phẩm <span class="text-danger">*</span></label>
                            <input asp-for="TenSanPham" class="form-control" placeholder="VD: Giày Nike Air Max 270" />
                            <span asp-validation-for="TenSanPham" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            <label asp-for="GiaGoc" class="form-label">Giá gốc (VNĐ) <span class="text-danger">*</span></label>
                            <input asp-for="GiaGoc" type="number" class="form-control" placeholder="VD: 1500000" step="1000" />
                            <span asp-validation-for="GiaGoc" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            <label asp-for="MoTa" class="form-label">Mô tả Sản phẩm</label>
                            <textarea asp-for="MoTa" class="form-control" rows="6" placeholder="Nhập mô tả chi tiết về Sản phẩm..."></textarea>
                            <span asp-validation-for="MoTa" class="text-danger"></span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Cột phải - Hình ảnh -->
            <div class="col-md-4">
                <div class="card">
                    <div class="card-header">
                        <h5><i class="fas fa-camera"></i> Hình ảnh Sản phẩm</h5>
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label class="form-label">Tải lên ảnh từ máy</label>
                            <input type="file" name="AnhChinh" class="form-control" accept="image/*" id="fileInput" />
                        </div>

                        <div class="mb-3">
                            <label asp-for="AnhChinh" class="form-label">Hoặc nhập URL ảnh</label>
                            <input asp-for="AnhChinh" class="form-control" placeholder="https://example.com/image.jpg" id="urlInput" />
                            <span asp-validation-for="AnhChinh" class="text-danger"></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="card mt-3">
            <div class="card-body">
                <button type="submit" class="btn btn-primary btn-lg">
                    <i class="fas fa-save"></i> Lưu Sản phẩm
                </button>
                <a asp-action="SanPham" asp-controller="Admin" class="btn btn-secondary btn-lg">
                    <i class="fas fa-times"></i> Hủy bỏ
                </a>
            </div>
        </div>
    </form>
</div>

@section Scripts {
    @await Html.PartialAsync("_ValidationScriptsPartial")

    <script>
        // Preview ảnh khi chọn file từ máy
        document.getElementById('fileInput').addEventListener('change', function() {
            const file = this.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    showImagePreview(e.target.result);
                };
                reader.readAsDataURL(file);
            }
        });

        // Format giá tiền
        document.querySelector('input[name="GiaGoc"]').addEventListener('blur', function() {
            if (this.value) {
                this.value = Math.round(this.value);
            }
        });
    </script>
}
```

**Giải thích:**
- Dòng 139: `@await Html.PartialAsync("_ValidationScriptsPartial")`
- PartialView được load trước các script tùy chỉnh khác
- Đảm bảo jQuery Validation được load trước khi sử dụng
- Các `<span asp-validation-for>` sẽ hoạt động nhờ script từ PartialView

---

## 🔍 SO SÁNH 2 PHƯƠNG THỨC

### **1. `@await Html.PartialAsync()`**

```cshtml
@section Scripts {
    @await Html.PartialAsync("_ValidationScriptsPartial")
}
```

**Đặc điểm:**
- ✅ Trả về `IHtmlContent`
- ✅ Có thể gán vào biến: `var content = await Html.PartialAsync("...")`
- ✅ Linh hoạt hơn, có thể xử lý kết quả trước khi render
- ✅ Cú pháp đơn giản, dễ đọc
- ⚠️ Hiệu suất thấp hơn một chút (do tạo object trung gian)

**Khi nào dùng:**
- Khi cần xử lý kết quả trước khi hiển thị
- Khi muốn code dễ đọc, dễ bảo trì
- Trong hầu hết các trường hợp thông thường

---

### **2. `@{await Html.RenderPartialAsync()}`**

```cshtml
@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

**Đặc điểm:**
- ✅ Trả về `void`
- ✅ Ghi trực tiếp vào output stream
- ✅ Hiệu suất tốt hơn một chút
- ⚠️ Phải bao bọc trong `@{ }` vì là statement
- ⚠️ Không thể gán vào biến hoặc xử lý kết quả

**Khi nào dùng:**
- Khi cần hiệu suất tối ưu (với PartialView lớn)
- Khi chỉ cần render đơn giản, không xử lý gì thêm
- Trong các trang có nhiều PartialView

---

## 📚 LỢI ÍCH CỦA PARTIALVIEW TRONG DỰ ÁN

### **1. Tái sử dụng code (Code Reusability)**
- Script validation được viết 1 lần, dùng ở 14 nơi
- Nếu cần cập nhật thư viện validation, chỉ sửa 1 file
- Giảm duplicate code, dễ bảo trì

### **2. Tổ chức code tốt hơn (Better Organization)**
- Tách biệt logic validation scripts ra file riêng
- Code chính (view) sạch hơn, dễ đọc hơn
- Tuân thủ nguyên tắc DRY (Don't Repeat Yourself)

### **3. Dễ bảo trì (Easy Maintenance)**
- Khi nâng cấp jQuery Validation, chỉ sửa 1 file
- Thêm/bớt script validation dễ dàng
- Không lo sót file nào khi cập nhật

### **4. Hiệu suất (Performance)**
- Browser có thể cache file script
- Giảm kích thước file view chính
- Load script chỉ khi cần (trong section Scripts)

---

## 🎓 CONVENTION VÀ BEST PRACTICES

### **1. Đặt tên PartialView**
```
✅ ĐÚNG: _ValidationScriptsPartial.cshtml
✅ ĐÚNG: _LoginPartial.cshtml
✅ ĐÚNG: _ProductCard.cshtml

❌ SAI: ValidationScripts.cshtml (thiếu underscore)
❌ SAI: Partial_Validation.cshtml (underscore sai vị trí)
```

**Quy tắc:**
- Bắt đầu bằng `_` (underscore)
- Tên rõ ràng, mô tả chức năng
- Suffix `Partial` (không bắt buộc nhưng nên có)

### **2. Vị trí lưu PartialView**

```
Views/
├── Shared/              ← PartialView dùng chung cho toàn dự án
│   ├── _Layout.cshtml
│   ├── _AdminLayout.cshtml
│   ├── _ValidationScriptsPartial.cshtml  ✅
│   └── Error.cshtml
├── Home/
│   ├── Index.cshtml
│   └── _HomeProductCard.cshtml  ← PartialView chỉ dùng cho Home
└── Admin/
    ├── SanPham.cshtml
    └── _ProductForm.cshtml  ← PartialView chỉ dùng cho Admin
```

### **3. Truyền Model vào PartialView**

```cshtml
<!-- Không truyền model (dùng ViewBag/ViewData) -->
@await Html.PartialAsync("_ValidationScriptsPartial")

<!-- Truyền model cụ thể -->
@await Html.PartialAsync("_ProductCard", Model.SanPham)

<!-- Truyền model với ViewData -->
@await Html.PartialAsync("_ProductCard", Model.SanPham, new ViewDataDictionary { { "ShowPrice", true } })
```

---

## 🚀 MỞ RỘNG: CÁC CÁCH SỬ DỤNG PARTIALVIEW KHÁC

### **1. Tạo PartialView cho Product Card**

**File**: `Views/Shared/_ProductCard.cshtml`
```cshtml
@model SHOESTKC.CSDL.SanPham

<div class="card product-card h-100">
    <img src="@Model.AnhChinh" class="card-img-top" alt="@Model.TenSanPham">
    <div class="card-body">
        <h5 class="card-title">@Model.TenSanPham</h5>
        <p class="card-text">@Model.Hang</p>
        <p class="text-danger fw-bold">@Model.GiaGoc.ToString("N0") VNĐ</p>
        <a asp-controller="Home" asp-action="ChiTiet" asp-route-id="@Model.SanPhamId" class="btn btn-primary">
            Xem chi tiết
        </a>
    </div>
</div>
```

**Sử dụng trong `Index.cshtml`:**
```cshtml
@model List<SHOESTKC.CSDL.SanPham>

<div class="row">
    @foreach (var product in Model)
    {
        <div class="col-md-3 mb-4">
            @await Html.PartialAsync("_ProductCard", product)
        </div>
    }
</div>
```

### **2. Tạo PartialView cho Pagination**

**File**: `Views/Shared/_Pagination.cshtml`
```cshtml
@model PaginationModel

@if (Model.TotalPages > 1)
{
    <nav aria-label="Page navigation">
        <ul class="pagination justify-content-center">
            <li class="page-item @(Model.CurrentPage == 1 ? "disabled" : "")">
                <a class="page-link" href="?page=@(Model.CurrentPage - 1)">Trước</a>
            </li>
            
            @for (int i = 1; i <= Model.TotalPages; i++)
            {
                <li class="page-item @(i == Model.CurrentPage ? "active" : "")">
                    <a class="page-link" href="?page=@i">@i</a>
                </li>
            }
            
            <li class="page-item @(Model.CurrentPage == Model.TotalPages ? "disabled" : "")">
                <a class="page-link" href="?page=@(Model.CurrentPage + 1)">Sau</a>
            </li>
        </ul>
    </nav>
}
```

**Sử dụng:**
```cshtml
@await Html.PartialAsync("_Pagination", new PaginationModel 
{ 
    CurrentPage = ViewBag.CurrentPage, 
    TotalPages = ViewBag.TotalPages 
})
```

### **3. Tạo PartialView cho Alert Messages**

**File**: `Views/Shared/_AlertMessages.cshtml`
```cshtml
@if (TempData["Success"] != null)
{
    <div class="alert alert-success alert-dismissible fade show" role="alert">
        <i class="fas fa-check-circle"></i> @TempData["Success"]
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>
}

@if (TempData["Error"] != null)
{
    <div class="alert alert-danger alert-dismissible fade show" role="alert">
        <i class="fas fa-exclamation-circle"></i> @TempData["Error"]
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>
}

@if (TempData["Warning"] != null)
{
    <div class="alert alert-warning alert-dismissible fade show" role="alert">
        <i class="fas fa-exclamation-triangle"></i> @TempData["Warning"]
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>
}
```

**Sử dụng:**
```cshtml
@await Html.PartialAsync("_AlertMessages")
```

---

## 📝 KẾT LUẬN

### **Tóm tắt:**
1. ✅ Dự án **CÓ SỬ DỤNG PartialView**
2. ✅ Có **1 PartialView** chính: `_ValidationScriptsPartial.cshtml`
3. ✅ Được sử dụng **14 lần** trong toàn dự án
4. ✅ Sử dụng 2 phương thức: `PartialAsync()` và `RenderPartialAsync()`
5. ✅ Mục đích: Tái sử dụng validation scripts

### **Lợi ích đạt được:**
- 🎯 Giảm duplicate code
- 🎯 Dễ bảo trì và cập nhật
- 🎯 Code sạch hơn, có tổ chức
- 🎯 Tuân thủ best practices của ASP.NET MVC

### **Khuyến nghị:**
- ✨ Có thể tạo thêm PartialView cho các component tái sử dụng (Product Card, Pagination, Alert Messages)
- ✨ Nên dùng `@await Html.PartialAsync()` cho code dễ đọc
- ✨ Chỉ dùng `@{await Html.RenderPartialAsync()}` khi cần tối ưu hiệu suất
- ✨ Luôn đặt tên PartialView bắt đầu bằng `_` (underscore)

---

**Ngày tạo**: 30/12/2025  
**Người phân tích**: Antigravity AI  
**Dự án**: SHOESTKC - Shoe E-commerce Website
