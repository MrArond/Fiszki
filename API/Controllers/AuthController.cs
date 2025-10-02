using API.DATA.Context;
using API.DATA.Models;
using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterDTO t)
        {
            var d = await _datacontext.Users.FirstOrDefaultAsync(c => c.Email == t.Email);
            if (d != null)
            {
                return BadRequest();
            }
            Random random = new Random();
            User User = new User {Id = random.Next(), Email = t.Email, Nickname = t.NickName, Password = t.Password };
            _datacontext.Users.Add(User);
            _datacontext.SaveChanges();
            return Ok(new { Message = "Device registered successfully" });
            
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginDTO t)
        {
            var user = await _datacontext.Users.FirstOrDefaultAsync(c => c.Email == t.Email && c.Password == t.Password);
            if(user == null)
            {
                return BadRequest(new { Message = "Invalid Login or Password" });
            }
            else
            {
                var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Nickname!),
                new(JwtRegisteredClaimNames.Email, user.Email!)
            };
                var token = _jwtService.GenerateJwtToken(claims);
                return Ok(new { token = token.EncodedPayload });
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
