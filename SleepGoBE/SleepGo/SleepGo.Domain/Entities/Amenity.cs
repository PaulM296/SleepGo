using SleepGo.Domain.Entities.BaseEntities;

namespace SleepGo.Domain.Entities
{
    public class Amenity : BaseEntity
    {
        public required Guid HotelId { get; set; }
        public required Hotel Hotel { get; set; }
        public bool? Pool {  get; set; }
        public bool? Restaurant { get; set; }
        public bool? Fitness { get; set; }
        public bool? WiFi { get; set; }
        public bool? RoomService { get; set; }
        public bool? Bar { get; set; }
    }
}
