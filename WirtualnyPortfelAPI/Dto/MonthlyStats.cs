namespace WirtualnyPortfelAPI.Dto
{
    public class MonthlyStats
    {
        public decimal CurrentMonthlyTotal { get; set; }
        public decimal YearlyTotal { get; set; }
        public int SubscriptionsCount { get; set; }
        public Dictionary<string, decimal> ByCategory { get; set; } = new();
        public List<MonthTrend> Trend { get; set; } = new();
    }

    public class MonthTrend
    {
        public string Month { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}