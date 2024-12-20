using SleepGo.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.RoomDtos
{
    public class CreateRoomDto
    {
        [Required]
        public Guid HotelId { get; set; }
        [Required]
        [EnumDataType(typeof(RoomType), ErrorMessage = "Invalid room type.")]
        public RoomType RoomType { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public bool Balcony { get; set; }
        [Required]
        public bool AirConditioning { get; set; }
        [Required]
        public bool Kitchenette { get; set; }
        [Required]
        public bool Hairdryer { get; set; }
        [Required]
        public bool TV {  get; set; }
    }
}
