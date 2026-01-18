#ifndef THICUOIKY_H
#define THICUOIKY_H

#include "Sach.h"
#include "GiaoDien.h"
#include "HienThiMoi.h"

using namespace std;

// ============================================
// YEU CAU 1: MERGE SORT THEO THE LOAI GIAM DAN (1 DIEM)
// ============================================

// Ham Merge cho Merge Sort (GIAM DAN)
void MergeTheoTheLoaiGiam(vector<Sach> &arr, int left, int mid, int right) {
    int n1 = mid - left + 1;
    int n2 = right - mid;
    
    // Tao mang tam
    vector<Sach> L(n1), R(n2);
    
    for (int i = 0; i < n1; i++)
        L[i] = arr[left + i];
    for (int j = 0; j < n2; j++)
        R[j] = arr[mid + 1 + j];
    
    // Merge 2 mang (GIAM DAN)
    int i = 0, j = 0, k = left;
    while (i < n1 && j < n2) {
        // So sanh theo Ma The Loai (GIAM DAN)
        if (L[i].GetMaTheLoai() >= R[j].GetMaTheLoai()) {
            arr[k] = L[i];
            i++;
        } else {
            arr[k] = R[j];
            j++;
        }
        k++;
    }
    
    // Copy phan con lai
    while (i < n1) {
        arr[k] = L[i];
        i++;
        k++;
    }
    
    while (j < n2) {
        arr[k] = R[j];
        j++;
        k++;
    }
}

// Ham Merge Sort de quy (GIAM DAN)
void MergeSortTheoTheLoaiGiam(vector<Sach> &arr, int left, int right) {
    if (left < right) {
        int mid = left + (right - left) / 2;
        
        MergeSortTheoTheLoaiGiam(arr, left, mid);
        MergeSortTheoTheLoaiGiam(arr, mid + 1, right);
        
        MergeTheoTheLoaiGiam(arr, left, mid, right);
    }
}

// Ham thuc hien va hien thi Merge Sort GIAM DAN
void ThucHienMergeSortTheoTheLoaiGiam(DanhSachSach &dss) {
    vector<Sach> ds = dss.GetList();
    
    if (ds.empty()) {
        system("cls");
        VeKhungChinh();
        SetColor(12);
        gotoXY(40, 12);
        cout << "Danh sach rong!";
        SetColor(15);
        _getch();
        return;
    }
    
    // Thuc hien Merge Sort GIAM DAN
    MergeSortTheoTheLoaiGiam(ds, 0, ds.size() - 1);
    
    // Hien thi ket qua
    system("cls");
    VeKhungChinh();
    SetColor(14);
    gotoXY(25, 2);
    cout << "SAP XEP THEO THE LOAI GIAM DAN (MERGE SORT)";
    
    HienThiDanhSach_New(ds, "", 0);
}

// ============================================
// YEU CAU 2: SAP XEP TANG THEO GIA SACH
// ============================================
void SapXepTangTheoGia(DanhSachSach &dss) {
    vector<Sach> ds = dss.GetList();
    
    if (ds.empty()) {
        system("cls");
        VeKhungChinh();
        SetColor(12);
        gotoXY(40, 12);
        cout << "Danh sach rong!";
        SetColor(15);
        _getch();
        return;
    }
    
    // Sap xep TANG dan theo gia
    for (size_t i = 0; i < ds.size() - 1; i++) {
        for (size_t j = i + 1; j < ds.size(); j++) {
            if (ds[i].GetGiaSach() > ds[j].GetGiaSach()) {
                Sach temp = ds[i];
                ds[i] = ds[j];
                ds[j] = temp;
            }
        }
    }
    
    // Hien thi ket qua
    system("cls");
    VeKhungChinh();
    SetColor(14);
    gotoXY(30, 2);
    cout << "SAP XEP THEO GIA TANG DAN";
    
    HienThiDanhSach_New(ds, "", 0);
}

