#include"main.h"

#include "UI.h"

void SetupConsole() {
	// Thiet lap kich thuoc console de vua voi khung chinh (120x30)
	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
	SMALL_RECT windowSize = {0, 0, 124, 31}; // Rong 125, cao 32
	COORD bufferSize = {125, 32};
	
	SetConsoleScreenBufferSize(hConsole, bufferSize);
	SetConsoleWindowInfo(hConsole, TRUE, &windowSize);
}

int main()
{
	SetConsoleOutputCP(437);
	SetupConsole();  // Thiet lap kich thuoc console
	ManHinhChinh();	
	RunApp(); 
	
	return 0;
}
