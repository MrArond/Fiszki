using API.DATA.Context;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace API.Controllers
{
    public class ForgotPasswordController : ControllerBase
    {
        [HttpPut("ForgotPassword")]
        [AllowAnonymous]

        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPassword)
        {
            if (string.IsNullOrWhiteSpace(forgotPassword.Email.ToLower()))
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
            if (!MailAddress.TryCreate(forgotPassword.Email.ToLower(), out var mailResult))
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


            if (user == null)
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

        public ForgotPasswordController(Datacontext datacontext)
        {
                       _datacontext = datacontext;
        }
    }
}
