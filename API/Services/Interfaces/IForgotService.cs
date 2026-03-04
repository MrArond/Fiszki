using API.DTOs;

namespace API.Services.Interfaces
{
    public interface IForgotService
    {
        Task<(bool, string)> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO);
    }
}
