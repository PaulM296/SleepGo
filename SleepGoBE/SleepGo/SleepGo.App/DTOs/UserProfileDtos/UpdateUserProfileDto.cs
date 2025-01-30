using Microsoft.AspNetCore.Http;
using SleepGo.App.Extensions;
using SleepGo.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.UserProfileDtos
{
    public class UpdateUserProfileDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
        [Required]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
        [AllowedExtensions(new string[] { ".png", ".jpg", ".jpeg" })]
        public IFormFile? ProfilePictureFile { get; set; }
    }
}
