#ifndef HIENTHIMOI_H
#define HIENTHIMOI_H

// HAM HIEN THI DANH SACH - PHIEN BAN MOI (CO COT THE LOAI)
// File nay duoc include TU GiaoDien.h (sau khi tat ca da duoc dinh nghia)
// Khong can include gi ca, tat ca da co san

#include <iostream>
#include <string>
#include <iomanip>
#include <sstream>
#include <vector>

using namespace std;

// Helper: Chuyen Date thanh string
string DateToString(Date d) {
    ostringstream oss;
    oss << d;
    return oss.str();
}

// Helper: Chuyen chuoi thanh chu thuong
string ToLower(string s) {
    string result = s;
    for (size_t i = 0; i < result.length(); i++) {
        if (result[i] >= 'A' && result[i] <= 'Z') {
            result[i] = result[i] + 32;
        }
    }
    return result;
}

// Helper: In chuoi voi tu khoa duoc to do (highlight ca chu va so)
void PrintWithHighlight(string text, string keyword, bool isSelected, int maxWidth) {
    if (keyword.empty() || text.empty()) {
        // Khong co keyword, in binh thuong
        string display = text;
        if (display.length() > (size_t)maxWidth) {
            display = display.substr(0, maxWidth);
        }
        cout << setw(maxWidth) << left << display;
        return;
    }
    
    // Tim vi tri cua keyword trong text (khong phan biet hoa thuong)
    // Chi chuyen chu cai thanh chu thuong, giu nguyen so
    string textLower = ToLower(text);
    string keywordLower = ToLower(keyword);
    
    size_t pos = textLower.find(keywordLower);
    
    if (pos == string::npos) {
        // Khong tim thay keyword, in binh thuong
        string display = text;
        if (display.length() > (size_t)maxWidth) {
            display = display.substr(0, maxWidth);
        }
        cout << setw(maxWidth) << left << display;
        return;
    }
    
    // Cat text neu qua dai
    string display = text;
    if (display.length() > (size_t)maxWidth) {
        // Neu keyword nam ngoai phan hien thi, cat sao cho keyword van hien thi
        if (pos >= (size_t)maxWidth) {
            size_t newStart = pos - 5;
            if (newStart + maxWidth > display.length()) {
                newStart = display.length() - maxWidth;
            }
            display = display.substr(newStart, maxWidth);
            pos = pos - newStart;
        } else {
            display = display.substr(0, maxWidth);
        }
    }
    
    // Tim lai vi tri trong display (case-insensitive)
    string displayLower = ToLower(display);
    pos = displayLower.find(keywordLower);
    
    if (pos == string::npos) {
        cout << setw(maxWidth) << left << display;
        return;
    }
    
    // In phan truoc keyword
    string before = display.substr(0, pos);
    cout << before;
    
    // In keyword voi mau do - LAY CHINH XAC DO DAI CUA KEYWORD
    // De highlight ca chu va so chinh xac
    string keywordPart = display.substr(pos, keyword.length());
    if (isSelected) {
        setConsoleColor(12, MenuConfig::COLOR_BG_HOVER); // Do tren nen xanh
    } else {
        SetColor(12); // Mau do
    }
    cout << keywordPart;
    
    // Khoi phuc mau
    if (isSelected) {
        setConsoleColor(15, MenuConfig::COLOR_BG_HOVER);
    } else {
        SetColor(15);
    }
    
    // In phan sau keyword
    string after = display.substr(pos + keyword.length());
    cout << after;
    
    // Them khoang trang de du maxWidth
    int printed = before.length() + keywordPart.length() + after.length();
    for (int i = printed; i < maxWidth; i++) {
        cout << " ";
    }
}