// ============================================
// YEU CAU 3: TIM KIEM THEO KHOANG NGAY (1 DIEM)
// ============================================
void TimKiemTheoKhoangNgay(DanhSachSach &dss) {
    system("cls");
    VeKhungChinh();
    VeKhungHuongDan();
    
    SetColor(14);
    gotoXY(30, 5);
    cout << "TIM KIEM THEO KHOANG NGAY NHAP KHO";
    
    // Nhap ngay bat dau
    SetColor(15);
    gotoXY(30, 8);
    cout << "Nhap ngay bat dau (dd/mm/yyyy): ";
    ShowCur(1);
    string ngayBatDau;
    getline(cin, ngayBatDau);
    
    gotoXY(30, 10);
    cout << "Nhap ngay ket thuc (dd/mm/yyyy): ";
    string ngayKetThuc;
    getline(cin, ngayKetThuc);
    ShowCur(0);
    
    // Chuyen doi string sang Date
    Date dateBatDau, dateKetThuc;
    
    // Parse ngay bat dau
    int d1, m1, y1;
    if (sscanf(ngayBatDau.c_str(), "%d/%d/%d", &d1, &m1, &y1) != 3) {
        SetColor(12);
        gotoXY(30, 12);
        cout << "Dinh dang ngay bat dau khong hop le!";
        SetColor(15);
        _getch();
        return;
    }
    dateBatDau.SetNgay(d1);
    dateBatDau.SetThang(m1);
    dateBatDau.SetNam(y1);
    
    // Parse ngay ket thuc
    int d2, m2, y2;
    if (sscanf(ngayKetThuc.c_str(), "%d/%d/%d", &d2, &m2, &y2) != 3) {
        SetColor(12);
        gotoXY(30, 12);
        cout << "Dinh dang ngay ket thuc khong hop le!";
        SetColor(15);
        _getch();
        return;
    }
    dateKetThuc.SetNgay(d2);
    dateKetThuc.SetThang(m2);
    dateKetThuc.SetNam(y2);
    
    // Tim kiem
    vector<Sach> ds = dss.GetList();
    vector<Sach> results;
    
    for (size_t i = 0; i < ds.size(); i++) {
        Date ngayNhap = ds[i].GetNgayNhapKho();
        
        // So sanh ngay: ngayNhap >= dateBatDau && ngayNhap <= dateKetThuc
        bool afterStart = (ngayNhap.GetNam() > dateBatDau.GetNam()) ||
                         (ngayNhap.GetNam() == dateBatDau.GetNam() && ngayNhap.GetThang() > dateBatDau.GetThang()) ||
                         (ngayNhap.GetNam() == dateBatDau.GetNam() && ngayNhap.GetThang() == dateBatDau.GetThang() && ngayNhap.GetNgay() >= dateBatDau.GetNgay());
        
        bool beforeEnd = (ngayNhap.GetNam() < dateKetThuc.GetNam()) ||
                        (ngayNhap.GetNam() == dateKetThuc.GetNam() && ngayNhap.GetThang() < dateKetThuc.GetThang()) ||
                        (ngayNhap.GetNam() == dateKetThuc.GetNam() && ngayNhap.GetThang() == dateKetThuc.GetThang() && ngayNhap.GetNgay() <= dateKetThuc.GetNgay());
        
        if (afterStart && beforeEnd) {
            results.push_back(ds[i]);
        }
    }
    
    // Hien thi ket qua
    if (results.empty()) {
        SetColor(12);
        gotoXY(30, 14);
        cout << "Khong tim thay sach nao trong khoang ngay nay!";
        SetColor(15);
        _getch();
    } else {
        HienThiDanhSach_New(results, "", 0);
    }
}

