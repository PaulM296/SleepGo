using SleepGo.Domain.Entities.BaseEntities;

namespace SleepGo.Domain.Entities
{
    public class Review : BaseEntity
    {
        public required Guid UserId { get; set; }
        public required AppUser AppUser { get; set; }
        public required Guid HotelId { get; set; }
        public required Hotel Hotel { get; set; }
        public required string ReviewText { get; set; }
        public int? Rating { get; set; }
        public bool IsModerated { get; set; }
    }
}
