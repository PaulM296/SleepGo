using SleepGo.Domain.Entities.BaseEntities;

namespace SleepGo.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public required Guid UserId { get; set; }
        public required AppUser AppUser { get; set; }
        public required Guid RoomId { get; set; }
        public required Room Room { get; set; }
        public required DateTime CheckIn { get; set; }
        public required DateTime CheckOut {  get; set; }
        public required decimal Price { get; set; }
        public required string Status { get; set; }
    }
}
