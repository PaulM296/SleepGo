using SleepGo.Domain.Entities.BaseEntities;
using SleepGo.Domain.Enums;

namespace SleepGo.Domain.Entities
{
    public class Room : BaseEntity
    {
        public Guid HotelId { get; set; }
        public required Hotel Hotel { get; set; }
        public required RoomType RoomType { get; set; }
        public required decimal Price { get; set; }
        public required int Quantity { get; set; }
        public bool Balcony { get; set; }
        public bool AirConditioning { get; set; }
        public bool Kitchenette { get; set; }
        public bool Hairdryer { get; set; }
        public bool TV { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

        public int GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            int reservedCount = Reservations.Count(r => r.CheckIn < endDate && r.CheckOut > startDate);
            return Quantity - reservedCount;
        }
    }
}
