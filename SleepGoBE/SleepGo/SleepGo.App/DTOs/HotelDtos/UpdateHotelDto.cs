using Microsoft.AspNetCore.Http;
using SleepGo.App.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.HotelDtos
{
    public class UpdateHotelDto
    {
        [Required]
        public string HotelName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
        [Required]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
        [Required]
        public string HotelDescription { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [AllowedExtensions(new string[] { ".png", ".jpg", ".jpeg" })]
        public IFormFile? HotelImageFile { get; set; }
    }
}
