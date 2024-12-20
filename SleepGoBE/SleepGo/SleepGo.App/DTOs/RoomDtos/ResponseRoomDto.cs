using SleepGo.App.DTOs.ReservationDtos;
using SleepGo.Domain.Entities;
using SleepGo.Domain.Enums;

namespace SleepGo.App.DTOs.RoomDtos
{
    public class ResponseRoomDto
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public RoomType RoomType { get; set; }
        public decimal Price { get; set; }
        public int RoomNumber { get; set; }
        public bool Balcony { get; set; }
        public bool AirConditioning { get; set; }
        public bool Kitchenette { get; set; }
        public bool Hairdryer { get; set; }
        public bool TV { get; set; }
        public bool IsReserved { get; set; } = false;
        public ICollection<ResponseReservationDto> Reservations { get; set; }
    }
}
