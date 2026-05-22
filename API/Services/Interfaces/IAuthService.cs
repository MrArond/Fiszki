using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.Interfaces
{
    public interface IAuthService
    {
    Task<(bool, string)> Login(LoginDTO loginDTO);
    Task<(bool, string)> Register(RegisterDTO registerDTO);

    
    }
}