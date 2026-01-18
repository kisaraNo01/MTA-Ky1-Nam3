# 📚 HƯỚNG DẪN MỞ RỘNG MENU VÀ CHỨC NĂNG - DỰ ÁN QUẢN LÝ SÁCH

## 🎯 MỤC ĐÍCH
Tài liệu này hướng dẫn chi tiết cách:
1. Thêm menu mới vào menu chính
2. Tạo chức năng mới
3. Liên kết chức năng với menu
4. Return về menu chính đúng cách

---

## 📋 CẤU TRÚC DỰ ÁN

### **File quan trọng:**
- `UI.h` - Xử lý logic menu chính
- `GiaoDien.h` - Vẽ menu và giao diện
- `ThiCuoiKy.h` - Ví dụ menu mở rộng (mới thêm)

### **Luồng hoạt động:**
```
main.cpp
  → RunApp() (UI.h)
      → SetMenu() (GiaoDien.h) - Hiển thị menu
      → switch (choice) - Xử lý lựa chọn
          → Gọi chức năng tương ứng
          → Return về menu chính
```

---

## 🔧 BƯỚC 1: THÊM MENU MỚI VÀO MENU CHÍNH

### **1.1. Sửa file `GiaoDien.h`**

**Vị trí:** Hàm `SetMenu()` (dòng ~404)

**Bước 1:** Tăng số lượng menu item
```cpp
// TRƯỚC (6 mục):
string menu[6] = {
    "1. Them moi ho so",
    "2. In danh sach",
    "3. Sap xep",
    "4. Tim kiem",
    "5. Thong Ke",
    "6. Thoat"
};

// SAU (7 mục - thêm "Thi Cuoi Ky"):
string menu[7] = {
    "1. Them moi ho so",
    "2. In danh sach",
    "3. Sap xep",
    "4. Tim kiem",
    "5. Thong Ke",
    "6. Thi Cuoi Ky",    // ← MỚI THÊM
    "7. Thoat"           // ← Đổi số thứ tự
};
```

**Bước 2:** Cập nhật vòng lặp vẽ menu
```cpp
// TRƯỚC:
for (int i = 0; i < 6; i++) {
    VeOMenuCanTrai(x, y + i * spacing, w, h, ...);
}

// SAU:
for (int i = 0; i < 7; i++) {  // ← Đổi 6 thành 7
    VeOMenuCanTrai(x, y + i * spacing, w, h, ...);
}
```

**Bước 3:** Cập nhật breadcrumb
```cpp
// TRƯỚC:
if (current < 5) {
    CapNhatBreadcrumb(menu[current]);
}

// SAU:
if (current < 6) {  // ← Đổi 5 thành 6
    CapNhatBreadcrumb(menu[current]);
}
```

**Bước 4:** Cập nhật phím ESC
```cpp
// TRƯỚC:
else if (c == 27) { // ESC
    return 6;
}

// SAU:
else if (c == 27) { // ESC
    return 7;  // ← Đổi 6 thành 7
}
```

---

## 🔧 BƯỚC 2: TẠO FILE CHỨC NĂNG MỚI

### **2.1. Tạo file header mới (VD: `ThiCuoiKy.h`)**

**Cấu trúc file:**
```cpp
#ifndef THICUOIKY_H
#define THICUOIKY_H

#include "Sach.h"
#include "GiaoDien.h"
#include "HienThiMoi.h"

using namespace std;

// ============================================
// HÀM CHỨC NĂNG 1
// ============================================
void ChucNang1(DanhSachSach &dss) {
    // Code chức năng
    
    // Hiển thị kết quả
    system("cls");
    VeKhungChinh();
    
    // ... code xử lý ...
    
    // Chờ người dùng nhấn phím
    SetColor(11);
    gotoXY(35, 25);
    cout << "Nhan phim bat ky de quay lai...";
    _getch();
    
    // Tự động return về menu
}

// ============================================
// HÀM CHỨC NĂNG 2
// ============================================
void ChucNang2(DanhSachSach &dss) {
    // Tương tự
}

// ============================================
// MENU CON (SUB-MENU)
// ============================================
void MenuCon(DanhSachSach &dss) {
    while (true) {
        string menu[] = {
            "1. Chuc nang 1",
            "2. Chuc nang 2",
            "3. Chuc nang 3",
            "4. Quay lai"
        };
        
        int choice = MenuConChung("===== MENU CON =====", menu, 4, "Chuc nang");
        
        switch (choice) {
            case 1:
                ChucNang1(dss);
                break;
            case 2:
                ChucNang2(dss);
                break;
            case 3:
                ChucNang3(dss);
                break;
            case 4:
                return;  // ← RETURN VỀ MENU CHÍNH
        }
    }
}

#endif
```

