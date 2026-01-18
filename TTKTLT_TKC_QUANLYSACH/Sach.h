#ifndef SACH_H
#define SACH_H
#include "Library.h"
#include "Date.h"

#include <vector>
#include <fstream>
#include <string>
#include <cstdlib> // Cho atoi
#include <iostream> // [FIX] QUAN TRONG: Them iostream de dung cerr, ios::in
#include <cctype>  // Cho isdigit

using namespace std;

// [FIX] Ham ho tro chuyen doi string sang long long thu cong (Chay duoc tren moi trinh bien dich C++)
long long StringToLong(string s) {
    long long res = 0;
    for (size_t i = 0; i < s.length(); i++) {
        if (isdigit(s[i])) res = res * 10 + (s[i] - '0');
    }
    return res;
}

// --- CLASS SACH ---
class Sach
{
	private:
		string maTheLoai;
		string tenTheLoai;  // THEM MOI: Ten day du cua the loai
		string maSach; // ISBN
		string tenSach;
		string tacGia;
		Date ngayNhapKho;
		long long giaSach; 
	public:
		string cach; 
		
		Sach(){
			this->maSach = "";
			this->maTheLoai = "";
			this->tenTheLoai = "";  // THEM MOI
			tenSach = "";
			tacGia = "";
			this->giaSach = 0;
			this->cach = "====================";
		}
		Sach(string maTL, string tenTL, string maS, string Ten, string TG, Date NNK, long long gia)
		{
			this->maSach = maS;
			this->maTheLoai = maTL;
			this->tenTheLoai = tenTL;  // THEM MOI
			tenSach = Ten;
			tacGia = TG;
			ngayNhapKho = NNK;
			this->giaSach = gia;
			this->cach = "====================";
		}
		
		// --- Setters ---
		void SetMaTheLoai(string MaTL) { this->maTheLoai = MaTL; }
		void SetTenTheLoai(string TenTL) { this->tenTheLoai = TenTL; }  // THEM MOI
		void SetMaSach(string MaS) { this->maSach = MaS; }
		void SetTenSach(string Ten) { this->tenSach = Ten; }
		void SetTacGia(string TG) { this->tacGia = TG; }		
		void SetNgayNhapKho(Date NNK) { this->ngayNhapKho = NNK; }			
		void SetGiaSach(long long Gia) { this->giaSach = Gia; }
		
		// --- Getters ---
		string GetMaTheLoai() { return maTheLoai; }
		string GetTenTheLoai() { return tenTheLoai; }  // THEM MOI
		string GetMaSach() { return maSach; }
		string GetTenSach() { return tenSach; }
		string GetTacGia() { return this->tacGia; }
		Date GetNgayNhapKho() { return this->ngayNhapKho; }
		long long GetGiaSach() { return giaSach; }
		
		// --- Ghi 1 cuon sach vao file BINARY (Append mode) ---
		void GhiSachVaoFile()
		{
			ofstream file("DSSach.dat", ios::binary | ios::app); 
			if (!file)
			{
				gotoXY(60, 12);
				cerr << "Khong mo duoc file" << endl;
				return;
			}
			
			// Ghi do dai va noi dung cua cac string
			size_t len;
			
			len = maTheLoai.length();
			file.write((char*)&len, sizeof(len));
			file.write(maTheLoai.c_str(), len);
			
			len = tenTheLoai.length();
			file.write((char*)&len, sizeof(len));
			file.write(tenTheLoai.c_str(), len);
			
			len = maSach.length();
			file.write((char*)&len, sizeof(len));
			file.write(maSach.c_str(), len);
			
			len = tenSach.length();
			file.write((char*)&len, sizeof(len));
			file.write(tenSach.c_str(), len);
			
			len = tacGia.length();
			file.write((char*)&len, sizeof(len));
			file.write(tacGia.c_str(), len);
			
			// Ghi Date (3 int)
			int d = ngayNhapKho.GetNgay();
			int m = ngayNhapKho.GetThang();
			int y = ngayNhapKho.GetNam();
			file.write((char*)&d, sizeof(d));
			file.write((char*)&m, sizeof(m));
			file.write((char*)&y, sizeof(y));
			
			// Ghi gia sach
			file.write((char*)&giaSach, sizeof(giaSach));
			
			file.close();
			return;
		}
};

