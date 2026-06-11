namespace WirtualnyPortfelAPI.Dto
{
    public class UpcomingPaymentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "PLN";
        public DateTime RenewalDate { get; set; }
        public int DaysUntil => (int)Math.Ceiling((RenewalDate - DateTime.UtcNow).TotalDays);
        public string? CategoryName { get; set; }
    }
}
