using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SleepGo.App.DTOs.ReservationDtos
{
    public class UpdateReservationDto
    {
        [Required]
        public string Status { get; set; }
    }
}
