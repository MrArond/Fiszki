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


            var d = await _datacontext.Users.FirstOrDefaultAsync(c => c.Email == register.Email);
            var b = await _datacontext.Users.FirstOrDefaultAsync(c => c.Nickname == register.NickName);

            if (d != null)
            {
                return BadRequest("Konto o podanym mailu istnieje juz");
            }
            
            if(b != null)
            {
                return BadRequest("Konto o podanym nicknamie istnieje już");
            }

            User User = new User { Email = register.Email, 
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

            var user = await _datacontext.Users.FirstOrDefaultAsync(c => c.Email == login.Email 
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
            if (string.IsNullOrWhiteSpace(forgotPassword.Email))
            {
                return BadRequest("Please provide mail");

            }
            if (string.IsNullOrWhiteSpace(forgotPassword.Password))
            {
                return BadRequest("Please provide password");

            }
            if (forgotPassword.Password.Length < 5)
            {
                return BadRequest("Password must have atleast 5 characters");

            }
            if (!MailAddress.TryCreate(forgotPassword.Email, out var mailResult))
            {
                return BadRequest("Please type the correct format of mail");

            }
            if (string.IsNullOrWhiteSpace(forgotPassword.SecretPassword))
            {
                return BadRequest("Please provide the secret answer");
            }

            var user = await _datacontext.Users.FirstOrDefaultAsync(c => c.Email == forgotPassword.Email 
                && c.IdOfSecretQuestion == forgotPassword.IdOfSecretQuestion 
                && c.SecretPassword == forgotPassword.SecretPassword);


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