// ============================================
// YEU CAU 4a: THONG KE THEO NGAY NHAP - BANG (1 DIEM)
// ============================================
void ThongKeTheoNgayNhap_Bang(DanhSachSach &dss) {
    vector<Sach> ds = dss.GetList();
    
    if (ds.empty()) {
        system("cls");
        VeKhungChinh();
        SetColor(12);
        gotoXY(40, 12);
        cout << "Danh sach rong!";
        SetColor(15);
        _getch();
        return;
    }
    
    // Thong ke 3 khoang nam: Truoc 2020, 2020-2023, Sau 2023
    int truoc2020 = 0;
    int tu2020_2023 = 0;
    int sau2023 = 0;
    
    for (size_t i = 0; i < ds.size(); i++) {
        int nam = ds[i].GetNgayNhapKho().GetNam();
        if (nam < 2020) truoc2020++;
        else if (nam <= 2023) tu2020_2023++;
        else sau2023++;
    }
    
    // Hien thi bang
    system("cls");
    VeKhungChinh();
    
    SetColor(14);
    gotoXY(40, 3);
    cout << "THONG KE THEO NGAY NHAP KHO";
    
    int x = 25, y = 7;
    int total = truoc2020 + tu2020_2023 + sau2023;
    
    // Ve bang 4 cot: STT | Khoang nam | So luong | Phan tram (%)
    SetColor(11);
    gotoXY(x, y);
    cout << char(218);
    for (int i = 0; i < 8; i++) cout << char(196);  // STT
    cout << char(194);
    for (int i = 0; i < 20; i++) cout << char(196); // Khoang nam
    cout << char(194);
    for (int i = 0; i < 10; i++) cout << char(196); // So luong
    cout << char(194);
    for (int i = 0; i < 12; i++) cout << char(196); // Phan tram
    cout << char(191);
    
    // Header
    gotoXY(x, y + 1);
    cout << char(179);
    SetColor(14);
    cout << setw(8) << right << "STT " << char(179);
    cout << setw(20) << left << " Khoang nam" << char(179);
    cout << setw(10) << right << "So luong" << char(179);
    cout << setw(12) << right << "Phan tram" << char(179);
    SetColor(11);
    
    // Vien phan cach header
    gotoXY(x, y + 2);
    cout << char(195);
    for (int i = 0; i < 8; i++) cout << char(196);
    cout << char(197);
    for (int i = 0; i < 20; i++) cout << char(196);
    cout << char(197);
    for (int i = 0; i < 10; i++) cout << char(196);
    cout << char(197);
    for (int i = 0; i < 12; i++) cout << char(196);
    cout << char(180);
    
    // Dong 1: Truoc 2020
    gotoXY(x, y + 3);
    cout << char(179);
    SetColor(15);
    cout << setw(8) << right << "1 " << char(179);
    cout << setw(20) << left << " Truoc 2020" << char(179);
    cout << setw(10) << right << truoc2020 << char(179);
    int p1 = (total > 0) ? (truoc2020 * 100 / total) : 0;
    cout << setw(12) << right << p1 << char(179);
    SetColor(11);
    
    // Dong 2: Tu 2020-2023
    gotoXY(x, y + 4);
    cout << char(179);
    SetColor(15);
    cout << setw(8) << right << "2 " << char(179);
    cout << setw(20) << left << " 2020 - 2023" << char(179);
    cout << setw(10) << right << tu2020_2023 << char(179);
    int p2 = (total > 0) ? (tu2020_2023 * 100 / total) : 0;
    cout << setw(12) << right << p2 << char(179);
    SetColor(11);
    
    // Dong 3: Sau 2023
    gotoXY(x, y + 5);
    cout << char(179);
    SetColor(15);
    cout << setw(8) << right << "3 " << char(179);
    cout << setw(20) << left << " Sau 2023" << char(179);
    cout << setw(10) << right << sau2023 << char(179);
    int p3 = (total > 0) ? (sau2023 * 100 / total) : 0;
    cout << setw(12) << right << p3 << char(179);
    SetColor(11);
    
    // Vien duoi
    gotoXY(x, y + 6);
    cout << char(192);
    for (int i = 0; i < 8; i++) cout << char(196);
    cout << char(193);
    for (int i = 0; i < 20; i++) cout << char(196);
    cout << char(193);
    for (int i = 0; i < 10; i++) cout << char(196);
    cout << char(193);
    for (int i = 0; i < 12; i++) cout << char(196);
    cout << char(217);
    
    SetColor(11);
    gotoXY(35, 25);
    cout << "Nhan phim bat ky de quay lai...";
    _getch();
}