---

## 🔧 BƯỚC 3: LIÊN KẾT CHỨC NĂNG VỚI MENU CHÍNH

### **3.1. Include file mới vào `UI.h`**

**Vị trí:** Đầu file `UI.h` (dòng ~1-10)

```cpp
#include "Sach.h"
#include "GiaoDien.h"
#include "InitData.h"
#include "TimKiem.h"
#include "ThongKe.h"
#include "ThiCuoiKy.h"  // ← THÊM DÒNG NÀY
```

### **3.2. Thêm case xử lý trong `RunApp()`**

**Vị trí:** Hàm `RunApp()` trong `UI.h` (dòng ~15-100)

```cpp
void RunApp() {
    // ... code khởi tạo ...
    
    while (true) {
        int choice = SetMenu();
        
        switch (choice) {
            case 1: // Them moi ho so
                NhapSachTrucTiep(dss);
                break;
            
            case 2: // In danh sach
                // ...
                break;
            
            case 3: // Sap xep
                // ...
                break;
            
            case 4: // Tim kiem
                MenuTimKiemChinh(dss);
                break;
            
            case 5: // Thong ke
                // ...
                break;
            
            case 6: // Thi Cuoi Ky ← MỚI THÊM
            {
                MenuThiCuoiKy(dss);  // ← GỌI HÀM MENU CON
                break;
            }
            
            case 7: // Thoat (đổi từ case 6)
                exit(0);
        }
    }
}
```

---

## 🎯 CÁCH RETURN VỀ MENU CHÍNH

### **Phương pháp 1: Dùng `return` trong vòng lặp**

```cpp
void MenuCon(DanhSachSach &dss) {
    while (true) {
        // Hiển thị menu
        int choice = MenuConChung(...);
        
        switch (choice) {
            case 1:
                ChucNang1(dss);
                break;  // ← Quay lại vòng lặp while
            
            case 2:
                return;  // ← THOÁT KHỎI HÀM, VỀ MENU CHÍNH
        }
    }
}
```

**Giải thích:**
- `break`: Thoát khỏi `switch`, quay lại vòng lặp `while` (vẫn ở menu con)
- `return`: Thoát khỏi hàm, quay về nơi gọi hàm (menu chính)

### **Phương pháp 2: Dùng biến cờ**

```cpp
void MenuCon(DanhSachSach &dss) {
    bool running = true;
    
    while (running) {
        int choice = MenuConChung(...);
        
        switch (choice) {
            case 1:
                ChucNang1(dss);
                break;
            
            case 2:
                running = false;  // ← Đặt cờ = false
                break;
        }
    }
    // Tự động return khi thoát vòng lặp
}
```

---

## 📝 VÍ DỤ CỤ THỂ: MENU "THI CUỐI KỲ"

### **Cấu trúc:**
```
Menu Chính (UI.h)
  ↓
6. Thi Cuoi Ky
  ↓
MenuThiCuoiKy() (ThiCuoiKy.h)
  ↓
  1. Sap xep giam theo gia sach → SapXepGiamTheoGia()
  2. Sap xep theo the loai (Merge Sort) → ThucHienMergeSortTheoTheLoai()
  3. Tim kiem theo khoang gia sach → TimKiemTheoKhoangGia()
  4. Thong ke theo gia sach (Bang) → ThongKeTheoGia_Bang()
  5. Bieu do theo gia sach → BieuDoTheoGia()
  6. Quay lai → return về Menu Chính
```

### **Code:**
```cpp
void MenuThiCuoiKy(DanhSachSach &dss) {
    while (true) {
        string menu[] = {
            "1. Sap xep giam theo gia sach",
            "2. Sap xep theo the loai (Merge Sort)",
            "3. Tim kiem theo khoang gia sach",
            "4. Thong ke theo gia sach (Bang)",
            "5. Bieu do theo gia sach",
            "6. Quay lai"
        };
        
        int choice = MenuConChung("===== MENU THI CUOI KY =====", menu, 6, "Chuc nang");
        
        switch (choice) {
            case 1:
                SapXepGiamTheoGia(dss);
                break;
            case 2:
                ThucHienMergeSortTheoTheLoai(dss);
                break;
            case 3:
                TimKiemTheoKhoangGia(dss);
                break;
            case 4:
                ThongKeTheoGia_Bang(dss);
                break;
            case 5:
                BieuDoTheoGia(dss);
                break;
            case 6:
                return;  // ← VỀ MENU CHÍNH
        }
    }
}
```

