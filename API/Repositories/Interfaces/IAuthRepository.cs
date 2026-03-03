using API.DATA.Models;
using API.DTOs;

namespace API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetByEmail(string email, string nickname);
    }
}
