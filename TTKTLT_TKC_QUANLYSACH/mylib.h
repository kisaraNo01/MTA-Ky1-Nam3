#pragma once // tranh trung lap thu vien khi goi nhieu file

#include <stdio.h>
#include <conio.h>
#include <ctime>
#include <windows.h>

//================ LAY TOA DO X CUA CON TRO ================
int whereX()
{
    CONSOLE_SCREEN_BUFFER_INFO csbi;
    if (GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE), &csbi))
        return csbi.dwCursorPosition.X;
    return -1;
}

//================ LAY TOA DO Y CUA CON TRO ================
int whereY()
{
    CONSOLE_SCREEN_BUFFER_INFO csbi;
    if (GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE), &csbi))
        return csbi.dwCursorPosition.Y;
    return -1;
}

//================ DI CHUYEN CON TRO ================
void gotoXY(int x, int y)
{
    HANDLE hConsoleOutput;
    COORD Cursor_Pos = { (SHORT)x, (SHORT)y };
    hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleCursorPosition(hConsoleOutput, Cursor_Pos);
}

//================ DAT MAU CHU ================
void SetColor(WORD color)
{
    HANDLE hConsoleOutput;
    hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);

    CONSOLE_SCREEN_BUFFER_INFO screen_buffer_info;
    GetConsoleScreenBufferInfo(hConsoleOutput, &screen_buffer_info);

    WORD wAttributes = screen_buffer_info.wAttributes;
    color &= 0x000f;
    wAttributes &= 0xfff0;
    wAttributes |= color;

    SetConsoleTextAttribute(hConsoleOutput, wAttributes);
}

//================ AN / HIEN CON TRO ================
void ShowCur(bool CursorVisibility)
{
    HANDLE handle = GetStdHandle(STD_OUTPUT_HANDLE);
    CONSOLE_CURSOR_INFO cursor = { 1, CursorVisibility };
    SetConsoleCursorInfo(handle, &cursor);
}

//================ NHAN PHIM TU BAN PHIM ================
#define KEY_NONE -1

int inputKey()
{
    if (_kbhit())
    {
        int key = _getch();

        if (key == 224) // phim mui ten
        {
            key = _getch();
            return key + 1000; // quy uoc phim mui ten
        }

        return key;
    }
    return KEY_NONE;
}

//================ DOI MAU CHU ================
void textcolor(int x)
{
    HANDLE mau;
    mau = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleTextAttribute(mau, x);
}

//================ SET MAU CHU + MAU NEN ================
void setConsoleColor(int textColor, int bgColor)
{
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleTextAttribute(hConsole, (bgColor << 4) | textColor);
}

//================ AN CON TRO ============================
void hideCursor()
{
    CONSOLE_CURSOR_INFO cursorInfo;
    cursorInfo.dwSize = 1;
    cursorInfo.bVisible = FALSE;
    SetConsoleCursorInfo(GetStdHandle(STD_OUTPUT_HANDLE), &cursorInfo);
}

//================ HIEN CON TRO ==========================
void showCursor()
{
    CONSOLE_CURSOR_INFO cursorInfo;
    cursorInfo.dwSize = 1;
    cursorInfo.bVisible = TRUE;
    SetConsoleCursorInfo(GetStdHandle(STD_OUTPUT_HANDLE), &cursorInfo);
}

//================ GIA LAP NHAP NHAY CON TRO (C++98) =====
void blinkCursor(int x, int y)
{
    hideCursor();
    for (int i = 0; i < 5; i++)
    {
        gotoXY(x, y);
        printf("%c", 219); // khoi vuong
        Sleep(300);

        gotoXY(x, y);
        printf(" ");
        Sleep(300);
    }
    showCursor();
}

