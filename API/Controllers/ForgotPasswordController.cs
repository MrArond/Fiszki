using API.DATA.Context;
using API.DATA.Models;
using API.DTOs;
using API.Services.Implementations;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForgotPasswordController : ControllerBase
    {
        public ForgotPasswordController(IForgotService forgotService)
        {
            _forgotService = forgotService;
        }

        [HttpPut("ForgotPassword")]
        [AllowAnonymous]
       
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPassword)
        {
            var (isSuccess, message) = await _forgotService.ForgotPassword(forgotPassword);
            if (isSuccess)
            {
                return Ok(new { message });
            }
            return BadRequest(new { message });
        }
        private readonly IForgotService _forgotService;

        
    }
}
