using SleepGo.Domain.Entities.BaseEntities;

namespace SleepGo.Domain.Entities
{
    public class Review : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? Rating { get; set; }
        public bool IsModerated { get; set; }
    }
}
