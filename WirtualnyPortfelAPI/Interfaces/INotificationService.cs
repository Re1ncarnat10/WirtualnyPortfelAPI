using WirtualnyPortfelAPI.Models;

namespace WirtualnyPortfelAPI.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetPendingNotifications();
        Task MarkAsSent(Guid notificationId);
    }
}