// ============================================
// YEU CAU 5: BIEU DO THEO NGAY NHAP (1 DIEM)
// ============================================
void BieuDoTheoNgayNhap(DanhSachSach &dss) {
    vector<Sach> ds = dss.GetList();
    
    if (ds.empty()) {
        system("cls");
        VeKhungChinh();
        SetColor(12);
        gotoXY(40, 12);
        cout << "Danh sach rong!";
        SetColor(15);
        _getch();
        return;
    }
    
    // Thong ke theo nam nhap kho: Truoc 2020, 2020-2023, Sau 2023
    int truoc2020 = 0;
    int tu2020_2023 = 0;
    int sau2023 = 0;
    
    for (size_t i = 0; i < ds.size(); i++) {
        int nam = ds[i].GetNgayNhapKho().GetNam();
        if (nam < 2020) truoc2020++;
        else if (nam <= 2023) tu2020_2023++;
        else sau2023++;
    }
    
    // Ve bieu do
    system("cls");
    VeKhungChinh();
    
    SetColor(14);
    gotoXY(45, 3);
    cout << "So luong";
    
    int maxVal = truoc2020;
    if (tu2020_2023 > maxVal) maxVal = tu2020_2023;
    if (sau2023 > maxVal) maxVal = sau2023;
    
    if (maxVal == 0) maxVal = 1;
    
    int x = 20, y = 8;
    const int HEIGHT = 15;
    
    // Ve truc Y
    SetColor(11);
    for (int i = HEIGHT; i >= 0; i--) {
        gotoXY(x, y + (HEIGHT - i));
        cout << setw(3) << right << (maxVal * i / HEIGHT) << " |";
    }
    
    // Ve 3 cot
    int xPos = x + 10;
    int colors[] = {9, 11, 12};
    int values[] = {truoc2020, tu2020_2023, sau2023};
    string labels[] = {"< 2020", "2020-2023", "> 2023"};
    
    for (int i = 0; i < 3; i++) {
        int barHeight = (maxVal > 0) ? (values[i] * HEIGHT / maxVal) : 0;
        if (barHeight < 1 && values[i] > 0) barHeight = 1;
        
        SetColor(colors[i]);
        for (int h = 0; h < barHeight; h++) {
            gotoXY(xPos, y + HEIGHT - h);
            for (int w = 0; w < 12; w++) cout << char(219);
        }
        
        // Hien thi gia tri
        SetColor(14);
        gotoXY(xPos + 4, y + HEIGHT - barHeight - 1);
        cout << values[i];
        
        // Nhan
        SetColor(15);
        gotoXY(xPos, y + HEIGHT + 2);
        cout << labels[i];
        
        SetColor(15);
        gotoXY(xPos + 5, y + HEIGHT + 3);
        cout << (i + 1);
        
        xPos += 22;
    }
    
    // Ve truc X
    SetColor(11);
    gotoXY(x + 5, y + HEIGHT);
    for (int i = 0; i < 66; i++) cout << char(196);
    
    SetColor(11);
    gotoXY(35, 27);
    cout << "Nhan phim bat ky de quay lai...";
    _getch();
}

// ============================================
// MENU THI CUOI KY CHINH
// ============================================
void MenuThiCuoiKy(DanhSachSach &dss) {
    while (true) {
        string menu[] = {
            "1. Sap xep the loai giam dan (Merge Sort)",
            "2. Sap xep gia tang dan",
            "3. Tim kiem theo khoang ngay nhap kho",
            "4. Thong ke theo ngay nhap kho (Bang)",
            "5. Bieu do theo ngay nhap kho",
            "6. Quay lai"
        };
        
        int choice = MenuConChung("===== MENU THI CUOI KY =====", menu, 6, "Chuc nang");
        
        switch (choice) {
            case 1:
                ThucHienMergeSortTheoTheLoaiGiam(dss);
                break;
            case 2:
                SapXepTangTheoGia(dss);
                break;
            case 3:
                TimKiemTheoKhoangNgay(dss);
                break;
            case 4:
                ThongKeTheoNgayNhap_Bang(dss);
                break;
            case 5:
                BieuDoTheoNgayNhap(dss);
                break;
            case 6:
                return;
        }
    }
}

#endif
