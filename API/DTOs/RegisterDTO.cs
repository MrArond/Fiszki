namespace API.DTOs
{
    public class RegisterDTO
    {
        public required string NickName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required int IdOfSecretQuestion { get; set; }

        public required string SecretPassword { get; set; }
    }
}
 