using Microsoft.AspNetCore.Http;
using SleepGo.App.Extensions;
using SleepGo.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.UserDtos
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Your username must be between 6 and 30 characters long!")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format!")]
        public string Email { get; set; }
        [Required]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
        public string? HotelName { get; set; }
        public string? HotelDescription { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [AllowedExtensions([".png", ".jpg", ".jpeg"])]
        public IFormFile? ProfilePicture { get; set; }
    }
}
