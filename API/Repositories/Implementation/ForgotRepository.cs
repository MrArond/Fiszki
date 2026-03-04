using API.DATA.Context;
using API.DTOs;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Implementation
{
    public class ForgotRepository(Datacontext _datacontext) : IForgotRepository
    {
        async public Task<(bool, string)> ResetPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            var user = await _datacontext.Users.FirstOrDefaultAsync(c => c.Email == forgotPasswordDTO.Email
                && c.IdOfSecretQuestion == forgotPasswordDTO.IdOfSecretQuestion
                && c.SecretPassword == forgotPasswordDTO.SecretPassword);
            if (user == null)
            {
                return (false, "something went wrong");
            }
            else
            {
                user.Password = forgotPasswordDTO.Password;
                await _datacontext.SaveChangesAsync();
                return (true, user.Password);
            }
        }
    }
}
