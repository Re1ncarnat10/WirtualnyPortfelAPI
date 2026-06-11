using Microsoft.AspNetCore.Mvc;
using WirtualnyPortfelAPI.Interfaces;
using WirtualnyPortfelAPI.Models;
using WirtualnyPortfelAPI.Dto;

namespace WirtualnyPortfelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] User user, [FromQuery] string password)
        {
            var created = await _userService.Create(user, password);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromQuery] string email, [FromQuery] string password)
        {
            var user = await _userService.Authenticate(email, password);
            if (user == null) return Unauthorized();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}