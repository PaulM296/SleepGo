using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.AmenityDtos
{
    public class UpdateAmenityDto
    {
        [Required]
        public bool Pool { get; set; }
        [Required]
        public bool Restaurant { get; set; }
        [Required]
        public bool Fitness { get; set; }
        [Required]
        public bool WiFi { get; set; }
        [Required]
        public bool RoomService { get; set; }
        [Required]
        public bool Bar { get; set; }
    }
}
