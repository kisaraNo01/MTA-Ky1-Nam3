# TỔNG QUAN DỰ ÁN SHOESTKC - CƠ CHẾ HOẠT ĐỘNG MVC
*Tài liệu giải thích luồng đi của dữ liệu và cách các file code "nói chuyện" với nhau.*

---

## 1. Mô hình MVC trong dự án này là gì?

Dự án **SHOESTKC** được xây dựng trên công nghệ **ASP.NET Core MVC**. Hãy tưởng tượng đây là một "nhà hàng":

*   **Models (Kho nguyên liệu - CSDL):** Nằm trong thư mục `CSDL` và `Data`. Chứa các class đại diện cho dữ liệu (như `SanPham`, `DonHang`, `NguoiDung`) và `CSDLShoesShopDbContext` (người quản kho).
*   **Views (Trang trí/Bày biện - Giao diện):** Nằm trong thư mục `Views`. Là các file `.cshtml` chịu trách nhiệm hiển thị hình ảnh, nút bấm cho người dùng xem.
*   **Controllers (Bếp trưởng - Xử lý):** Nằm trong thư mục `Controllers`. Nhận yêu cầu từ khách (User), sai bảo Model lấy dữ liệu, chế biến, rồi ném sang View để hiển thị.

---

## 2. Luồng chạy của một yêu cầu (Request Lifecycle)

Khi thầy hỏi: *"Code nó chạy từ đâu đến đâu?"*, hãy trả lời theo quy trình 4 bước này:

### Bước 1: Khởi động & Định tuyến (Routing)
File đầu tiên chạy là `Program.cs`. Tại đây, bạn cấu hình "bản đồ" (Routing).
```csharp
// Program.cs
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```
*Ý nghĩa:* Khi người dùng vào `shoestkc.com/SanPham/Details/5`:
- **Controller** = SanPham
- **Action** = Details
- **Id** = 5

### Bước 2: Controller nhận lệnh
Hệ thống tìm file `Controllers/SanPhamController.cs` và chạy hàm `Details(int id)`.
Tại đây, Controller sẽ cần dữ liệu. Nó không tự có dữ liệu mà phải nhờ **Dependency Injection** (Cơ chế tiêm phụ thuộc).
```csharp
// SanPhamController.cs
public class SanPhamController : Controller {
    private readonly CSDLShoesShopDbContext _context; // Biến giữ kết nối DB
    
    // Constructor này được gọi tự động khi Controller khởi tạo
    public SanPhamController(CSDLShoesShopDbContext context) {
        _context = context; // Nhận "chìa khóa kho"
    }

    public async Task<IActionResult> Details(int id) {
        // ... code xử lý
    }
}
```

### Bước 3: Tương tác Model (Truy vấn DB)
Trong hàm `Details`, Controller dùng `_context` để lấy dữ liệu.
```csharp
// SanPhamController.cs
var sanPham = await _context.SanPham
    .Include(sp => sp.DanhMuc) // Kèm theo thông tin Danh mục (Join bảng)
    .FirstOrDefaultAsync(m => m.Id == id); // Tìm đúng ID
```
*Bản chất:* Dòng này dịch sang SQL gửi xuống Database, lấy kết quả đổ vào object `sanPham`.

### Bước 4: Trả về View
Sau khi có dữ liệu (món ăn đã nấu xong), Controller tống nó sang View.
```csharp
return View(sanPham); // Truyền model sanPham sang View
```
Hệ thống sẽ tự tìm file theo quy tắc: `Views/[TenController]/[TenAction].cshtml` -> `Views/SanPham/Details.cshtml` (hoặc đường dẫn được chỉ định cụ thể).

---

## 3. Bản chất liên kết giữa các file

### A. Liên kết giữa Controller và View
- **Dữ liệu:** Controller truyền dữ liệu sang View qua 2 cách chính:
    1.  `return View(model)` -> Bên View dùng `@model ...` để hứng.
    2.  `ViewBag.TenBien = ...` -> Bên View dùng `@ViewBag.TenBien` để lấy.
- **Tên file:** Mặc định tên hàm trong Controller trùng tên file View.

### B. Liên kết giữa Các View (Layout & Partial)
File `_Layout.cshtml` giống như cái khung tranh.
- Các View con (Index, Details...) chỉ chứa nội dung ở giữa.
- Khi chạy, nội dung View con được nhét vào chỗ `@RenderBody()` trong Layout.
- **Code:**
  - View con: `Layout = "~/Views/Shared/_Layout.cshtml";`
  - Layout: `<html>... @RenderBody() ...</html>`

### C. Liên kết Client (Trình duyệt) và Server (Controller)
- **Link (<a>):** Khi bấm `<a asp-controller="GioHang" asp-action="Index">`, trình duyệt gửi **GET** request.
- **Form (<form>):** Khi bấm Submit, trình duyệt gửi **POST** request kèm dữ liệu nhập.
- **Ajax (JS):** Trong file JS (`site.js` hoặc script inline), dùng `fetch()` hoặc `$.ajax()` để gọi ngầm Controller (ví dụ nút "Thêm vào giỏ hàng").
  ```javascript
  // Javascript gọi về Server
  fetch('/GioHang/Add?bienTheId=' + id)
  ```
  -> Controller xử lý xong trả về **JSON** (`return Json(...)`) thay vì HTML.

---

## 4. Các logic đặc thù trong dự án SHOESTKC

### 1. Quản lý Đăng nhập (Session)
Dự án này không dùng Identity phức tạp mà dùng **Session**.
- **Login:** Kiểm tra DB -> Đúng thì `HttpContext.Session.SetInt32("UserId", user.Id)`.
- **Check Login:** Kiểm tra `HttpContext.Session.GetInt32("UserId") != null`.
- **Logout:** `HttpContext.Session.Clear()`.
*Các file liên quan:* `AuthController.cs`, `_Layout.cshtml` (để hiện tên user trên menu).

### 2. Giỏ hàng (Logic lai)
- **Chưa đăng nhập:** (Nếu bạn chưa làm tính năng lưu Session cho giỏ hàng khách vãng lai) -> Thường yêu cầu đăng nhập.
- **Đã đăng nhập:** Dữ liệu lưu thẳng vào bảng `GioHang` trong SQL.
*File liên quan:* `GioHangController.cs` (Hàm `Add`, `Index`).

### 3. Phân quyền Admin
Dùng Middleware hoặc kiểm tra thủ công trong Controller/View.
```csharp
// Trong View _Layout.cshtml
@if (Context.Session.GetString("UserRole") == "admin") {
    // Hiện menu quản lý
}
```

---

## 5. Tóm tắt "Bản chất" để trả lời Vấn đáp
1.  **Code nó gọi nhau ntn?**: Mọi thứ bắt đầu từ **URL** -> **Program** điều hướng -> **Controller** lấy dữ liệu -> **View** hiển thị.
2.  **Tại sao phải chia Controller/View/Model?**: Để tách biệt trách nhiệm. Ông lấy dữ liệu (Model) không quan tâm việc hiển thị. Ông hiển thị (View) không cần biết logic xử lý. Dễ sửa code.
3.  **Dữ liệu đi như nào?**: DB -> DbContext -> Controller -> View -> Trình duyệt người dùng.

---
*File này giúp bạn có cái nhìn toàn cảnh (Helicopter View) về hệ thống.*
