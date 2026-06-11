using WirtualnyPortfelAPI.Models;
using WirtualnyPortfelAPI.Dto;

namespace WirtualnyPortfelAPI.Interfaces
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<Subscription>> GetForUser(Guid userId);
        Task<Subscription?> GetById(Guid id);
        Task<SubscriptionDetailDto?> GetDetail(Guid id);
        Task<MonthlyStats> GetMonthlyStats(Guid userId, int monthsBack);
        Task<Subscription> Create(Subscription subscription);
        Task Update(Subscription subscription);
        Task Delete(Guid id);
    }
}