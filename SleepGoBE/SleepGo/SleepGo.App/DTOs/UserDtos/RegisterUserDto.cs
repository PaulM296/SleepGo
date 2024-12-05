using Microsoft.AspNetCore.Http;
using SleepGo.App.Extensions;
using SleepGo.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.UserDtos
{
    public class RegisterUserDto
    {
        [Required]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Your username must be between 6 abd 30 characters long!")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format!")]
        public string Email { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Your password must be between 8 and 25 characters long!")]
        public string Password { get; set; }
        [Required]
        [Range(0, 1)]
        public Role Role { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [AllowedExtensions([".png", ".jpg", ".jpeg"])]
        public IFormFile? ProfilePicture { get; set; }
    }
}
