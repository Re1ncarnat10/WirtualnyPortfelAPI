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
            // Basic server-side validation with specific error codes/messages
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                return BadRequest("PASSWORD_TOO_WEAK: Has³o musi mieæ co najmniej 8 znaków.");
            }

            var existing = await _userService.GetByEmail(user.Email);
            if (existing != null)
            {
                return Conflict("EMAIL_TAKEN: Podany adres e-mail jest ju¿ zarejestrowany.");
            }

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

        [HttpPost("google-login")]
        public async Task<ActionResult<UserDto>> GoogleLogin([FromQuery] string email)
        {
            // return existing user by email or create a lightweight record for Google users
            var user = await _userService.GetByEmail(email);
            if (user != null) return Ok(user);
            var created = await _userService.Create(new Models.User { Email = email, DisplayName = "Google User" }, "");
            return Ok(created);
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