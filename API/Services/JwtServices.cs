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
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new InvalidOperationException("JWT_SECRET is not configured");
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var now = DateTime.UtcNow;

            return new JwtSecurityToken(
                issuer: _configuration["Authentication:ValidIssuer"],
                audience: _configuration["Authentication:ValidAudience"],
                claims: claims,
                notBefore: now,
                expires: now.AddHours(3),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
        }
    }
}
