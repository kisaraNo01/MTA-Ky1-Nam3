#ifndef GIAODIENBD_H
#define GIAODIENBD_H

#include "Library.h"
#include <iostream>
#include <string>
#include <iomanip>
#include <conio.h>

using namespace std;

// ============================================
// CONSTANTS - Cau hinh mau sac va kich thuoc
// ============================================
namespace MenuConfig {
    // Mau sac
    const int COLOR_BORDER = 10;        // Xanh la - vien khung
    const int COLOR_BG_NORMAL = 0;      // Den - nen binh thuong
    const int COLOR_BG_HOVER = 9;       // Xanh duong sang - nen khi hover
    const int COLOR_BG_ACTIVE = 14;     // Vang - nen khi bam
    const int COLOR_TEXT = 15;          // Trang - chu
    const int COLOR_TITLE = 14;         // Vang - tieu de
    const int COLOR_GUIDE = 11;         // Cyan - huong dan
    const int COLOR_BREADCRUMB = 13;    // Hong - breadcrumb
    
    // Kich thuoc menu
    const int MENU_WIDTH = 28;
    const int MENU_HEIGHT = 2;
    const int MENU_SPACING = 2;  // Giam tu 3 xuong 2 de khong de len khung
    
    // Vi tri khung chinh
    const int FRAME_X = 2;
    const int FRAME_Y = 0;
    const int FRAME_WIDTH = 120;  // Tang tu 110 len 120
    const int FRAME_HEIGHT = 28;
}

// ============================================
// BIEN TOAN CUC
// ============================================
string breadcrumb_trail = "";

// ============================================
// KHAI BAO NGUYEN MAU
// ============================================
void VeKhungChinh();
void VeKhungHuongDan();
void CapNhatBreadcrumb(string item);

// ============================================
// HAM VE CO BAN - Toi uu hoa
// ============================================

// Ve mot o menu voi vien va nen
void VeOMenu(int x, int y, int w, int h, int border_color, int bg_color, string text) {
    // To mau nen
    textcolor(bg_color);
    for (int iy = y + 1; iy <= y + h - 1; iy++) {
        for (int ix = x + 1; ix <= x + w - 1; ix++) {
            gotoXY(ix, iy);
            cout << " ";
        }
    }
    
    // Ve chu (can giua)
    SetColor(MenuConfig::COLOR_TEXT);
    int text_x = x + (w - text.length()) / 2;
    gotoXY(text_x, y + 1);
    cout << text;
    
    // Ve vien
    textcolor(0);
    SetColor(border_color);
    for (int ix = x; ix <= x + w; ix++) {
        gotoXY(ix, y); cout << char(196);
        gotoXY(ix, y + h); cout << char(196);
    }
    for (int iy = y; iy <= y + h; iy++) {
        gotoXY(x, iy); cout << char(179);
        gotoXY(x + w, iy); cout << char(179);
    }
    gotoXY(x, y); cout << char(218);
    gotoXY(x + w, y); cout << char(191);
    gotoXY(x + w, y + h); cout << char(217);
    gotoXY(x, y + h); cout << char(192);
}

// Ve o menu voi text can trai
void VeOMenuCanTrai(int x, int y, int w, int h, int border_color, int bg_color, string text) {
    // To mau nen
    textcolor(bg_color);
    for (int iy = y + 1; iy <= y + h - 1; iy++) {
        for (int ix = x + 1; ix <= x + w - 1; ix++) {
            gotoXY(ix, iy);
            cout << " ";
        }
    }
    
    // Ve chu (can trai, cach le 3 ky tu)
    SetColor(MenuConfig::COLOR_TEXT);
    gotoXY(x + 3, y + 1);
    cout << text;
    
    // Ve vien
    textcolor(0);
    SetColor(border_color);
    for (int ix = x; ix <= x + w; ix++) {
        gotoXY(ix, y); cout << char(196);
        gotoXY(ix, y + h); cout << char(196);
    }
    for (int iy = y; iy <= y + h; iy++) {
        gotoXY(x, iy); cout << char(179);
        gotoXY(x + w, iy); cout << char(179);
    }
    gotoXY(x, y); cout << char(218);
    gotoXY(x + w, y); cout << char(191);
    gotoXY(x + w, y + h); cout << char(217);
    gotoXY(x, y + h); cout << char(192);
}