// --- CLASS QUAN LY DANH SACH SACH ---
class DanhSachSach
{
	private:
		vector <Sach> DS;
		vector <string> arrMaTL;
		vector <string> arrMaSach;
		vector <string> arrTenSach;
		vector <string> arrTacGia;
		vector <Date> arrDate;
		vector <long long> arrGia;
		int SoLuong;
	public:
		DanhSachSach()
		{
			this->SoLuong = 0;
		}
		void InsertSach(Sach s)
		{
			DS.push_back(s);
			SoLuong++;
		}
		bool IsDSnull()
		{
			return DS.empty();
		}
		
		// Cap nhat cac mang phu tro
		void UpdateArrays()
		{
			arrMaTL.clear(); arrMaSach.clear(); arrTenSach.clear(); 
			arrTacGia.clear(); arrDate.clear(); arrGia.clear();
			
			for (size_t k = 0; k < DS.size(); k++)
			{
				arrMaTL.push_back(DS[k].GetMaTheLoai());
				arrMaSach.push_back(DS[k].GetMaSach());
				arrTenSach.push_back(DS[k].GetTenSach());
				arrTacGia.push_back(DS[k].GetTacGia());
				arrDate.push_back(DS[k].GetNgayNhapKho());
				arrGia.push_back(DS[k].GetGiaSach());
			}
		}
		
		// Doc du lieu tu file BINARY
		void DocFileSach(const string Tenfile)
		{
			ifstream in(Tenfile.c_str(), ios::binary);
			if (!in)
			{
				// Tao file rong neu chua ton tai
				ofstream out(Tenfile.c_str(), ios::binary);
				out.close();
				return; 
			}
			
			while(in.peek() != EOF)
			{
				size_t len;
				
				// Doc maTheLoai
				if (!in.read((char*)&len, sizeof(len))) break;
				char* buffer = new char[len + 1];
				in.read(buffer, len);
				buffer[len] = '\0';
				string MaTL(buffer);
				delete[] buffer;
				
				// Doc tenTheLoai
				in.read((char*)&len, sizeof(len));
				buffer = new char[len + 1];
				in.read(buffer, len);
				buffer[len] = '\0';
				string TenTL(buffer);
				delete[] buffer;
				
				// Doc maSach
				in.read((char*)&len, sizeof(len));
				buffer = new char[len + 1];
				in.read(buffer, len);
				buffer[len] = '\0';
				string MaS(buffer);
				delete[] buffer;
				
				// Doc tenSach
				in.read((char*)&len, sizeof(len));
				buffer = new char[len + 1];
				in.read(buffer, len);
				buffer[len] = '\0';
				string TenS(buffer);
				delete[] buffer;
				
				// Doc tacGia
				in.read((char*)&len, sizeof(len));
				buffer = new char[len + 1];
				in.read(buffer, len);
				buffer[len] = '\0';
				string TG(buffer);
				delete[] buffer;
				
				// Doc Date
				int d, m, y;
				in.read((char*)&d, sizeof(d));
				in.read((char*)&m, sizeof(m));
				in.read((char*)&y, sizeof(y));
				Date d1(d, m, y);
				
				// Doc gia sach
				long long gia;
				in.read((char*)&gia, sizeof(gia));
				
				Sach s(MaTL, TenTL, MaS, TenS, TG, d1, gia);
				InsertSach(s);
			}
			in.close();
		}
		
