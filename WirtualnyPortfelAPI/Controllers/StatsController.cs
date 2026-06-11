using Microsoft.AspNetCore.Mvc;
using WirtualnyPortfelAPI.Interfaces;
using WirtualnyPortfelAPI.Dto;

namespace WirtualnyPortfelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public StatsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("user/{userId}/monthly")]
        public async Task<IActionResult> Monthly(Guid userId, [FromQuery] int months = 6)
        {
            var stats = await _subscriptionService.GetMonthlyStats(userId, months);
            return Ok(stats);
        }
    }
}
