using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Unitess.Dtos;
using Unitess.Requests;
using Unitess.Services.Interfaces;

namespace Unitess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }
        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.Password) || string.IsNullOrEmpty(registerRequest.Login))
            {
                return BadRequest("LoginRequest failed");
            }

            var tokenDto = await _userService.CreateAsync(registerRequest);
            return Ok(tokenDto);
        }
        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Password) || string.IsNullOrEmpty(loginRequest.Login))
            {
                return BadRequest("LoginRequest failed");
            }
           var user = await _userService.GetAsync(loginRequest.Login, loginRequest.Password);
           var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
           var refreshToken = _tokenService.GenerateRefreshToken();
           user.RefreshTokenExpiryTime = DateTime.Now.AddDays(2);
           user.RefreshToken = refreshToken;
           var tokenDto = new TokenDto
           {
               AccessToken = accessToken,
               RefreshToken = refreshToken
           };
           await _userService.UpdateAsync(user);
            return Ok(tokenDto);
        }
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
        {
            if (string.IsNullOrEmpty(tokenDto.AccessToken) || string.IsNullOrEmpty(tokenDto.RefreshToken))
                return BadRequest("Failed request from client");
            var accessToken = tokenDto.AccessToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            if (principal.Identity == null)
            {
                return BadRequest();
            }
            var login = principal.Identity.Name;
            if (string.IsNullOrEmpty(login))
            {
                return BadRequest();
            }
            var user = await _userService.GetAsync(login, null);
            if (user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(2);
            await _userService.UpdateAsync(user);
            var token = new TokenDto
            {
                AccessToken = await _tokenService.GenerateAccessTokenAsync(user),
                RefreshToken = newRefreshToken
            };
            return Ok(token);
        }
    }
}
