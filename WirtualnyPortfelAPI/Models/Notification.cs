namespace WirtualnyPortfelAPI.Models
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }
        public DateTime NotifyAt { get; set; }
        public bool Sent { get; set; }
    }
}