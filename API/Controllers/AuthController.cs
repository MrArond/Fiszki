using API.DATA.Context;
using API.DATA.Models;
using API.DTOs;
using API.Repositories.Interfaces;
using API.Services;
using API.Services.Implementations;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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
            var (isSuccess, message) = await _authService.Register(register);
            if (isSuccess)
            {
                return Ok(new { message });
            }
            return BadRequest(new { message });
        }
        
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {

            var (isSuccess, message) = await _authService.Login(login);
            if (isSuccess)
            {
                return Ok(message);
            }
            return BadRequest(message);

        }
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        private readonly IAuthService _authService;
    }
}
