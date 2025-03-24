using SleepGo.App.DTOs.PaymentDtos;

namespace SleepGo.App.DTOs.ReservationDtos
{
    public class ResponseReservationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
        public string HotelName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public PaymentDto Payment { get; set; }
    }
}
