using Unitess.Dtos;
using Unitess.Models;
using Unitess.Requests;

namespace Unitess.Services.Interfaces
{
    public interface IUserService
    {
        Task<TokenDto> CreateAsync(RegisterRequest registerRequest);
        Task<User> GetAsync(string login, string? password);
        Task UpdateAsync(User user);
    }
}