---

## 🔍 CHECKLIST KHI THÊM MENU MỚI

### **1. File `GiaoDien.h`:**
- [ ] Thêm menu item vào mảng `menu[]`
- [ ] Tăng số lượng trong vòng lặp `for`
- [ ] Cập nhật điều kiện breadcrumb
- [ ] Cập nhật giá trị return của phím ESC

### **2. File `UI.h`:**
- [ ] Include file chức năng mới
- [ ] Thêm case xử lý trong `switch`
- [ ] Cập nhật số case "Thoat"

### **3. File chức năng mới (VD: `ThiCuoiKy.h`):**
- [ ] Có `#ifndef` và `#define` guard
- [ ] Include các file cần thiết
- [ ] Có hàm menu con với vòng lặp `while(true)`
- [ ] Có case "Quay lai" với `return`
- [ ] Mỗi chức năng có `_getch()` trước khi return

---

## 🎨 MẪU CODE CHUẨN

### **Hàm chức năng đơn:**
```cpp
void ChucNangMoi(DanhSachSach &dss) {
    // 1. Xóa màn hình
    system("cls");
    VeKhungChinh();
    
    // 2. Hiển thị tiêu đề
    SetColor(14);
    gotoXY(40, 3);
    cout << "TIEU DE CHUC NANG";
    
    // 3. Xử lý logic
    vector<Sach> ds = dss.GetList();
    // ... code xử lý ...
    
    // 4. Hiển thị kết quả
    // ... code hiển thị ...
    
    // 5. Chờ người dùng
    SetColor(11);
    gotoXY(35, 25);
    cout << "Nhan phim bat ky de quay lai...";
    _getch();
    
    // 6. Tự động return về menu
}
```

### **Hàm menu con:**
```cpp
void MenuCon(DanhSachSach &dss) {
    while (true) {
        // 1. Định nghĩa menu
        string menu[] = {
            "1. Chuc nang 1",
            "2. Chuc nang 2",
            "3. Quay lai"
        };
        
        // 2. Hiển thị và nhận lựa chọn
        int choice = MenuConChung("===== TIEU DE MENU =====", menu, 3, "Chuc nang");
        
        // 3. Xử lý lựa chọn
        switch (choice) {
            case 1:
                ChucNang1(dss);
                break;
            case 2:
                ChucNang2(dss);
                break;
            case 3:
                return;  // VỀ MENU CHÍNH
        }
    }
}
```

---

## ⚠️ LƯU Ý QUAN TRỌNG

### **1. Về số thứ tự menu:**
- Khi thêm menu mới, **TẤT CẢ** các menu sau phải đổi số
- Nhớ cập nhật cả trong `GiaoDien.h` VÀ `UI.h`

### **2. Về return:**
- Dùng `return` để về menu cha
- Dùng `break` để ở lại menu hiện tại
- **KHÔNG** dùng `exit(0)` trong menu con (chỉ dùng ở menu chính)

### **3. Về _getch():**
- Mỗi chức năng phải có `_getch()` trước khi kết thúc
- Nếu không, màn hình sẽ biến mất ngay lập tức

### **4. Về tham số:**
- Luôn truyền `DanhSachSach &dss` (tham chiếu)
- Nếu không cần sửa dữ liệu, dùng `const DanhSachSach &dss`

---

## 🚀 BƯỚC TIẾP THEO

### **Sau khi thêm menu mới:**
1. **Compile:** Ctrl+F11 (Rebuild All)
2. **Chạy thử:** F9
3. **Kiểm tra:**
   - Menu hiển thị đúng
   - Chức năng hoạt động
   - Return về đúng menu
   - Không bị crash

### **Debug nếu lỗi:**
- Lỗi compile: Kiểm tra `#include` và tên hàm
- Menu không hiển thị: Kiểm tra số lượng trong vòng lặp
- Không return được: Kiểm tra `return` trong case "Quay lai"
- Crash: Kiểm tra con trỏ và tham chiếu

---

## 📚 TÀI LIỆU THAM KHẢO

### **File mẫu:**
- `ThiCuoiKy.h` - Menu con hoàn chỉnh
- `TimKiem.h` - Menu con phức tạp với nhiều cấp
- `ThongKe.h` - Các hàm chức năng đơn

### **Hàm hỗ trợ:**
- `MenuConChung()` - Tạo menu con (GiaoDien.h)
- `VeKhungChinh()` - Vẽ khung chính (GiaoDien.h)
- `HienThiDanhSach_New()` - Hiển thị danh sách (HienThiMoi.h)

---

**CHÚC BẠN THÀNH CÔNG!** 🎉
