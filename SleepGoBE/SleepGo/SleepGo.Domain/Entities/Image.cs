using SleepGo.Domain.Entities.BaseEntities;

namespace SleepGo.Domain.Entities
{
    public class Image : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
    }
}
