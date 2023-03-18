using Unitess.Common;
using Unitess.Dtos;
using Unitess.Models;
using Unitess.Repositories.Interfaces;
using Unitess.Requests;
using Unitess.Services.Interfaces;

namespace Unitess.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        public async Task<TokenDto> CreateAsync(RegisterRequest registerRequest)
        {
            var hasUser = await _userRepository.HasUserAsync(registerRequest.Login);
            if (hasUser)
            {
                throw new Exception($"The user with the login {registerRequest.Login} already exists");
            }
            var refreshToken = _tokenService.GenerateRefreshToken();
            var user = new User()
            {
                Login = registerRequest.Login,
                Password = registerRequest.Password,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(2)
            };
            await _userRepository.CreateUserAsync(user);
            if (user == null)
            {
                throw new NotFoundException(nameof(user));
            }
            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            
            var tokenDto = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return tokenDto;
        }

        public async Task<User> GetAsync(string login, string? password)
        {
            var user = await _userRepository.GetUserAsync(login, password);
            if (user == null)
            {
                throw new NotFoundException(nameof(user));
            }
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }
    }
}
