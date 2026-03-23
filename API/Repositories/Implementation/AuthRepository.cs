using API.DATA.Context;
using API.DATA.Models;
using API.DTOs;
using API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace API.Repositories.Implementation
{
    public class AuthRepository(Datacontext _datacontext) : IAuthRepository
    {
        public async Task<User?> GetByEmail(RegisterDTO registerDTO)
            => await _datacontext.Users.FirstOrDefaultAsync(c => 
                c.Email.ToLower() == registerDTO.Email.ToLower() || c.Nickname == registerDTO.NickName);


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
            var user = await _datacontext.Users.FirstOrDefaultAsync(c =>
                c.Email.ToLower() == loginDTO.Email.ToLower() && c.Password == loginDTO.Password);

            if (user != null)
            {
                return user;
            }
            else
            {
                throw new UnauthorizedAccessException("Zle dane");
            }
        }
        
    }
}