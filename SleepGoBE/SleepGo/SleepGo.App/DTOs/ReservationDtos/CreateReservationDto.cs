using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.ReservationDtos
{
    public class CreateReservationDto
    {
        [Required]
        public Guid HotelId { get; set; }
        [Required]
        public DateTime CheckIn { get; set; }
        [Required]
        public DateTime CheckOut { get; set; }
    }
}
