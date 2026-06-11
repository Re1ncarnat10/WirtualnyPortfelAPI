namespace WirtualnyPortfelAPI.Models
{
    public class Subscription
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime RenewalDate { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool NotificationsEnabled { get; set; } = true;
        // Additional fields used by frontend
        public BillingCycle Cycle { get; set; } = BillingCycle.Monthly;
        public string? PaymentMethod { get; set; }
        public DateTime? ActiveSince { get; set; }
        public bool IsActive { get; set; } = true;
        public string Currency { get; set; } = "PLN";
    }

    public enum BillingCycle
    {
        Monthly,
        Yearly,
        Weekly
    }
}