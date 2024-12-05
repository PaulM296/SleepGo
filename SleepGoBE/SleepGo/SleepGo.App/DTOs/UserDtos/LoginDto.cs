using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.UserDtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format!")]
        public string Email { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Your password must be between 8 and 25 characters long!")]
        public string Password { get; set; }
    }
}
