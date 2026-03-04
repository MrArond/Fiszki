using API.DTOs;

namespace API.Repositories.Interfaces
{
    public interface IForgotRepository
    {
        Task<(bool, string)> ResetPassword(ForgotPasswordDTO forgotPasswordDTO);
    }
}
