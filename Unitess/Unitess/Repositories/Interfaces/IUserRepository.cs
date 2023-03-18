using Unitess.Models;

namespace Unitess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string login, string? password);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<bool> HasUserAsync(string login);
        Task<bool> HasUserAsync(int userId);
    }
}
