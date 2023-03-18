using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Unitess.Jwt;
using Unitess.Models;
using Unitess.Services.Interfaces;

namespace Unitess.Services.Implementations
{
    public class TokenService : ITokenService
    {
        public async Task<string> GenerateAccessTokenAsync(User user)
        {
            var key = JwtOptions.GetSymmetricSecurityKey();
            var claims = await Task.Run(() => GetClaims(user));
            var token = new JwtSecurityTokenHandler().WriteToken
            (
                new JwtSecurityToken
                (
                    JwtOptions.Issuer,
                    JwtOptions.Audience,
                    claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                )
            );
            return token;
        }

        public string GenerateRefreshToken()
        {
            var tokenByte = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenByte);
            return refreshToken;
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = JwtOptions.GetSymmetricSecurityKey(),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase)) throw new SecurityTokenException("This is invalid token");
            return principal;
        }
        private List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Login)
            };
            return claims;
        }
    }
}
