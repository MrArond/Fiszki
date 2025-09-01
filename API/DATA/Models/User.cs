using System.ComponentModel.DataAnnotations;

namespace API.DATA.Models
{
    public class User
    {
        [Key]
        public required int Id { get; set; }

        public required string Nickname { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
