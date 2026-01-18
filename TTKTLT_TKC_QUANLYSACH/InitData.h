// Ham tao du lieu mau cho file binary
void TaoDuLieuMau() {
    DanhSachSach dss;
    
    // Tao 70 cuon sach mau - Du lieu da dang cho thong ke dep
    
    // TRI TUE NHAN TAO (6 cuon)
    Sach s1("TL001", "Tri Tue Nhan Tao", "82343478", "Deep Learning", "Ian Goodfellow", Date(12, 7, 2024), 350000);
    Sach s2("TL001", "Tri Tue Nhan Tao", "12345678", "Machine Learning", "Andrew Ng", Date(15, 6, 2024), 280000);
    Sach s3("TL001", "Tri Tue Nhan Tao", "23456789", "Neural Networks", "Michael Nielsen", Date(20, 8, 2024), 320000);
    Sach s4("TL001", "Tri Tue Nhan Tao", "34567890", "AI For Everyone", "Andrew Ng", Date(5, 9, 2023), 180000);
    Sach s5("TL001", "Tri Tue Nhan Tao", "45678901", "Computer Vision", "Richard Szeliski", Date(18, 4, 2024), 395000);
    Sach s6("TL001", "Tri Tue Nhan Tao", "56789012", "Natural Language Processing", "Dan Jurafsky", Date(22, 11, 2023), 410000);
    
    // LAP TRINH (8 cuon)
    Sach s7("TL002", "Lap Trinh", "87654321", "Clean Code", "Robert Martin", Date(20, 3, 2023), 195000);
    Sach s8("TL002", "Lap Trinh", "11223344", "Design Patterns", "Gang Of Four", Date(10, 1, 2025), 420000);
    Sach s9("TL002", "Lap Trinh", "67890123", "The Pragmatic Programmer", "Andy Hunt", Date(14, 7, 2024), 245000);
    Sach s10("TL002", "Lap Trinh", "78901234", "Code Complete", "Steve McConnell", Date(9, 2, 2024), 385000);
    Sach s11("TL002", "Lap Trinh", "89012345", "Refactoring", "Martin Fowler", Date(25, 5, 2023), 275000);
    Sach s12("TL002", "Lap Trinh", "90123456", "Head First Java", "Kathy Sierra", Date(3, 10, 2024), 165000);
    Sach s13("TL002", "Lap Trinh", "01234567", "Python Crash Course", "Eric Matthes", Date(17, 12, 2023), 155000);
    Sach s14("TL002", "Lap Trinh", "12340987", "JavaScript The Good Parts", "Douglas Crockford", Date(28, 6, 2024), 135000);
    
    // VAN HOC (7 cuon)
    Sach s15("TL003", "Van Hoc", "99887766", "Truyen Kieu", "Nguyen Du", Date(5, 2, 2024), 85000);
    Sach s16("TL003", "Van Hoc", "55443322", "Nhat Ky Trong Tu", "Ho Chi Minh", Date(8, 9, 2023), 65000);
    Sach s17("TL003", "Van Hoc", "23450987", "So Do", "Vu Trong Phung", Date(19, 4, 2024), 95000);
    Sach s18("TL003", "Van Hoc", "34561098", "Lao Hac", "Nam Cao", Date(11, 11, 2023), 55000);
    Sach s19("TL003", "Van Hoc", "45672109", "Chi Pheo", "Nam Cao", Date(7, 8, 2024), 50000);
    Sach s20("TL003", "Van Hoc", "56783210", "Vo Nhat", "Nam Cao", Date(23, 1, 2024), 60000);
    Sach s21("TL003", "Van Hoc", "67894321", "Tat Den", "Ngo Tat To", Date(15, 3, 2023), 70000);
    
    // TOAN HOC (6 cuon)
    Sach s22("TL004", "Toan Hoc", "66778899", "Giai Tich", "Nguyen Van A", Date(12, 4, 2024), 125000);
    Sach s23("TL004", "Toan Hoc", "33221100", "Dai So Tuyen Tinh", "Tran Van B", Date(25, 11, 2023), 145000);
    Sach s24("TL004", "Toan Hoc", "78905432", "Xac Suat Thong Ke", "Le Thi C", Date(6, 7, 2024), 135000);
    Sach s25("TL004", "Toan Hoc", "89016543", "Toan Roi Rac", "Pham Van D", Date(18, 2, 2024), 165000);
    Sach s26("TL004", "Toan Hoc", "90127654", "Hinh Hoc Giai Tich", "Nguyen Thi E", Date(29, 9, 2023), 155000);
    Sach s27("TL004", "Toan Hoc", "01238765", "Giai Tich So", "Tran Van F", Date(13, 5, 2024), 175000);
    
    // VAT LY (5 cuon)
    Sach s28("TL005", "Vat Ly", "44556677", "Vat Ly Dai Cuong", "Le Van C", Date(18, 7, 2024), 175000);
    Sach s29("TL005", "Vat Ly", "77889900", "Co Hoc Luong Tu", "Pham Van D", Date(30, 5, 2023), 295000);
    Sach s30("TL005", "Vat Ly", "12349876", "Dien Tu Hoc", "Nguyen Van G", Date(8, 10, 2024), 215000);
    Sach s31("TL005", "Vat Ly", "23450987", "Quang Hoc", "Tran Thi H", Date(21, 3, 2024), 185000);
    Sach s32("TL005", "Vat Ly", "34561098", "Nhiet Dong Luc Hoc", "Le Van I", Date(4, 12, 2023), 225000);
    
    // HOA HOC (4 cuon)
    Sach s33("TL006", "Hoa Hoc", "22334455", "Hoa Hoc Huu Co", "Nguyen Thi E", Date(14, 8, 2024), 165000);
    Sach s34("TL006", "Hoa Hoc", "88990011", "Hoa Hoc Vo Co", "Tran Thi F", Date(22, 2, 2024), 155000);
    Sach s35("TL006", "Hoa Hoc", "45672109", "Hoa Hoc Phan Tich", "Pham Van J", Date(16, 6, 2023), 175000);
    Sach s36("TL006", "Hoa Hoc", "56783210", "Hoa Sinh Hoc", "Le Thi K", Date(27, 11, 2024), 195000);
    
    // LICH SU (6 cuon)
    Sach s37("TL007", "Lich Su", "11009988", "Lich Su Viet Nam", "Dao Duy Anh", Date(9, 10, 2023), 135000);
    Sach s38("TL007", "Lich Su", "99001122", "Lich Su The Gioi", "Nguyen Van G", Date(17, 12, 2024), 245000);
    Sach s39("TL007", "Lich Su", "67894321", "Lich Su Dang Cong San", "Ban Nghien Cuu", Date(2, 5, 2024), 115000);
    Sach s40("TL007", "Lich Su", "78905432", "Chien Tranh Viet Nam", "Tran Van L", Date(24, 8, 2023), 165000);
    Sach s41("TL007", "Lich Su", "89016543", "Cach Mang Thang Tam", "Nguyen Thi M", Date(10, 1, 2024), 125000);
    Sach s42("TL007", "Lich Su", "90127654", "Lich Su Van Minh", "Le Van N", Date(19, 7, 2024), 285000);
    
    // KINH TE (5 cuon)
    Sach s43("TL008", "Kinh Te", "55667788", "Kinh Te Hoc Vi Mo", "Nguyen Van H", Date(3, 3, 2024), 185000);
    Sach s44("TL008", "Kinh Te", "66778800", "Kinh Te Hoc Vo Mo", "Tran Van I", Date(28, 6, 2023), 195000);
    Sach s45("TL008", "Kinh Te", "01238765", "Kinh Te Phat Trien", "Pham Thi O", Date(12, 9, 2024), 215000);
    Sach s46("TL008", "Kinh Te", "12349876", "Tai Chinh Doanh Nghiep", "Le Van P", Date(5, 4, 2024), 235000);
    Sach s47("TL008", "Kinh Te", "23450987", "Marketing Can Ban", "Nguyen Thi Q", Date(26, 11, 2023), 175000);
    
    // TRIET HOC (4 cuon)
    Sach s48("TL009", "Triet Hoc", "77880099", "Triet Hoc Mac Lenin", "Ho Chi Minh", Date(11, 1, 2024), 95000);
    Sach s49("TL009", "Triet Hoc", "88991100", "Triet Hoc Dong Phuong", "Phan Van J", Date(19, 9, 2023), 115000);
    Sach s50("TL009", "Triet Hoc", "34561098", "Triet Hoc Tay Phuong", "Tran Van R", Date(7, 6, 2024), 145000);
    Sach s51("TL009", "Triet Hoc", "45672109", "Luan Ly Hoc", "Le Thi S", Date(22, 2, 2024), 105000);
    
    // TAM LY HOC (5 cuon)
    Sach s52("TL010", "Tam Ly Hoc", "00112233", "Tam Ly Hoc Dai Cuong", "Le Thi K", Date(7, 5, 2024), 145000);
    Sach s53("TL010", "Tam Ly Hoc", "11223355", "Tam Ly Hoc Tre Em", "Nguyen Thi L", Date(26, 8, 2023), 125000);
    Sach s54("TL010", "Tam Ly Hoc", "56783210", "Tam Ly Hoc Xa Hoi", "Pham Van T", Date(14, 10, 2024), 155000);
    Sach s55("TL010", "Tam Ly Hoc", "67894321", "Tam Ly Hoc Phat Trien", "Tran Thi U", Date(3, 3, 2024), 135000);
    Sach s56("TL010", "Tam Ly Hoc", "78905432", "Tam Ly Hoc Giao Duc", "Le Van V", Date(18, 12, 2023), 165000);
    
    // NGOAI NGU (4 cuon)
    Sach s57("TL011", "Ngoai Ngu", "89016543", "English Grammar In Use", "Raymond Murphy", Date(9, 4, 2024), 195000);
    Sach s58("TL011", "Ngoai Ngu", "90127654", "TOEIC Preparation", "Nguyen Van W", Date(21, 7, 2023), 175000);
    Sach s59("TL011", "Ngoai Ngu", "01238765", "IELTS Writing", "Tran Thi X", Date(15, 11, 2024), 185000);
    Sach s60("TL011", "Ngoai Ngu", "12349876", "Chinese For Beginners", "Le Van Y", Date(28, 2, 2024), 165000);
    
    // KHOA HOC XA HOI (3 cuon)
    Sach s61("TL012", "Khoa Hoc Xa Hoi", "23450987", "Xa Hoi Hoc Dai Cuong", "Pham Thi Z", Date(6, 9, 2023), 155000);
    Sach s62("TL012", "Khoa Hoc Xa Hoi", "34561098", "Nhan Hoc Van Hoa", "Nguyen Van AA", Date(19, 5, 2024), 175000);
    Sach s63("TL012", "Khoa Hoc Xa Hoi", "45672109", "Chinh Tri Hoc", "Tran Van BB", Date(11, 1, 2024), 165000);
    
    // Y HOC (3 cuon)
    Sach s64("TL013", "Y Hoc", "56783210", "Giai Phau Hoc", "Le Thi CC", Date(23, 8, 2024), 285000);
    Sach s65("TL013", "Y Hoc", "67894321", "Sinh Ly Hoc", "Pham Van DD", Date(4, 3, 2024), 265000);
    Sach s66("TL013", "Y Hoc", "78905432", "Duoc Hoc Co Ban", "Nguyen Thi EE", Date(17, 10, 2023), 295000);
    
    // CONG NGHE THONG TIN (2 cuon)
    Sach s67("TL014", "Cong Nghe Thong Tin", "89016543", "Mang May Tinh", "Tran Van FF", Date(8, 6, 2024), 215000);
    Sach s68("TL014", "Cong Nghe Thong Tin", "90127654", "An Toan Thong Tin", "Le Van GG", Date(20, 12, 2023), 245000);
    
    // NGHE THUAT (2 cuon)
    Sach s69("TL015", "Nghe Thuat", "01238765", "Lich Su Nghe Thuat", "Pham Thi HH", Date(13, 7, 2024), 175000);
    Sach s70("TL015", "Nghe Thuat", "12349876", "Am Nhac Co Dien", "Nguyen Van II", Date(25, 4, 2023), 155000);
    
    // Them tat ca sach vao danh sach
    dss.InsertSach(s1); dss.InsertSach(s2); dss.InsertSach(s3); dss.InsertSach(s4); dss.InsertSach(s5);
    dss.InsertSach(s6); dss.InsertSach(s7); dss.InsertSach(s8); dss.InsertSach(s9); dss.InsertSach(s10);
    dss.InsertSach(s11); dss.InsertSach(s12); dss.InsertSach(s13); dss.InsertSach(s14); dss.InsertSach(s15);
    dss.InsertSach(s16); dss.InsertSach(s17); dss.InsertSach(s18); dss.InsertSach(s19); dss.InsertSach(s20);
    dss.InsertSach(s21); dss.InsertSach(s22); dss.InsertSach(s23); dss.InsertSach(s24); dss.InsertSach(s25);
    dss.InsertSach(s26); dss.InsertSach(s27); dss.InsertSach(s28); dss.InsertSach(s29); dss.InsertSach(s30);
    dss.InsertSach(s31); dss.InsertSach(s32); dss.InsertSach(s33); dss.InsertSach(s34); dss.InsertSach(s35);
    dss.InsertSach(s36); dss.InsertSach(s37); dss.InsertSach(s38); dss.InsertSach(s39); dss.InsertSach(s40);
    dss.InsertSach(s41); dss.InsertSach(s42); dss.InsertSach(s43); dss.InsertSach(s44); dss.InsertSach(s45);
    dss.InsertSach(s46); dss.InsertSach(s47); dss.InsertSach(s48); dss.InsertSach(s49); dss.InsertSach(s50);
    dss.InsertSach(s51); dss.InsertSach(s52); dss.InsertSach(s53); dss.InsertSach(s54); dss.InsertSach(s55);
    dss.InsertSach(s56); dss.InsertSach(s57); dss.InsertSach(s58); dss.InsertSach(s59); dss.InsertSach(s60);
    dss.InsertSach(s61); dss.InsertSach(s62); dss.InsertSach(s63); dss.InsertSach(s64); dss.InsertSach(s65);
    dss.InsertSach(s66); dss.InsertSach(s67); dss.InsertSach(s68); dss.InsertSach(s69); dss.InsertSach(s70);
    
    // Ghi vao file binary
    dss.GhiDanhSachVaoFile("DSSach.dat");
    
    SetColor(10);
    gotoXY(35, 15);
    cout << "Da tao file DSSach.dat voi 70 ban ghi mau!";
    SetColor(15);
    Sleep(2000);
}
