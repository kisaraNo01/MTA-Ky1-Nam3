#ifndef SAPXEP_H
#define SAPXEP_H

#include "Sach.h"
#include <vector>

using namespace std;

// ============================================
// CAC HAM SO SANH - COMPARISON FUNCTIONS
// ============================================

// So sanh theo Ma The Loai (key = 0)
bool SoSanhMaTheLoai(Sach a, Sach b) {
    return a.GetMaTheLoai() < b.GetMaTheLoai();
}

// So sanh theo Ma Sach (key = 1)
bool SoSanhMaSach(Sach a, Sach b) {
    return a.GetMaSach() < b.GetMaSach();
}

// So sanh theo Ten Sach (key = 2)
bool SoSanhTenSach(Sach a, Sach b) {
    return a.GetTenSach() < b.GetTenSach();
}

// So sanh theo Tac Gia (key = 3)
bool SoSanhTacGia(Sach a, Sach b) {
    return a.GetTacGia() < b.GetTacGia();
}

// So sanh theo Ngay Nhap Kho (key = 4)
bool SoSanhNgayNhap(Sach a, Sach b) {
    Date d1 = a.GetNgayNhapKho();
    Date d2 = b.GetNgayNhapKho();
    if (d1.GetNam() != d2.GetNam()) return d1.GetNam() < d2.GetNam();
    if (d1.GetThang() != d2.GetThang()) return d1.GetThang() < d2.GetThang();
    return d1.GetNgay() < d2.GetNgay();
}

// So sanh theo Gia Sach (key = 5)
bool SoSanhGiaSach(Sach a, Sach b) {
    return a.GetGiaSach() < b.GetGiaSach();
}

// So sanh da tieu chi: Ma The Loai -> Ten Sach -> Ngay Nhap
// (key = 6)
bool SoSanhDaTieuChi(Sach a, Sach b) {
    // Uu tien 1: Ma The Loai
    if (a.GetMaTheLoai() != b.GetMaTheLoai()) {
        return a.GetMaTheLoai() < b.GetMaTheLoai();
    }
    
    // Uu tien 2: Ten Sach (neu cung Ma The Loai)
    if (a.GetTenSach() != b.GetTenSach()) {
        return a.GetTenSach() < b.GetTenSach();
    }
    
    // Uu tien 3: Ngay Nhap Kho (neu cung Ma The Loai va Ten Sach)
    Date d1 = a.GetNgayNhapKho();
    Date d2 = b.GetNgayNhapKho();
    if (d1.GetNam() != d2.GetNam()) return d1.GetNam() < d2.GetNam();
    if (d1.GetThang() != d2.GetThang()) return d1.GetThang() < d2.GetThang();
    return d1.GetNgay() < d2.GetNgay();
}

// Ham chon ham so sanh theo key
bool SoSanhTheoKey(Sach a, Sach b, int key) {
    switch(key) {
        case 0: return SoSanhMaTheLoai(a, b);
        case 1: return SoSanhMaSach(a, b);
        case 2: return SoSanhTenSach(a, b);
        case 3: return SoSanhTacGia(a, b);
        case 4: return SoSanhNgayNhap(a, b);
        case 5: return SoSanhGiaSach(a, b);
        case 6: return SoSanhDaTieuChi(a, b);
        default: return false;
    }
}

// ============================================
// THUAT TOAN SAP XEP - SORTING ALGORITHMS
// ============================================

// 1. SELECTION SORT - Sap xep chon
void SelectionSort(vector<Sach>& ds, int key) {
    int n = ds.size();
    for (int i = 0; i < n - 1; i++) {
        int min_idx = i;
        for (int j = i + 1; j < n; j++) {
            if (SoSanhTheoKey(ds[j], ds[min_idx], key)) {
                min_idx = j;
            }
        }
        if (min_idx != i) {
            swap(ds[min_idx], ds[i]);
        }
    }
}

// 2. INSERTION SORT - Sap xep chen
void InsertionSort(vector<Sach>& ds, int key) {
    int n = ds.size();
    for (int i = 1; i < n; i++) {
        Sach keyItem = ds[i];
        int j = i - 1;
        while (j >= 0 && SoSanhTheoKey(keyItem, ds[j], key)) {
            ds[j + 1] = ds[j];
            j = j - 1;
        }
        ds[j + 1] = keyItem;
    }
}

// 3. HEAP SORT - Sap xep vun dong
void Heapify(vector<Sach>& ds, int n, int i, int key) {
    int largest = i;
    int left = 2 * i + 1;
    int right = 2 * i + 2;
    
    if (left < n && !SoSanhTheoKey(ds[largest], ds[left], key)) {
        largest = left;
    }
    if (right < n && !SoSanhTheoKey(ds[largest], ds[right], key)) {
        largest = right;
    }
    
    if (largest != i) {
        swap(ds[i], ds[largest]);
        Heapify(ds, n, largest, key);
    }
}

void HeapSort(vector<Sach>& ds, int key) {
    int n = ds.size();
    // Xay dung heap (sap xep lai mang)
    for (int i = n / 2 - 1; i >= 0; i--) {
        Heapify(ds, n, i, key);
    }
    // Trich xuat tung phan tu tu heap
    for (int i = n - 1; i > 0; i--) {
        swap(ds[0], ds[i]);
        Heapify(ds, i, 0, key);
    }
}

// 4. QUICK SORT - Sap xep nhanh
int Partition(vector<Sach>& ds, int low, int high, int key) {
    Sach pivot = ds[high];
    int i = (low - 1);
    for (int j = low; j <= high - 1; j++) {
        if (SoSanhTheoKey(ds[j], pivot, key)) {
            i++;
            swap(ds[i], ds[j]);
        }
    }
    swap(ds[i + 1], ds[high]);
    return (i + 1);
}

void QuickSort(vector<Sach>& ds, int low, int high, int key) {
    if (low < high) {
        int pi = Partition(ds, low, high, key);
        QuickSort(ds, low, pi - 1, key);
        QuickSort(ds, pi + 1, high, key);
    }
}

void QuickSortWrapper(vector<Sach>& ds, int key) {
    if (!ds.empty()) QuickSort(ds, 0, ds.size() - 1, key);
}

// ============================================
// HAM TICH HOP VOI CLASS DanhSachSach
// ============================================

// Them cac phuong thuc sap xep vao class DanhSachSach
// Su dung cac ham tren

#endif
