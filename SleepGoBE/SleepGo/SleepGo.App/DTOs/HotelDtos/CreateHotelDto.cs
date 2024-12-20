using SleepGo.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.HotelDtos
{
    public class CreateHotelDto
    {
        [Required]
        public string HotelName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string HotelDescription { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
    }
}
