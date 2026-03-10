using API.DATA.Context;
using API.DTOs;
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
            var result = await _forgotService.ForgotPassword(forgotPassword);
            return Ok(result);
        }
        private readonly IForgotService _forgotService;

        
    }
}
