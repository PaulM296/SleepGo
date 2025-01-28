using Microsoft.AspNetCore.Http;
using SleepGo.App.Extensions;
using SleepGo.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.UserDtos
{
    public class RegisterUserDto
    {
        [Required]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Your username must be between 6 and 30 characters long!")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format!")]
        public string Email { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Your password must be between 8 and 25 characters long!")]
        public string Password { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required]
        [Range(0, 2)]
        public Role Role { get; set; }

        // User-specific fields
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        // Hotel-specific fields
        public string? HotelName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public string? HotelDescription { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [AllowedExtensions(new[] { ".png", ".jpg", ".jpeg" })]
        public IFormFile? ProfilePicture { get; set; }
    }

}
