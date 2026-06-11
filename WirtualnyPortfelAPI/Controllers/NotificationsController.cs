using Microsoft.AspNetCore.Mvc;
using WirtualnyPortfelAPI.Interfaces;

namespace WirtualnyPortfelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var pending = await _notificationService.GetPendingNotifications();
            return Ok(pending);
        }

        [HttpPost("{id}/mark-sent")]
        public async Task<IActionResult> MarkSent(Guid id)
        {
            await _notificationService.MarkAsSent(id);
            return NoContent();
        }
    }
}