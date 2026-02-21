using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DATA.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public required string Nickname { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required int IdOfSecretQuestion { get; set; }

        public required string SecretPassword { get; set; }
    }
}