// Highlight mot o menu (voi hieu ung)
void HighlightOMenu(int x, int y, int w, int h, int bg_color, string text) {
    // To mau nen
    textcolor(bg_color);
    for (int iy = y + 1; iy <= y + h - 1; iy++) {
        for (int ix = x + 1; ix <= x + w - 1; ix++) {
            gotoXY(ix, iy);
            cout << " ";
        }
    }
    
    // Ve chu (can giua, mau sang)
    SetColor(MenuConfig::COLOR_TEXT);
    int text_x = x + (w - text.length()) / 2;
    gotoXY(text_x, y + 1);
    cout << text;
    
    textcolor(0);
}

// Highlight toan bo o menu (to mau nen toan bo nut)
void HighlightOMenuFull(int x, int y, int w, int h, int bg_color, string text) {
    // Ve vien truoc
    setConsoleColor(MenuConfig::COLOR_BORDER, 0);  // Vien xanh la, nen den
    for (int ix = x; ix <= x + w; ix++) {
        gotoXY(ix, y); cout << char(196);
        gotoXY(ix, y + h); cout << char(196);
    }
    for (int iy = y; iy <= y + h; iy++) {
        gotoXY(x, iy); cout << char(179);
        gotoXY(x + w, iy); cout << char(179);
    }
    gotoXY(x, y); cout << char(218);
    gotoXY(x + w, y); cout << char(191);
    gotoXY(x + w, y + h); cout << char(217);
    gotoXY(x, y + h); cout << char(192);
    
    // To mau nen BEN TRONG khung (khong tran ra vien)
    setConsoleColor(15, bg_color);  // Chu trang, nen mau bg_color
    for (int iy = y + 1; iy < y + h; iy++) {  // y+1 den y+h-1 (khong cham vien)
        for (int ix = x + 1; ix < x + w; ix++) {  // x+1 den x+w-1 (khong cham vien)
            gotoXY(ix, iy);
            cout << " ";
        }
    }
    
    // Ve chu (can giua, mau trang tren nen mau)
    setConsoleColor(15, bg_color);  // Chu trang, nen mau bg_color
    int text_x = x + (w - text.length()) / 2;
    gotoXY(text_x, y + 1);
    cout << text;
    
    // Reset ve mau mac dinh
    setConsoleColor(15, 0);  // Chu trang, nen den
}

// Highlight toan bo o menu voi text can trai
void HighlightOMenuFullCanTrai(int x, int y, int w, int h, int bg_color, string text) {
    // Ve vien truoc
    setConsoleColor(MenuConfig::COLOR_BORDER, 0);  // Vien xanh la, nen den
    for (int ix = x; ix <= x + w; ix++) {
        gotoXY(ix, y); cout << char(196);
        gotoXY(ix, y + h); cout << char(196);
    }
    for (int iy = y; iy <= y + h; iy++) {
        gotoXY(x, iy); cout << char(179);
        gotoXY(x + w, iy); cout << char(179);
    }
    gotoXY(x, y); cout << char(218);
    gotoXY(x + w, y); cout << char(191);
    gotoXY(x + w, y + h); cout << char(217);
    gotoXY(x, y + h); cout << char(192);
    
    // To mau nen BEN TRONG khung (khong tran ra vien)
    setConsoleColor(15, bg_color);  // Chu trang, nen mau bg_color
    for (int iy = y + 1; iy < y + h; iy++) {
        for (int ix = x + 1; ix < x + w; ix++) {
            gotoXY(ix, iy);
            cout << " ";
        }
    }
    
    // Ve chu (can trai, cach le 3 ky tu)
    setConsoleColor(15, bg_color);
    gotoXY(x + 3, y + 1);
    cout << text;
    
    // Reset ve mau mac dinh
    setConsoleColor(15, 0);
}

// Hieu ung nhan nut (nhap nhay) - Toi uu de chuyen menu nhanh
void AnimatePress(int x, int y, int w, int h, string text) {
    // Chi nhan nhay 1 lan thay vi 3 lan, giam delay
    HighlightOMenuFullCanTrai(x, y, w, h, MenuConfig::COLOR_BG_ACTIVE, text);
    Sleep(50);  // Giam tu 80ms xuong 50ms
}

