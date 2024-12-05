using Microsoft.AspNetCore.Http;
using SleepGo.App.Extensions;
using SleepGo.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.UserDtos
{
    public class UpdateUserDto
    {
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Your username must be between 6 and 30 characters long!")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format!")]
        public string Email { get; set; }

        [Range(0, 1)]
        public Role Role { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [AllowedExtensions([".png", ".jpg", ".jpeg"])]
        public IFormFile? ProfilePicture { get; set; }
    }
}
