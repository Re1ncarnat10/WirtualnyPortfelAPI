using WirtualnyPortfelAPI.Models;

namespace WirtualnyPortfelAPI.Dto
{
    public class SubscriptionDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "PLN";
        public BillingCycle Cycle { get; set; }
        public DateTime RenewalDate { get; set; }
        public DateTime? ActiveSince { get; set; }
        public bool IsActive { get; set; }
        public string? PaymentMethod { get; set; }
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool NotificationsEnabled { get; set; }
        public decimal YearlyCost => Cycle == BillingCycle.Yearly ? Price : Cycle == BillingCycle.Monthly ? Price * 12 : Price * 52;
    }
}