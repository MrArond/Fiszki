using API.DATA.Context;
using API.DATA.Models;
using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO t)
        {
            
            var d = await _datacontext.Users.FirstOrDefaultAsync(c => c.Email == t.Email);
            var b = await _datacontext.Users.FirstOrDefaultAsync(c => c.Nickname == t.NickName);

            if (d != null)
            {
                return BadRequest("Konto o podanym mailu istnieje juz");
            }
            
            if(b != null)
            {
                return BadRequest("Konto o podanym nicknamie istnieje już");
            }

            User User = new User { Email = t.Email, Nickname = t.NickName, Password = t.Password, SecretPassword = t.SecretPassword, IdOfSecretQuestion = t.IdOfSecretQuestion };
            _datacontext.Users.Add(User);
            _datacontext.SaveChanges();
            return Ok(new { Message = "Device registered successfully" });

        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO t)
        {
            var user = await _datacontext.Users.FirstOrDefaultAsync(c => c.Email == t.Email && c.Password == t.Password);
            if (user == null)
            {
                return BadRequest(new { Message = "Invalid Login or Password" });
            }
            else
            {
                var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Nickname!),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
                var token = _jwtService.GenerateJwtToken(claims);
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
        }


        [HttpPut("ForgotPassword")]
        [AllowAnonymous]

        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPassword)
           {
                var user = await _datacontext.Users.FirstOrDefaultAsync(c => c.Email == forgotPassword.Email && c.IdOfSecretQuestion == forgotPassword.IdOfSecretQuestion && c.SecretPassword == forgotPassword.SecretPassword);
                if(user == null)
                {
                    return BadRequest("Podano zly secret haslo");
                }
                else
                {
                user.Password = forgotPassword.Password;
                await _datacontext.SaveChangesAsync();
                return Ok(new { Message = "Device update properly" });
                }
           }

        private readonly Datacontext _datacontext;
        public AuthController(Datacontext e, JwtServices jwtService)
        {
            _datacontext = e;
            _jwtService = jwtService;
        }
        private readonly JwtServices _jwtService;



    }
}
