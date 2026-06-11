using Microsoft.AspNetCore.Mvc;
using WirtualnyPortfelAPI.Interfaces;
using WirtualnyPortfelAPI.Models;

namespace WirtualnyPortfelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetForUser(Guid userId)
        {
            var subs = await _subscriptionService.GetForUser(userId);
            return Ok(subs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetById(Guid id)
        {
            var s = await _subscriptionService.GetById(id);
            if (s == null) return NotFound();
            return Ok(s);
        }

        [HttpPost]
        public async Task<ActionResult<Subscription>> Create([FromBody] Subscription subscription)
        {
            var created = await _subscriptionService.Create(subscription);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Subscription subscription)
        {
            if (id != subscription.Id) return BadRequest();
            await _subscriptionService.Update(subscription);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _subscriptionService.Delete(id);
            return NoContent();
        }
    }
}