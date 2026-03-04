using API.DTOs;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using System.Net.Mail;

namespace API.Services.Implementations
{
    public class ForgotService : IForgotService
    {
        private readonly IForgotRepository _forgotRepository;
        public ForgotService(IForgotRepository forgotRepository)
        {
            _forgotRepository = forgotRepository;
        }
        async public Task<(bool, string)> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            if (string.IsNullOrWhiteSpace(forgotPasswordDTO.Email.ToLower()))
            {
                return (false, "Please provide mail");

            }
            if (string.IsNullOrWhiteSpace(forgotPasswordDTO.Password))
            {
                return (false, "Please provide password");

            }
            if (forgotPasswordDTO.Password.Length < 5)
            {
                return (false, "Password must have atleast 5 characters");

            }
            if (!MailAddress.TryCreate(forgotPasswordDTO.Email.ToLower(), out var mailResult))
            {
                return (false, "Please type the correct format of mail");

            }
            if (string.IsNullOrWhiteSpace(forgotPasswordDTO.SecretPassword))
            {
                return (false, "Please provide the secret answer");
            }
            try
            {
                await _forgotRepository.ResetPassword(forgotPasswordDTO);
                return (true, "Good");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }
    }
}
