using WirtualnyPortfelAPI.Interfaces;
using WirtualnyPortfelAPI.Models;

namespace WirtualnyPortfelAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly List<Notification> _notifications = new();

        public Task<IEnumerable<Notification>> GetPendingNotifications()
        {
            return Task.FromResult(_notifications.Where(n => !n.Sent).AsEnumerable());
        }

        public Task MarkAsSent(Guid notificationId)
        {
            var n = _notifications.FirstOrDefault(x => x.Id == notificationId);
            if (n != null) n.Sent = true;
            return Task.CompletedTask;
        }
    }
}