// Helper: Ve mot dong du lieu
// highlightColumn: Bitmask - Bit 0=MaTL, 1=TenTL, 2=MaSach, 3=TenSach, 4=TacGia
// VD: highlightColumn = 0b10000 (16) = chi highlight TacGia
//     highlightColumn = 0b11000 (24) = highlight TenSach va TacGia
void VeDongDuLieu(int x, int y, Sach& sach, int stt, int colWidths[], bool isSelected, string keyword = "", int highlightColumn = 0) {
    // Neu dong duoc chon, to mau nen xanh duong
    if (isSelected) {
        setConsoleColor(15, MenuConfig::COLOR_BG_HOVER);
    }
    
    SetColor(11);
    gotoXY(x, y);
    cout << char(179);
    
    // STT
    if (isSelected) setConsoleColor(15, MenuConfig::COLOR_BG_HOVER);
    else SetColor(15);
    cout << setw(colWidths[0]) << right << stt;
    
    // Ma The Loai
    SetColor(11);
    cout << char(179);
    if (isSelected) setConsoleColor(15, MenuConfig::COLOR_BG_HOVER);
    else SetColor(15);
    if (highlightColumn & 1) { // Bit 0
        PrintWithHighlight(sach.GetMaTheLoai(), keyword, isSelected, colWidths[1]);
    } else {
        cout << setw(colWidths[1]) << left << sach.GetMaTheLoai();
    }
    
    // The Loai
    SetColor(11);
    cout << char(179);
    if (isSelected) setConsoleColor(15, MenuConfig::COLOR_BG_HOVER);
    else SetColor(15);
    if (highlightColumn & 2) { // Bit 1
        PrintWithHighlight(sach.GetTenTheLoai(), keyword, isSelected, colWidths[2]);
    } else {
        string tenTL = sach.GetTenTheLoai();
        if (tenTL.length() > (size_t)colWidths[2]) tenTL = tenTL.substr(0, colWidths[2]);
        cout << setw(colWidths[2]) << left << tenTL;
    }
    
    // Ma Sach
    SetColor(11);
    cout << char(179);
    if (isSelected) setConsoleColor(15, MenuConfig::COLOR_BG_HOVER);
    else SetColor(15);
    if (highlightColumn & 4) { // Bit 2
        PrintWithHighlight(sach.GetMaSach(), keyword, isSelected, colWidths[3]);
    } else {
        cout << setw(colWidths[3]) << right << sach.GetMaSach();
    }
    
    // Ten Sach
    SetColor(11);
    cout << char(179);
    if (isSelected) setConsoleColor(15, MenuConfig::COLOR_BG_HOVER);
    else SetColor(15);
    if (highlightColumn & 8) { // Bit 3
        PrintWithHighlight(sach.GetTenSach(), keyword, isSelected, colWidths[4]);
    } else {
        string tenSach = sach.GetTenSach();
        if (tenSach.length() > (size_t)colWidths[4]) tenSach = tenSach.substr(0, colWidths[4]);
        cout << setw(colWidths[4]) << left << tenSach;
    }
    
    // Tac Gia
    SetColor(11);
    cout << char(179);
    if (isSelected) setConsoleColor(15, MenuConfig::COLOR_BG_HOVER);
    else SetColor(15);
    if (highlightColumn & 16) { // Bit 4
        PrintWithHighlight(sach.GetTacGia(), keyword, isSelected, colWidths[5]);
    } else {
        string tacGia = sach.GetTacGia();
        if (tacGia.length() > (size_t)colWidths[5]) tacGia = tacGia.substr(0, colWidths[5]);
        cout << setw(colWidths[5]) << left << tacGia;
    }
    
    // Ngay Nhap
    SetColor(11);
    cout << char(179);
    if (isSelected) setConsoleColor(15, MenuConfig::COLOR_BG_HOVER);
    else SetColor(15);
    cout << setw(colWidths[6]) << right << DateToString(sach.GetNgayNhapKho());
    
    // Gia
    SetColor(11);
    cout << char(179);
    if (isSelected) setConsoleColor(15, MenuConfig::COLOR_BG_HOVER);
    else SetColor(15);
    cout << setw(colWidths[7]) << right << sach.GetGiaSach();
    
    SetColor(11);
    cout << char(179);
    
    // Reset mau ve mac dinh
    if (isSelected) setConsoleColor(15, 0);
}


