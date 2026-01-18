#ifndef DATE_H
#define DATE_H
#include"Library.h"

class Date
{
	private:
		int ngay;
		int thang;
		int nam;
	public:
		Date()
		{
			this->ngay = 0;
			thang = 0;
			nam = 0; 
		}
		Date(int ngay,int thang,int nam)
		{
			this->ngay = ngay;
			this->thang = thang;
			this->nam = nam;
		}
		
		void SetNgay(int Ngay)
		{
			ngay = Ngay;
		}
		
		void SetThang(int Thang)
		{
			thang = Thang;
		}
		
		void SetNam(int Nam)
		{
			nam = Nam;
		}
		
		int GetNgay()
		{
			return ngay;
		}
		
		int GetThang()
		{
			return thang;
		}
		
		int GetNam()
		{
			return nam;
		}
		static bool Kiemtra(int d,int m,int y)
		{
			if (d<1) return false;
			switch(m)
			{
				case 1:
				case 3:
				case 5:
				case 7:
				case 8:
				case 10:
				case 12:
					if (d>31) return false;
					break;
				case 4:
				case 6:
				case 9:
				case 11:
					if (d>30) return false;
					break;
				case 2:
					if ((y%4==0&&y%100!=0)||y%400==0)
					{
						if (d>29) return false;
					}
					else if (d>28) return false;
					break;
				default:
					return false;
					break;
				
			}
			// Kiem tra ngay phai SAU 01/01/2000 (khong bao gom 01/01/2000)
			if (y < 2000) return false;
			if (y == 2000 && m == 1 && d <= 1) return false; // Loai tru 01/01/2000
			
			// Lay ngay hien tai
			time_t now = time(0);
			tm* ltm = localtime(&now);
			int currentYear = 1900 + ltm->tm_year;
			int currentMonth = 1 + ltm->tm_mon;
			int currentDay = ltm->tm_mday;
			
			// Kiem tra khong vuot qua ngay hien tai
			if (y > currentYear) return false;
			if (y == currentYear && m > currentMonth) return false;
			if (y == currentYear && m == currentMonth && d > currentDay) return false;
			
			return true;
		}
		
		friend istream &operator >>(istream &i, Date &d1)
		{
			i>> d1.ngay>>d1.thang>>d1.nam;
			return i;
		}
		
		friend ostream &operator <<(ostream &o,Date d1)
		{
			
			if (d1.ngay >=10 && d1.thang >=10 ) o<<d1.ngay<<"/"<<d1.thang<<"/"<<d1.nam;
			else if (d1.ngay <10 && d1.thang >=10 ) o<<"0"<<d1.ngay<<"/"<<d1.thang<<"/"<<d1.nam;
			else if (d1.ngay >=10 && d1.thang <10 ) o<<d1.ngay<<"/0"<<d1.thang<<"/"<<d1.nam;
			else o<<"0"<<d1.ngay<<"/0"<<d1.thang<<"/"<<d1.nam;
			return o;
		}
	
};

#endif
