using System.ComponentModel.DataAnnotations;

namespace UdemyEgitimPlatformu.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool RememberMe { get; set; }
    }
}