// ============================================
// VE KHUNG VA HUONG DAN
// ============================================

void VeKhungChinh() {
    int x = MenuConfig::FRAME_X;
    int y = MenuConfig::FRAME_Y;
    int w = MenuConfig::FRAME_WIDTH;
    int h = MenuConfig::FRAME_HEIGHT;
    
    SetColor(MenuConfig::COLOR_BORDER);
    for (int ix = x; ix <= x + w; ix++) {
        gotoXY(ix, y); cout << char(196);
        gotoXY(ix, y + h); cout << char(196);
    }
    for (int iy = y; iy <= y + h; iy++) {
        gotoXY(x, iy); cout << char(179);
        gotoXY(x + w, iy); cout << char(179);
    }
    gotoXY(x, y); cout << char(218);
    gotoXY(x + w, y); cout << char(191);
    gotoXY(x + w, y + h); cout << char(217);
    gotoXY(x, y + h); cout << char(192);
}

void VeKhungHuongDan() {
    // KHUNG 1: Huong dan phim (tren)
    int x1 = 5, y1 = 20, w1 = 104, h1 = 2;  // Thay doi y1 tu 18 -> 20
    SetColor(MenuConfig::COLOR_BORDER);
    for (int ix = x1; ix <= x1 + w1; ix++) {
        gotoXY(ix, y1); cout << char(196);
        gotoXY(ix, y1 + h1); cout << char(196);
    }
    for (int iy = y1; iy <= y1 + h1; iy++) {
        gotoXY(x1, iy); cout << char(179);
        gotoXY(x1 + w1, iy); cout << char(179);
    }
    gotoXY(x1, y1); cout << char(218);
    gotoXY(x1 + w1, y1); cout << char(191);
    gotoXY(x1 + w1, y1 + h1); cout << char(217);
    gotoXY(x1, y1 + h1); cout << char(192);
    
    // Noi dung huong dan
    gotoXY(x1 + 2, y1 + 1); SetColor(7); cout << "Huong dan :";
    gotoXY(x1 + 15, y1 + 1); SetColor(11); cout << "Len";
    gotoXY(x1 + 25, y1 + 1); SetColor(10); cout << "Xuong";
    gotoXY(x1 + 35, y1 + 1); SetColor(14); cout << "Enter";
    gotoXY(x1 + 45, y1 + 1); SetColor(12); cout << "Esc: Thoat";
    
    // KHUNG 2: Breadcrumb/Dieu huong (giua)
    int x2 = 5, y2 = 23, w2 = 104, h2 = 2;  // Thay doi y2 tu 21 -> 23
    SetColor(MenuConfig::COLOR_BORDER);
    for (int ix = x2; ix <= x2 + w2; ix++) {
        gotoXY(ix, y2); cout << char(196);
        gotoXY(ix, y2 + h2); cout << char(196);
    }
    for (int iy = y2; iy <= y2 + h2; iy++) {
        gotoXY(x2, iy); cout << char(179);
        gotoXY(x2 + w2, iy); cout << char(179);
    }
    gotoXY(x2, y2); cout << char(218);
    gotoXY(x2 + w2, y2); cout << char(191);
    gotoXY(x2 + w2, y2 + h2); cout << char(217);
    gotoXY(x2, y2 + h2); cout << char(192);
    
    // Breadcrumb trail
    if (!breadcrumb_trail.empty()) {
        gotoXY(x2 + 2, y2 + 1);
        SetColor(MenuConfig::COLOR_BREADCRUMB);
        string display = breadcrumb_trail;
        if (display.length() > 95) {
            display = "..." + display.substr(display.length() - 92);
        }
        cout << display;
    }
    
    // KHUNG 3: Tac gia (duoi ben phai)
    int x3 = 86, y3 = 26, w3 = 23, h3 = 2;  // Thay doi y3 tu 24 -> 26
    SetColor(MenuConfig::COLOR_BORDER);
    for (int ix = x3; ix <= x3 + w3; ix++) {
        gotoXY(ix, y3); cout << char(196);
        gotoXY(ix, y3 + h3); cout << char(196);
    }
    for (int iy = y3; iy <= y3 + h3; iy++) {
        gotoXY(x3, iy); cout << char(179);
        gotoXY(x3 + w3, iy); cout << char(179);
    }
    gotoXY(x3, y3); cout << char(218);
    gotoXY(x3 + w3, y3); cout << char(191);
    gotoXY(x3 + w3, y3 + h3); cout << char(217);
    gotoXY(x3, y3 + h3); cout << char(192);
    
    gotoXY(x3 + 1, y3 + 1); SetColor(13); cout << "Tran Kim Chau An59";
}

