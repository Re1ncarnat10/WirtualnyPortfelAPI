using WirtualnyPortfelAPI.Interfaces;
using WirtualnyPortfelAPI.Models;
using WirtualnyPortfelAPI.Dto;

namespace WirtualnyPortfelAPI.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly List<Subscription> _subs = new();
        private readonly List<Category> _categories = new();

        public Task<Subscription> Create(Subscription subscription)
        {
            _subs.Add(subscription);
            return Task.FromResult(subscription);
        }

        public Task Delete(Guid id)
        {
            var s = _subs.FirstOrDefault(x => x.Id == id);
            if (s != null) _subs.Remove(s);
            return Task.CompletedTask;
        }

        public Task<Subscription?> GetById(Guid id)
        {
            return Task.FromResult(_subs.FirstOrDefault(x => x.Id == id));
        }

        public Task<SubscriptionDetailDto?> GetDetail(Guid id)
        {
            var s = _subs.FirstOrDefault(x => x.Id == id);
            if (s == null) return Task.FromResult<SubscriptionDetailDto?>(null);
            var cat = _categories.FirstOrDefault(c => c.Id == s.CategoryId);
            var dto = new SubscriptionDetailDto
            {
                Id = s.Id,
                Title = s.Title,
                Price = s.Price,
                Currency = s.Currency,
                Cycle = s.Cycle,
                RenewalDate = s.RenewalDate,
                ActiveSince = s.ActiveSince,
                IsActive = s.IsActive,
                PaymentMethod = s.PaymentMethod,
                CategoryId = s.CategoryId,
                CategoryName = cat?.Name,
                NotificationsEnabled = s.NotificationsEnabled
            };
            return Task.FromResult<SubscriptionDetailDto?>(dto);
        }

        public Task<IEnumerable<Subscription>> GetForUser(Guid userId)
        {
            return Task.FromResult(_subs.Where(x => x.UserId == userId).AsEnumerable());
        }

        public Task<MonthlyStats> GetMonthlyStats(Guid userId, int monthsBack)
        {
            var userSubs = _subs.Where(s => s.UserId == userId && s.IsActive).ToList();
            var currentMonthTotal = userSubs.Where(s => s.Cycle == BillingCycle.Monthly).Sum(s => s.Price)
                                     + userSubs.Where(s => s.Cycle == BillingCycle.Yearly).Sum(s => s.Price / 12);
            var yearlyTotal = userSubs.Sum(s => s.Cycle == BillingCycle.Yearly ? s.Price : s.Cycle == BillingCycle.Monthly ? s.Price * 12 : s.Price * 52);

            var byCat = userSubs.GroupBy(s => s.Category?.Name ?? "Inne").ToDictionary(g => g.Key, g => g.Sum(s => s.Cycle == BillingCycle.Yearly ? s.Price / 12 : s.Cycle == BillingCycle.Monthly ? s.Price : s.Price / 52));

            var trend = new List<MonthTrend>();
            for (int i = monthsBack - 1; i >= 0; i--)
            {
                var month = DateTime.UtcNow.AddMonths(-i);
                var monthStr = month.ToString("MMM");
                // naive: assume monthly cost as sum of monthly equivalents
                var amount = userSubs.Sum(s =>
                    s.Cycle == BillingCycle.Monthly ? s.Price : s.Cycle == BillingCycle.Yearly ? s.Price / 12 : s.Price * 4);
                trend.Add(new MonthTrend { Month = monthStr, Amount = amount });
            }

            var stats = new MonthlyStats
            {
                CurrentMonthlyTotal = Math.Round(currentMonthTotal, 2),
                YearlyTotal = Math.Round(yearlyTotal, 2),
                SubscriptionsCount = userSubs.Count,
                ByCategory = byCat,
                Trend = trend
            };

            return Task.FromResult(stats);
        }

        public Task Update(Subscription subscription)
        {
            var s = _subs.FirstOrDefault(x => x.Id == subscription.Id);
            if (s != null)
            {
                s.Title = subscription.Title;
                s.Price = subscription.Price;
                s.RenewalDate = subscription.RenewalDate;
                s.CategoryId = subscription.CategoryId;
                s.NotificationsEnabled = subscription.NotificationsEnabled;
            }
            return Task.CompletedTask;
        }
    }
}