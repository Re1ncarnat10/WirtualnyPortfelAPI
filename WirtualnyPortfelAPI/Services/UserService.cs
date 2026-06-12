using WirtualnyPortfelAPI.Interfaces;
using WirtualnyPortfelAPI.Models;
using WirtualnyPortfelAPI.Dto;

namespace WirtualnyPortfelAPI.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new();

        public UserService()
        {
            // create a template user available at startup for testing / demo Google-login flow
            var template = new User
            {
                Email = "templatka@example.com",
                PasswordHash = "templatka",
                DisplayName = "Użytkownik Templatka"
            };
            _users.Add(template);
        }

        public Task<UserDto?> Authenticate(string email, string password)
        {
            // Placeholder: do not use in production
            var user = _users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
            if (user == null) return Task.FromResult<UserDto?>(null);
            return Task.FromResult<UserDto?>(new UserDto { Id = user.Id, Email = user.Email, DisplayName = user.DisplayName });
        }

        public Task<UserDto> Create(User user, string password)
        {
            user.PasswordHash = password; // placeholder
            _users.Add(user);
            return Task.FromResult(new UserDto { Id = user.Id, Email = user.Email, DisplayName = user.DisplayName });
        }

        public Task<UserDto?> GetById(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return Task.FromResult<UserDto?>(null);
            return Task.FromResult<UserDto?>(new UserDto { Id = user.Id, Email = user.Email, DisplayName = user.DisplayName });
        }

        public Task<UserDto?> GetByEmail(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);
            if (user == null) return Task.FromResult<UserDto?>(null);
            return Task.FromResult<UserDto?>(new UserDto { Id = user.Id, Email = user.Email, DisplayName = user.DisplayName });
        }
    }
}