void CapNhatBreadcrumb(string item) {
    if (!breadcrumb_trail.empty()) {
        breadcrumb_trail += " > ";
    }
    breadcrumb_trail += item;
}

// ============================================
// MAN HINH CHAO MUNG
// ============================================
void ManHinhChinh() {
    system("cls");
    VeKhungChinh();
    
    // Tieu de - Can giua theo khung chinh
    SetColor(MenuConfig::COLOR_TITLE);
    int tw = 50, th = 4;
    int tx = MenuConfig::FRAME_X + (MenuConfig::FRAME_WIDTH - tw) / 2;  // Can giua tieu de
    int ty = 8;
    
    for (int i = tx; i <= tx + tw; i++) {
        gotoXY(i, ty); cout << "=";
        gotoXY(i, ty + th); cout << "=";
    }
    for (int i = ty; i <= ty + th; i++) {
        gotoXY(tx, i); cout << "|";
        gotoXY(tx + tw, i); cout << "|";
    }
    
    gotoXY(tx + 8, ty + 2);
    cout << "CHUONG TRINH QUAN LY SACH - LIBRARY";
    
    // Cac nut - Can giua man hinh, can doi voi tieu de
    int w = 15, h = 2, gap = 30;
    int total_width = 2 * w + gap;  // Tong chieu rong: 15 + 30 + 15 = 60
    int xms = MenuConfig::FRAME_X + (MenuConfig::FRAME_WIDTH - total_width) / 2;  // Can giua 2 nut
    int yms = 18;
    
    VeOMenu(xms, yms, w, h, MenuConfig::COLOR_BORDER, MenuConfig::COLOR_BG_NORMAL, "Start");
    VeOMenu(xms + w + gap, yms, w, h, MenuConfig::COLOR_BORDER, MenuConfig::COLOR_BG_NORMAL, "End");
    
    // Xu ly chon
    int current = 0; // 0 = Start, 1 = End
    string labels[2] = {"Start", "End"};
    
    HighlightOMenuFull(xms, yms, w, h, MenuConfig::COLOR_BG_HOVER, labels[0]);
    
    while (true) {
        if (_kbhit()) {
            char c = _getch();
            if (c == -32) {
                c = _getch();
                int old = current;
                
                if (c == 75 && current == 1) current = 0; // Trai
                if (c == 77 && current == 0) current = 1; // Phai
                
                if (old != current) {
                    // Xoa highlight cu
                    int old_x = (old == 0) ? xms : xms + w + gap;
                    VeOMenu(old_x, yms, w, h, MenuConfig::COLOR_BORDER, MenuConfig::COLOR_BG_NORMAL, labels[old]);
                    
                    // Highlight moi - Highlight toan bo nut
                    int new_x = (current == 0) ? xms : xms + w + gap;
                    HighlightOMenuFull(new_x, yms, w, h, MenuConfig::COLOR_BG_HOVER, labels[current]);
                }
            }
            else if (c == 13) { // Enter
                int x = (current == 0) ? xms : xms + w + gap;
                AnimatePress(x, yms, w, h, labels[current]);
                
                if (current == 0) {
                    // Chuyen menu nhanh, khong delay
                    system("cls");
                    return;
                } else {
                    system("cls");
                    SetColor(7);
                    cout << "\nTam biet! Hen gap lai.";
                    exit(0);
                }
            }
        }
    }
}

