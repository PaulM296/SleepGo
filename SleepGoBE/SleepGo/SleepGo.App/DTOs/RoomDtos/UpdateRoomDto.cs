using SleepGo.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.RoomDtos
{
    public class UpdateRoomDto
    {
        [Required]
        public RoomType RoomType { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int RoomNumber { get; set; }
        [Required]
        public bool Balcony { get; set; }
        [Required]
        public bool AirConditioning { get; set; }
        [Required]
        public bool Kitchenette { get; set; }
        [Required]
        public bool Hairdryer { get; set; }
        [Required]
        public bool TV { get; set; }
        [Required]
        public bool IsReserved { get; set; }
    }
}
