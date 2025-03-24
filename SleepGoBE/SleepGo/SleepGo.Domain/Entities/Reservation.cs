using SleepGo.Domain.Entities.BaseEntities;

namespace SleepGo.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut {  get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public Payment Payment { get; set; }
    }
}
