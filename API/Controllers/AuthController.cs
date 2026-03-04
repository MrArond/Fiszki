using API.DATA.Context;
using API.DATA.Models;
using API.DTOs;
using API.Repositories.Interfaces;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Runtime.CompilerServices;
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
           var wynik = await _authService.Register(register);
            return Ok(wynik);
        }
        
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var user = await _authService.Login(login);
            return Ok(user);
            
        }
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        private readonly IAuthService _authService;




    }
}