// ============================================
// MENU CHINH
// ============================================
int SetMenu() {
    ShowCur(0);
    system("cls");
    VeKhungChinh();
    
    // Can trai menu, mo rong khung
    int x = 10;  // Can trai hon
    int y = 4;
    int w = 40;  // Mo rong khung tu 28 -> 40
    int h = MenuConfig::MENU_HEIGHT;
    int spacing = MenuConfig::MENU_SPACING;
    
    // Tieu de
    SetColor(MenuConfig::COLOR_TITLE);
    gotoXY(x + 8, y - 2);
    cout << "===== MENU CHINH =====";
    
    string menu[7] = {
        "1. Them moi ho so",
        "2. In danh sach",
        "3. Sap xep",
        "4. Tim kiem",
        "5. Thong Ke",
        "6. Thi Cuoi Ky",
        "7. Thoat"
    };
    
    // Ve cac muc menu - Can trai
    for (int i = 0; i < 7; i++) {
        VeOMenuCanTrai(x, y + i * spacing, w, h, MenuConfig::COLOR_BORDER, MenuConfig::COLOR_BG_NORMAL, menu[i]);
    }
    
    VeKhungHuongDan();
    
    // Xu ly chon - Su dung HighlightOMenuFullCanTrai
    int current = 0;
    HighlightOMenuFullCanTrai(x, y, w, h, MenuConfig::COLOR_BG_HOVER, menu[0]);
    
    while (true) {
        if (_kbhit()) {
            char c = _getch();
            if (c == -32) {
                c = _getch();
                int old = current;
                
                if (c == 72 && current > 0) current--; // Len
                if (c == 80 && current < 5) current++; // Xuong
                
                if (old != current) {
                    // Xoa highlight cu
                    VeOMenuCanTrai(x, y + old * spacing, w, h, MenuConfig::COLOR_BORDER, MenuConfig::COLOR_BG_NORMAL, menu[old]);
                    
                    // Highlight moi - Boi mau xanh duong
                    HighlightOMenuFullCanTrai(x, y + current * spacing, w, h, MenuConfig::COLOR_BG_HOVER, menu[current]);
                }
            }
            else if (c == 13) { // Enter
                AnimatePress(x, y + current * spacing, w, h, menu[current]);
                
                // Cap nhat breadcrumb
                if (current < 6) {
                    CapNhatBreadcrumb(menu[current]);
                }
                
                return current + 1;
            }
            else if (c == 27) { // ESC
                return 7;
            }
        }
    }
}

// ============================================
// MENU CON CHUNG (Toi uu hoa)
// ============================================
int MenuConChung(string tieu_de, string menu[], int n, string breadcrumb_item = "") {
    system("cls");
    VeKhungChinh();
    VeKhungHuongDan();
    
    // Can trai menu con, mo rong khung, di chuyen len cao
    int x = 10, y = 3, w = 40, h = MenuConfig::MENU_HEIGHT;  // Giam y tu 6 -> 3
    int spacing = 2;  // Giam spacing tu 3 -> 2
    
    // Tieu de
    SetColor(MenuConfig::COLOR_TITLE);
    gotoXY(x + 8, y - 1);
    cout << tieu_de;
    
    // Ve cac muc - Can trai
    for (int i = 0; i < n; i++) {
        VeOMenuCanTrai(x, y + 1 + i * spacing, w, h, MenuConfig::COLOR_BORDER, MenuConfig::COLOR_BG_NORMAL, menu[i]);
    }
    
    int current = 0;
    HighlightOMenuFullCanTrai(x, y + 1, w, h, MenuConfig::COLOR_BG_HOVER, menu[0]);
    
    while (true) {
        if (_kbhit()) {
            char c = _getch();
            if (c == -32) {
                c = _getch();
                int old = current;
                
                if (c == 72 && current > 0) current--;
                if (c == 80 && current < n - 1) current++;
                
                if (old != current) {
                    VeOMenuCanTrai(x, y + 1 + old * spacing, w, h, MenuConfig::COLOR_BORDER, MenuConfig::COLOR_BG_NORMAL, menu[old]);
                    HighlightOMenuFullCanTrai(x, y + 1 + current * spacing, w, h, MenuConfig::COLOR_BG_HOVER, menu[current]);
                }
            }
            else if (c == 13) {
                AnimatePress(x, y + 1 + current * spacing, w, h, menu[current]);
                
                // Cap nhat breadcrumb neu khong phai nut "Quay lai"
                if (!breadcrumb_item.empty() && current < n - 1) {
                    CapNhatBreadcrumb(menu[current]);
                }
                
                return current + 1;
            }
            else if (c == 27) {
                return n;
            }
        }
    }
}