		// Ghi toan bo danh sach vao file BINARY (overwrite mode)
		void GhiDanhSachVaoFile(const string Tenfile) {
			ofstream out(Tenfile.c_str(), ios::binary | ios::out); // Binary overwrite mode
			if (!out) {
				cerr << "Khong mo duoc file de ghi!" << endl;
				return;
			}
			
			for(size_t i = 0; i < DS.size(); i++) {
				size_t len;
				
				// Ghi maTheLoai
				string maTL = DS[i].GetMaTheLoai();
				len = maTL.length();
				out.write((char*)&len, sizeof(len));
				out.write(maTL.c_str(), len);
				
				// Ghi tenTheLoai
				string tenTL = DS[i].GetTenTheLoai();
				len = tenTL.length();
				out.write((char*)&len, sizeof(len));
				out.write(tenTL.c_str(), len);
				
				// Ghi maSach
				string maS = DS[i].GetMaSach();
				len = maS.length();
				out.write((char*)&len, sizeof(len));
				out.write(maS.c_str(), len);
				
				// Ghi tenSach
				string tenS = DS[i].GetTenSach();
				len = tenS.length();
				out.write((char*)&len, sizeof(len));
				out.write(tenS.c_str(), len);
				
				// Ghi tacGia
				string tg = DS[i].GetTacGia();
				len = tg.length();
				out.write((char*)&len, sizeof(len));
				out.write(tg.c_str(), len);
				
				// Ghi Date
				Date ngay = DS[i].GetNgayNhapKho();
				int d = ngay.GetNgay();
				int m = ngay.GetThang();
				int y = ngay.GetNam();
				out.write((char*)&d, sizeof(d));
				out.write((char*)&m, sizeof(m));
				out.write((char*)&y, sizeof(y));
				
				// Ghi gia sach
				long long gia = DS[i].GetGiaSach();
				out.write((char*)&gia, sizeof(gia));
			}
			out.close();
		}
		
