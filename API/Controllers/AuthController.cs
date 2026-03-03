using API.DATA.Context;
using API.DATA.Models;
using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
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
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
        {

            if (!MailAddress.TryCreate(register.Email, out var mailResult))
            {
                return BadRequest("Wrong format of the mail");
            }
            if(register.Password.Length < 5)
            {
                return BadRequest("Password must have atleast 5 characters");
            }
            if (string.IsNullOrWhiteSpace(register.NickName))
            {
                return BadRequest("Please provide nickname");
            }
            if (string.IsNullOrWhiteSpace(register.SecretPassword))
            {
                return BadRequest("Please provide secret password");
            }


            var d = await _datacontext.Users.FirstOrDefaultAsync(c => c.Email.ToLower() == register.Email);
            var b = await _datacontext.Users.FirstOrDefaultAsync(c => c.Nickname == register.NickName);

            if (d != null)
            {
                return BadRequest("Konto o podanym mailu istnieje juz");
            }
            
            if(b != null)
            {
                return BadRequest("Konto o podanym nicknamie istnieje już");
            }

            User User = new User { Email = register.Email.ToLower(), 
                Nickname = register.NickName, 
                Password = register.Password, 
                SecretPassword = register.SecretPassword, 
                IdOfSecretQuestion = register.IdOfSecretQuestion };

            _datacontext.Users.Add(User);
            _datacontext.SaveChanges();
            return Ok(new { Message = "Device registered successfully" });
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {

            if (string.IsNullOrWhiteSpace(login.Email))
            {
                return BadRequest("Please provide email");
            }
            if (string.IsNullOrWhiteSpace(login.Password))
            {
                return BadRequest("Please provide password");
            }

            var user = await _datacontext.Users.FirstOrDefaultAsync(c => c.Email.ToLower() == login.Email.ToLower() 
            && c.Password == login.Password);

            if (user == null)
            {
                return BadRequest(new { Message = "Invalid Login or Password" });
            }
            else
            {
                var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Nickname!),
                new(JwtRegisteredClaimNames.Email, user.Email.ToLower()!),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
                var token = _jwtService.GenerateJwtToken(claims);
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
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
