namespace SHOESTKC.Models
{
    public class UpdateProfileRequest
    {
        public string HoTen { get; set; } = null!;
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