		// --- SORTING ---
		// Helper to compare two books based on key
		// key: 0-MaTheLoai, 1-MaSach, 2-TenSach, 3-TacGia, 4-NgayNhapKho, 5-GiaSach, 6-DaTieuChi
		bool Compare(Sach a, Sach b, int key) {
			switch(key) {
				case 0: return a.GetMaTheLoai() < b.GetMaTheLoai(); // THEM MOI: Ma The Loai
				case 1: return a.GetMaSach() < b.GetMaSach();
				case 2: return a.GetTenSach() < b.GetTenSach();
				case 3: return a.GetTacGia() < b.GetTacGia();
				case 4: {
					Date d1 = a.GetNgayNhapKho();
					Date d2 = b.GetNgayNhapKho();
					if (d1.GetNam() != d2.GetNam()) return d1.GetNam() < d2.GetNam();
					if (d1.GetThang() != d2.GetThang()) return d1.GetThang() < d2.GetThang();
					return d1.GetNgay() < d2.GetNgay();
				}
				case 5: return a.GetGiaSach() < b.GetGiaSach();
				case 6: { // THEM MOI: Sap xep da tieu chi (Ma The Loai -> Ten Sach -> Ngay Nhap)
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
				default: return false;
			}
		}

		void SelectionSort(int key) {
			int n = DS.size();
			for (int i = 0; i < n - 1; i++) {
				int min_idx = i;
				for (int j = i + 1; j < n; j++) {
					if (Compare(DS[j], DS[min_idx], key)) {
						min_idx = j;
					}
				}
				swap(DS[min_idx], DS[i]);
			}
		}

		void InsertionSort(int key) {
			int n = DS.size();
			for (int i = 1; i < n; i++) {
				Sach keyItem = DS[i];
				int j = i - 1;
				while (j >= 0 && Compare(keyItem, DS[j], key)) {
					DS[j + 1] = DS[j];
					j = j - 1;
				}
				DS[j + 1] = keyItem;
			}
		}
		
		// THEM MOI: Heap Sort - Sap xep vun dong
		void Heapify(int n, int i, int key) {
			int largest = i;
			int left = 2 * i + 1;
			int right = 2 * i + 2;
			
			if (left < n && !Compare(DS[largest], DS[left], key)) {
				largest = left;
			}
			if (right < n && !Compare(DS[largest], DS[right], key)) {
				largest = right;
			}
			
			if (largest != i) {
				swap(DS[i], DS[largest]);
				Heapify(n, largest, key);
			}
		}
		
		void HeapSort(int key) {
			int n = DS.size();
			// Xay dung heap (sap xep lai mang)
			for (int i = n / 2 - 1; i >= 0; i--) {
				Heapify(n, i, key);
			}
			// Trich xuat tung phan tu tu heap
			for (int i = n - 1; i > 0; i--) {
				swap(DS[0], DS[i]);
				Heapify(i, 0, key);
			}
		}

		int Partition(int low, int high, int key) {
			Sach pivot = DS[high];
			int i = (low - 1);
			for (int j = low; j <= high - 1; j++) {
				if (Compare(DS[j], pivot, key)) {
					i++;
					swap(DS[i], DS[j]);
				}
			}
			swap(DS[i + 1], DS[high]);
			return (i + 1);
		}

		void QuickSort(int low, int high, int key) {
			if (low < high) {
				int pi = Partition(low, high, key);
				QuickSort(low, pi - 1, key);
				QuickSort(pi + 1, high, key);
			}
		}
		
		void QuickSortWrapper(int key) {
			if (!DS.empty()) QuickSort(0, DS.size() - 1, key);
		}

		void Merge(int l, int m, int r, int key) {
			int n1 = m - l + 1;
			int n2 = r - m;
			vector<Sach> L(n1), R(n2);
			for (int i = 0; i < n1; i++) L[i] = DS[l + i];
			for (int j = 0; j < n2; j++) R[j] = DS[m + 1 + j];
			int i = 0, j = 0, k = l;
			while (i < n1 && j < n2) {
				if (Compare(L[i], R[j], key) || (!Compare(R[j], L[i], key) && !Compare(L[i], R[j], key))) { // <=
					DS[k] = L[i]; i++;
				} else {
					DS[k] = R[j]; j++;
				}
				k++;
			}
			while (i < n1) { DS[k] = L[i]; i++; k++; }
			while (j < n2) { DS[k] = R[j]; j++; k++; }
		}

		void MergeSort(int l, int r, int key) {
			if (l < r) {
				int m = l + (r - l) / 2;
				MergeSort(l, m, key);
				MergeSort(m + 1, r, key);
				Merge(l, m, r, key);
			}
		}
		
		void MergeSortWrapper(int key) {
			if (!DS.empty()) MergeSort(0, DS.size() - 1, key);
		}

		// --- SEARCHING ---
		// keyType: 1-MaTheLoai, 2-MaSach, 3-TenSach, 4-TacGia
		vector<Sach> LinearSearch(int keyType, string value) {
			vector<Sach> results;
			for (size_t i = 0; i < DS.size(); i++) {
				bool match = false;
				switch(keyType) {
					case 1: if (DS[i].GetMaTheLoai() == value) match = true; break;
					case 2: if (DS[i].GetMaSach() == value) match = true; break;
					case 3: if (DS[i].GetTenSach().find(value) != string::npos) match = true; break; // Partial match
					case 4: if (DS[i].GetTacGia().find(value) != string::npos) match = true; break; // Partial match
				}
				if (match) results.push_back(DS[i]);
			}
			return results;
		}

		// Binary Search requires sorted list by the same key. 
		// Here we assume list is sorted by the key we are searching for.
		// Only exact match for Binary Search.
		int BinarySearch(int keyType, string value) {
			int l = 0, r = DS.size() - 1;
			while (l <= r) {
				int m = l + (r - l) / 2;
				string currentVal;
				switch(keyType) {
					case 1: currentVal = DS[m].GetMaTheLoai(); break;
					case 2: currentVal = DS[m].GetMaSach(); break;
					case 3: currentVal = DS[m].GetTenSach(); break;
					case 4: currentVal = DS[m].GetTacGia(); break;
				}
				if (currentVal == value) return m;
				if (currentVal < value) l = m + 1;
				else r = m - 1;
			}
			return -1;
		}

		// --- STATISTICS ---
		// Count by Category
		void ThongKeTheoTheLoai() {
			// Simple map implementation using parallel vectors since we can't use std::map easily in all envs or want to keep it simple
			vector<string> categories;
			vector<int> counts;
			
			for(size_t i=0; i<DS.size(); i++) {
				string cat = DS[i].GetMaTheLoai();
				bool found = false;
				for(size_t j=0; j<categories.size(); j++) {
					if(categories[j] == cat) {
						counts[j]++;
						found = true;
						break;
					}
				}
				if(!found) {
					categories.push_back(cat);
					counts.push_back(1);
				}
			}
			
			// Print Report
			cout << "   BAO CAO SO LUONG SACH THEO THE LOAI" << endl;
			cout << "   -----------------------------------" << endl;
			for(size_t i=0; i<categories.size(); i++) {
				cout << "   The loai: " << categories[i] << " - So luong: " << counts[i] << endl;
			}
		}
		
		// Stats by Price Range
		void ThongKeTheoGia() {
			int range1 = 0; // < 50k
			int range2 = 0; // 50k - 100k
			int range3 = 0; // > 100k
			
			for(size_t i=0; i<DS.size(); i++) {
				long long price = DS[i].GetGiaSach();
				if(price < 50000) range1++;
				else if(price <= 100000) range2++;
				else range3++;
			}
			
			int total = DS.size();
			double percent1 = (total > 0) ? (range1 * 100.0 / total) : 0;
			double percent2 = (total > 0) ? (range2 * 100.0 / total) : 0;
			double percent3 = (total > 0) ? (range3 * 100.0 / total) : 0;
			
			cout << "   BAO CAO PHAN LOAI THEO GIA SACH" << endl;
			cout << "   -------------------------------" << endl;
			cout << "   Tong so sach: " << total << endl << endl;
			cout << "   Duoi 50,000 VND:         " << range1 << " cuon (" << percent1 << "%)" << endl;
			cout << "   Tu 50,000 - 100,000 VND: " << range2 << " cuon (" << percent2 << "%)" << endl;
			cout << "   Tren 100,000 VND:        " << range3 << " cuon (" << percent3 << "%)" << endl;
		}

		vector<Sach> GetList() { return DS; }
};

// --- CAC HAM TIEN ICH ---

string ChuanHoaTen(string str) 
{
	for (int i = 0; i < str.length(); i++)
	{
		if (i == 0) 
		{
			if (str[i] > 96) str[i] -= 32;
		}
		else if (str[i-1] == ' ' && str[i] != ' ')
		{
			if (str[i] > 96) str[i] -= 32;
		}
		else if (str[i] != ' ' && str[i] < 97)
		{
			str[i] += 32;
		}
	}
	return str; 
}

bool IsValidString(string s)
{
	for (int i = 0; i < s.length(); i++)
	{
		bool tg1 = (s[i] >= 65) && (s[i] < 65 + 26);
		bool tg2 = (s[i] >= 97) && (s[i] < 97 + 26);
		bool tg3 = (s[i] == ' ' || s[i] == '-' || s[i] == '.' || isdigit(s[i])); 
		if (!(tg1 || tg2 || tg3)) return false;
	}
	return true;
}

bool isValidIntegerString(const std::string& s) {
    if (s.empty()) return false;
    size_t i = 0;
    if (s[0] == '-') {
        if (s.size() == 1) return false;
        i = 1;
    }
    for (; i < s.size(); ++i) {
        if (!isdigit(s[i])) return false;
    }
    return true;
}

bool getLongLongFromInput(long long& outValue) { 
    string input;
    cin >> input;
    if (!isValidIntegerString(input)) return false;
    
    // [FIX] C++98: Dung ham tu viet, khong dung stringstream de tranh loi
    outValue = StringToLong(input);
    return true;
}

bool isTwoDigitNumber(string input, int& outNumber) {
    if (input.length() != 2) return false;
    if (!isdigit(input[0]) || !isdigit(input[1])) return false;
    outNumber = (input[0] - '0') * 10 + (input[1] - '0');
    return true;
}

bool IsfourDigitnumber(string input, int & outNumber)
{
	if (input.length()!=4) return false;
	if (!isdigit(input[0]) || !isdigit(input[1])||!isdigit(input[2]) || !isdigit(input[3])) return false;
	outNumber = (input[0] - '0') * 1000 + (input[1] - '0')*100 + (input[2] - '0') * 10 + (input[3] - '0');
	return true;
}

// Kiem tra ma sach (ISBN) phai dung 8 chu so
bool IsValidISBN(string isbn) {
	if (isbn.length() != 8) return false;
	for (int i = 0; i < 8; i++) {
		if (!isdigit(isbn[i])) return false;
	}
	return true;
}


// --- HAM GIAO DIEN NHAP SACH ---
// --- Moved NhapSach to UI layer ---
#endif
