using API.DATA.Models;
using API.DTOs;

namespace API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetByEmail(string email, string nickname);

        Task<User?> LoginUser(string email, string password);
 
        Task<User> RegisterUser(RegisterDTO registerDto);

        Task<User> LoginUser(LoginDTO loginDto);


        
    }
}