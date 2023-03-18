using Dapper;
using Unitess.Common;
using Unitess.Models;
using Unitess.Repositories.Interfaces;

namespace Unitess.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> GetUserAsync(string login, string? password)
        {
            await using var sqlConnection = DbConnection.CreateConnection();
            var sqlQuery = "SELECT * FROM Users WHERE Login = @Login";
            var parameters = new DynamicParameters();
            parameters.Add("Login", login);
            var user = sqlConnection.Query<User>(sqlQuery, parameters).FirstOrDefault();
            if (user == null)
            {
                throw new NotFoundException(nameof(user));
            }

            if (!string.IsNullOrEmpty(password) && password != user.Password)
            {
                throw new Exception("Password entered incorrectly");
            }
            return user;
        }

        public async Task CreateUserAsync(User user)
        {
            await using var sqlConnection = DbConnection.CreateConnection();
            var sqlQuery = "INSERT INTO Users (Login, Password, RefreshToken, RefreshTokenExpiryTime) VALUES(@Login, @Password, @RefreshToken, @RefreshTokenExpiryTime)";
            await sqlConnection.ExecuteAsync(sqlQuery, user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await using var sqlConnection = DbConnection.CreateConnection();
            var sqlQuery = "UPDATE Users SET Login = @Login, Password = @Password, RefreshToken = @RefreshToken, RefreshTokenExpiryTime = @RefreshTokenExpiryTime WHERE UserId = @UserId";
            await sqlConnection.ExecuteAsync(sqlQuery, user);
        }

        public async Task<bool> HasUserAsync(string login)
        {
            await using var sqlConnection = DbConnection.CreateConnection();
            var sqlQuery = "SELECT * FROM Users WHERE Login = @Login";
            var parameters = new DynamicParameters();
            parameters.Add("Login", login);
            var hasUser = sqlConnection.Query<User>(sqlQuery, parameters).Any();
            return hasUser;
        }
        public async Task<bool> HasUserAsync(int userId)
        {
            await using var sqlConnection = DbConnection.CreateConnection();
            var sqlQuery = "SELECT * FROM Users WHERE UserId = @UserId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId);
            var hasUser = sqlConnection.Query<User>(sqlQuery, parameters).Any();
            return hasUser;
        }
    }
}
