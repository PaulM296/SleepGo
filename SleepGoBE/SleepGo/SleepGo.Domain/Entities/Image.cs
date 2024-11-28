using SleepGo.Domain.Entities.BaseEntities;

namespace SleepGo.Domain.Entities
{
    public class Image : BaseEntity
    {
        public required string Name { get; set; }
        public required string Type { get; set; }
        public required byte[] Data { get; set; }
    }
}
