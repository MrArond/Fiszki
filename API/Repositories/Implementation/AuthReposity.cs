using API.DATA.Context;
using API.DATA.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Implementation
{
    public class AuthReposity(Datacontext _datacontext) : IAuthRepository
    {
         public async Task<User?> GetByEmail(string email, string nickname) 
            => await _datacontext.Users.FirstOrDefaultAsync(c => c.Email.ToLower() == email || c.Nickname == nickname );


    }
}
