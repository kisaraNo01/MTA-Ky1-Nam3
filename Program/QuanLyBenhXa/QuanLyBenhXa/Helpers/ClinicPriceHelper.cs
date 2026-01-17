namespace QuanLyBenhXa.Helpers
{
    public static class ClinicPriceHelper
    {
        public static Dictionary<string, decimal> Prices = new Dictionary<string, decimal>
        {
            { "Đa khoa", 50000m },
            { "Tim mạch", 150000m },
            { "Da liễu", 100000m },
            { "Xét nghiệm máu", 80000m },
            { "Siêu âm", 120000m }
        };

        public static decimal GetPrice(string clinicName)
        {
            if (Prices.ContainsKey(clinicName))
            {
                return Prices[clinicName];
            }
            return 0;
        }
    }
}
