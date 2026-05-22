using API.DATA.Models;
using API.DTOs;

namespace API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> RegisterUser(RegisterDTO registerDto);

        Task<User> LoginUser(LoginDTO loginDto);

        Task<User?> GetByEmail(RegisterDTO registerDTO);


        
    }
}