int MenuSapXep() {
    string menu[] = {"1. Selection Sort", "2. Insertion Sort", "3. Heap Sort", "4. Quick Sort", "5. Quay lai"};
    return MenuConChung("===== CHON THUAT TOAN SAP XEP =====", menu, 5, "Sap xep");
}

int MenuChonKhoa() {
    string menu[] = {
        "1. Ma The Loai", 
        "2. Ma Sach (ISBN)", 
        "3. Ten Sach", 
        "4. Tac Gia", 
        "5. Ngay Nhap Kho", 
        "6. Gia Sach", 
        "7. Da Tieu Chi (TL->Ten->Ngay)", 
        "8. Quay lai"
    };
    return MenuConChung("===== CHON KHOA SAP XEP =====", menu, 8, "Khoa");
}


int MenuTimKiem() {
	string menu[] = {"1. Tuan tu (Linear)", "2. Nhi phan (Binary)", "3. Quay lai"};
	return MenuConChung("===== CHON THUAT TOAN TIM KIEM =====", menu, 3, "Tim kiem");
}

int MenuChonKhoaTim() {
	string menu[] = {"1. Ma The Loai", "2. Ma Sach", "3. Ten Sach", "4. Tac Gia"};
	return MenuConChung("===== CHON KHOA TIM KIEM =====", menu, 4, "Khoa tim");
}

int MenuThongKe() {
	string menu[] = {"1. Theo The Loai", "2. Theo Gia Sach", "3. Theo Tac Gia", "4. Quay lai"};
	return MenuConChung("===== CHON LOAI BAO CAO =====", menu, 4, "Thong ke");
}

// ============================================
// FORM NHAP SACH (Toi uu hoa)
// ============================================
#include "Sach.h"


void VeFormNhapSach() {
    int x = 4, y = 1, w = 106, h = 2;
    
    // Tieu de
    VeOMenu(x, y, w, h, MenuConfig::COLOR_BORDER, MenuConfig::COLOR_BG_NORMAL, "NHAP THONG TIN SACH THU VIEN");
    
    // Cac nhan
    string labels[6] = {
        "Ma the loai",
        "Ma sach (ISBN)",
        "Ten sach",
        "Tac gia",
        "Ngay nhap (dd/mm/yyyy)",
        "Gia sach (VND)"
    };
    
    for (int i = 0; i < 6; i++) {
        VeOMenu(x, y + 3 + 2 * i, 30, h, MenuConfig::COLOR_BORDER, MenuConfig::COLOR_BG_NORMAL, labels[i]);
    }
    
    // Ve o nhap
    for (int i = 0; i < 6; i++) {
        VeOMenu(x + 35, y + 3 + 2 * i, 30, h, MenuConfig::COLOR_BORDER, MenuConfig::COLOR_BG_NORMAL, "");
    }
    
    // Khung huong dan
    VeKhungHuongDan();
}

// ============================================
// HIEN THI DANH SACH
// ============================================
// Include ham moi
#include "HienThiMoi.h"
#define HienThiDanhSach HienThiDanhSach_New



// ============================================
// CAC HAM LEGACY (Compatibility)
// ============================================
void box_s(int x, int y, int w, int h, int t_color, int b_color, string nd) {
    VeOMenu(x, y, w, h, t_color, b_color, nd);
}

void Thanh_sang(int x, int y, int w, int h, int b_color, string nd) {
    HighlightOMenu(x, y, w, h, b_color, nd);
}

void box(int x, int y, int w, int h, int color_t, int b_color, string nd) {
    VeOMenu(x, y, w, h, color_t, b_color, nd);
}

void n_box(int x, int y, int w, int h, int t_color, int b_color, string nd[], int n) {
    for (int i = 0; i < n; i++) {
        VeOMenu(x, y + (i * 2), w, h, t_color, b_color, nd[i]);
    }
}

void Khung() {
    VeKhungChinh();
}

void Khung_nho1() {
    // Deprecated - Now part of VeKhungHuongDan (Frame 1)
    VeKhungHuongDan();
}

void Khung_nho2() {
    // Deprecated - Now part of VeKhungHuongDan (Frame 2)
}

void Khung_nho3() {
    // Deprecated - Now part of VeKhungHuongDan (Frame 3)
}

void Khung_nho() {
    VeKhungHuongDan();
}

#endif
