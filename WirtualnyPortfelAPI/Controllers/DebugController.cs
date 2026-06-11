using Microsoft.AspNetCore.Mvc;
using WirtualnyPortfelAPI.Models;
using WirtualnyPortfelAPI.Dto;

namespace WirtualnyPortfelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : ControllerBase
    {
        private readonly WirtualnyPortfelAPI.Services.SubscriptionService _subs;

        public DebugController(WirtualnyPortfelAPI.Interfaces.ISubscriptionService subs)
        {
            _subs = subs as WirtualnyPortfelAPI.Services.SubscriptionService ?? throw new ArgumentException("requires SubscriptionService implementation");
        }

        [HttpPost("seed-demo/{userId}")]
        public IActionResult SeedDemo(Guid userId)
        {
            // create categories
            var cat1 = new Category { Name = "Rozrywka" };
            var cat2 = new Category { Name = "Produktywność" };
            var cat3 = new Category { Name = "Muzyka" };

            _subs.SeedCategory(cat1);
            _subs.SeedCategory(cat2);
            _subs.SeedCategory(cat3);

            // create subscriptions
            var s1 = new Subscription { Title = "Netflix", Price = 43m, RenewalDate = DateTime.UtcNow.AddDays(3), UserId = userId, CategoryId = cat1.Id };
            var s2 = new Subscription { Title = "Spotify Premium", Price = 23.99m, RenewalDate = DateTime.UtcNow.AddDays(10), UserId = userId, CategoryId = cat3.Id };
            var s3 = new Subscription { Title = "ChatGPT Plus", Price = 81.83m, RenewalDate = DateTime.UtcNow.AddDays(5), UserId = userId, CategoryId = cat2.Id };

            _subs.Create(s1).Wait();
            _subs.Create(s2).Wait();
            _subs.Create(s3).Wait();

            return Ok();
        }

        [HttpGet("upcoming/{userId}")]
        public IActionResult Upcoming(Guid userId)
        {
            var list = _subs.GetUpcomingForUser(userId).Result;
            return Ok(list);
        }
    }
}
