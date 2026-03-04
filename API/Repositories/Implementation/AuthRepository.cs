using API.DATA.Context;
using API.DATA.Models;
using API.DTOs;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace API.Repositories.Implementation
{
    public class AuthRepository(Datacontext _datacontext) : IAuthRepository
    {
        public async Task<User?> GetByEmail(string email, string nickname)
            => await _datacontext.Users.FirstOrDefaultAsync(c => 
                c.Email.ToLower() == email.ToLower() || c.Nickname == nickname);

        public async Task<User?> LoginUser(string email, string password)
            => await _datacontext.Users.FirstOrDefaultAsync(c =>
                c.Email.ToLower() == email.ToLower() && c.Password == password);

        public async Task<User> RegisterUser(RegisterDTO registerDto)
        {
            var user = new User
            {
                Nickname = registerDto.NickName,
                Email = registerDto.Email.ToLower(),
                Password = registerDto.Password,
                IdOfSecretQuestion = registerDto.IdOfSecretQuestion,
                SecretPassword = registerDto.SecretPassword
            };

            _datacontext.Users.Add(user);
            await _datacontext.SaveChangesAsync();
            return user;
        }

        public async Task<User> LoginUser(LoginDTO loginDTO)
        {
            var user = await LoginUser(loginDTO.Email, loginDTO.Password);

            if(user != null)
            {
                return user;
            }

            throw new UnauthorizedAccessException("Invalid email or password");
        }
        
    }
}