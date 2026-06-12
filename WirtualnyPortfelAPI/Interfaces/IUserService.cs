using WirtualnyPortfelAPI.Models;
using WirtualnyPortfelAPI.Dto;

namespace WirtualnyPortfelAPI.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> Authenticate(string email, string password);
        Task<UserDto> Create(User user, string password);
        Task<UserDto?> GetById(Guid id);
        Task<UserDto?> GetByEmail(string email);
    }
}