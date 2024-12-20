using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.AmenityDtos
{
    public class CreateAmenityDto
    {
        [Required]
        public Guid HotelId { get; set; }
        public bool? Pool { get; set; }
        public bool? Restaurant { get; set; }
        public bool? Fitness { get; set; }
        public bool? WiFi { get; set; }
        public bool? RoomService { get; set; }
        public bool? Bar { get; set; }
    }
}
