using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    internal class ForgotPasswordFormModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Choose question")]
        public int IdOfSecretQuestion { get; set; }

        [Required(ErrorMessage = "Answer is required")]
        public string SecretPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Min 6 characters")]
        public string Password { get; set; }
    }
}
}
