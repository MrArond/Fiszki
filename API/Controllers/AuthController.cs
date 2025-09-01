using API.DATA.Context;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using API.DATA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
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

        public async Task<IActionResult> Login([FromBody]LoginDTO t)
        {
            var user = await _datacontext.Users.FirstOrDefaultAsync(c => c.Email == t.Email && c.Password == t.Password);
            if(user == null)
            {
                return BadRequest(new { Message = "Invalid Login or Password" });
            }
            else
            {
                return Ok(new { Message = "Device logged" });
            }
                    
                

        }
        private readonly Datacontext _datacontext;
        public AuthController(Datacontext e)
        {
            _datacontext = e;
        }

    }
}
