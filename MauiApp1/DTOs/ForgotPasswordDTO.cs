namespace API.DTOs
{
    public class ForgotPasswordDTO
    {
        public string? Email { get; set; }

        public int IdOfSecretQuestion { get; set; }

        public string? SecretPassword { get; set; }

        public string? Password { get; set; }
    }
}
