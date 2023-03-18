using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Unitess.Jwt
{
    internal class JwtOptions
    {
        public const string Issuer = "Unitess";
        public const string Audience = "Unitess";
        private const string Key = "Unitesssssssss!!!2023";

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