void HienThiDanhSach_New(vector<Sach> ds, string keyword = "", int highlightColumn = 0) {
    if (ds.empty()) {
        system("cls");
        VeKhungChinh();
        SetColor(MenuConfig::COLOR_GUIDE);
        gotoXY(40, 12);
        cout << "Danh sach trong!";
        gotoXY(35, 24);
        cout << "Nhan phim bat ky de quay lai...";
        _getch();
        return;
    }
    
    const int ITEMS_PER_PAGE = 10;
    int totalItems = ds.size();
    int totalPages = (totalItems + ITEMS_PER_PAGE - 1) / ITEMS_PER_PAGE;
    int currentPage = 1;
    int selectedRow = 0;  // Chi so dong duoc chon (0 - 9)
    int lastPage = -1;  // Trang truoc do, de biet khi nao can ve lai toan bo
    
    // Do rong cac cot - TONG = 110 (vua khung 120)
    // STT | Ma TL | The Loai | Ma Sach | Ten Sach | Tac Gia | Ngay Nhap | Gia
    int colWidths[] = {4, 7, 15, 10, 22, 18, 12, 12};
    
    int x = 5;
    int y = 5;
    
    while (true) {
        int startIdx = (currentPage - 1) * ITEMS_PER_PAGE;
        int endIdx = min(startIdx + ITEMS_PER_PAGE, totalItems);
        int row = y + 3;
        
        // Chi ve lai toan bo khi chuyen trang
        if (currentPage != lastPage) {
            system("cls");
            VeKhungChinh();
            
            SetColor(MenuConfig::COLOR_TITLE);
            gotoXY(40, 2);
            cout << "===== DANH SACH SACH =====";
            
            // Vien tren
            SetColor(11);
            gotoXY(x, y);
            cout << char(218);
            for (int i = 0; i < 8; i++) {
                cout << string(colWidths[i], char(196));
                cout << (i == 7 ? char(191) : char(194));
            }
            
            gotoXY(x, y + 1);
            cout << char(179);
            SetColor(14);
            cout << setw(colWidths[0]) << right << "STT" << char(179)
                 << setw(colWidths[1]) << left << "Ma TL" << char(179)
                 << setw(colWidths[2]) << left << "The Loai" << char(179)
                 << setw(colWidths[3]) << right << "Ma Sach" << char(179)
                 << setw(colWidths[4]) << left << "Ten Sach" << char(179)
                 << setw(colWidths[5]) << left << "Tac Gia" << char(179)
                 << setw(colWidths[6]) << right << "Ngay Nhap" << char(179)
                 << setw(colWidths[7]) << right << "Gia" << char(179);
            
            // Vien ngang sau header
            SetColor(11);
            gotoXY(x, y + 2);
            cout << char(195);
            for (int i = 0; i < 8; i++) {
                cout << string(colWidths[i], char(196));
                cout << (i == 7 ? char(180) : char(197));
            }
            
            // Ve tat ca cac dong
            for (int i = startIdx; i < endIdx; i++) {
                int rowIdx = i - startIdx;
                VeDongDuLieu(x, row + rowIdx, ds[i], i + 1, colWidths, rowIdx == selectedRow, keyword, highlightColumn);
            }
            
            // Vien duoi
            SetColor(11);
            gotoXY(x, row + (endIdx - startIdx));
            cout << char(192);
            for (int i = 0; i < 8; i++) {
                cout << string(colWidths[i], char(196));
                cout << (i == 7 ? char(217) : char(193));
            }
            
            // Thong tin
            SetColor(MenuConfig::COLOR_TITLE);
            gotoXY(x + 2, row + (endIdx - startIdx) + 2);
            cout << "Trang: " << currentPage << "/" << totalPages;
            gotoXY(x + 30, row + (endIdx - startIdx) + 2);
            cout << "Tong so sach: " << totalItems;
            
            // Huong dan
            SetColor(MenuConfig::COLOR_GUIDE);
            gotoXY(x + 2, row + (endIdx - startIdx) + 3);
            if (totalPages > 1) {
                cout << "Huong dan: [Len/Xuong] Di chuyen  [<-/->] Trang  [ESC] Thoat";
            } else {
                cout << "Huong dan: [Len/Xuong] Di chuyen  [ESC] Thoat";
            }
            
            lastPage = currentPage;
        }
        
        // Xu ly phim
        char c = _getch();
        if (c == 27) break;  // ESC
        
        if (c == -32) {  // Phim mui ten
            c = _getch();
            int itemsInCurrentPage = endIdx - startIdx;
            int oldSelectedRow = selectedRow;
            int oldPage = currentPage;
            
            if (c == 72) {  // Mui ten len
                if (selectedRow > 0) {
                    selectedRow--;
                } else if (currentPage > 1) {
                    currentPage--;
                    int newStartIdx = (currentPage - 1) * ITEMS_PER_PAGE;
                    int newEndIdx = min(newStartIdx + ITEMS_PER_PAGE, totalItems);
                    selectedRow = newEndIdx - newStartIdx - 1;
                }
            }
            else if (c == 80) {  // Mui ten xuong
                if (selectedRow < itemsInCurrentPage - 1) {
                    selectedRow++;
                } else if (currentPage < totalPages) {
                    currentPage++;
                    selectedRow = 0;
                }
            }
            else if (c == 77 && currentPage < totalPages) {  // Mui ten phai
                currentPage++;
                selectedRow = 0;
            }
            else if (c == 75 && currentPage > 1) {  // Mui ten trai
                currentPage--;
                selectedRow = 0;
            }
            
            // Chi ve lai 2 dong thay doi neu van o cung trang
            if (currentPage == oldPage && selectedRow != oldSelectedRow) {
                // Ve lai dong cu (khong highlight)
                int oldIdx = startIdx + oldSelectedRow;
                VeDongDuLieu(x, row + oldSelectedRow, ds[oldIdx], oldIdx + 1, colWidths, false, keyword, highlightColumn);
                
                // Ve lai dong moi (co highlight)
                int newIdx = startIdx + selectedRow;
                VeDongDuLieu(x, row + selectedRow, ds[newIdx], newIdx + 1, colWidths, true, keyword, highlightColumn);
            }
        }
    }
}

#endif
