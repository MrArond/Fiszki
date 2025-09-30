using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class JwtServices
    {
        private readonly IConfiguration _configuration;

        public JwtServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public JwtSecurityToken GenerateJwtToken(IEnumerable<Claim> claims)
        {
            
            var jwtSecret = _configuration["JWT_SECRET"];

            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret));

            return new JwtSecurityToken(
                issuer: _configuration["Authentication:ValidIssuer"],
                audience: _configuration["Authentication:ValidAudience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
        }
    }
}
