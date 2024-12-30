using SleepGo.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.ReservationDtos
{
    public class CreateReservationDto
    {
        [Required]
        public Guid HotelId { get; set; }
        [Required]
        public RoomType RoomType { get; set; }
        [Required]
        public DateTime CheckIn { get; set; }
        [Required]
        public DateTime CheckOut { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [DefaultValue("Pending")]
        public string Status { get; set; } = "Pending";
    }
}
