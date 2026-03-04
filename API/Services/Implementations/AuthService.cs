using API.DTOs;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;

namespace API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly JwtServices _jwtServices;
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository, JwtServices jwtServices)
        {
            _authRepository = authRepository;
            _jwtServices = jwtServices;
        }
        async public Task<(bool, string)> Login(LoginDTO loginDTO)
        {
            if (string.IsNullOrWhiteSpace(loginDTO.Email))
            {
                return (false, "Please provide email");
            }
            if (string.IsNullOrWhiteSpace(loginDTO.Password))
            {
                return (false, "Please provide password");
            }

            try
            {
                var user = await _authRepository.LoginUser(loginDTO);
                if (user == null)
                {
                    return (false, "something went wrong");
                }
                else
                {
                    var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Nickname!),
                new(JwtRegisteredClaimNames.Email, user.Email.ToLower()!),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
                    var token = _jwtServices.GenerateJwtToken(claims);
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);  
                    return (true, tokenString);
                }
            }
            catch (UnauthorizedAccessException)
            {
                return (false, "Invalid email or password");
            }
            catch (Exception ex)
            {   
                return (false, $"An error occurred: {ex.Message}");
            }
        }

        async public Task<(bool, string)> Register(RegisterDTO registerDTO)
        {
            if (!MailAddress.TryCreate(registerDTO.Email, out var mailResult))
            {
                return (false, "Please provide valid email");
            }
            if (registerDTO.Password.Length < 5)
            {
                return (false, "Password must be at least 5 characters long");
            }
            if (string.IsNullOrWhiteSpace(registerDTO.NickName))
            {
                return (false, "Please provide nickname");
            }
            if (string.IsNullOrWhiteSpace(registerDTO.SecretPassword))
            {
                return (false, "Please provide secret password");
            }

            try
            {
                var existingUser = await _authRepository.GetByEmail(registerDTO.Email, registerDTO.NickName);
                if (existingUser != null)
                {
                    return (false, "User with this email or nickname already exists");
                }

                var user = await _authRepository.RegisterUser(registerDTO);
                return (true, "Registration successful");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }
    }
}
