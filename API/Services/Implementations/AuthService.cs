using API.DTOs;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        async public Task<(bool, string)> Login(LoginDTO loginDTO)
        {
            if (string.IsNullOrWhiteSpace(loginDTO.Email))
            {
                bool isEmailValid = false;
                return (isEmailValid, "Please provide email");
            }
            if (string.IsNullOrWhiteSpace(loginDTO.Password))
            {
                bool isPasswordValid = false;
                return (isPasswordValid, "Please provide password");
            }

        }

        async public Task<(bool, string)> Register(RegisterDTO registerDTO)
        {
            if (!MailAddress.TryCreate(registerDTO.Email, out var mailResult))
            {
                bool isEmailValid = false;
                return (isEmailValid, "Please provide valid email");
            }
            if (registerDTO.Password.Length < 5)
            {
                bool isPasswordValid = false;
                return (isPasswordValid, "Password must be at least 5 characters long");
            }
            if (string.IsNullOrWhiteSpace(registerDTO.NickName))
            {
                bool isNickNameValid = false;
                return (isNickNameValid, "Please provide nickname");
            }
            if (string.IsNullOrWhiteSpace(registerDTO.SecretPassword))
            {
                bool isSecretPasswordValid = false;
                return (isSecretPasswordValid, "Please provide secret password");
            }
           
        }
    }
}
