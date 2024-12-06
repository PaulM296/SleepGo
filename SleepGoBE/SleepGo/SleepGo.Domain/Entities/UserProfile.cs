using SleepGo.Domain.Entities.BaseEntities;

namespace SleepGo.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Guid UserId {  get; set; }
        public AppUser AppUser { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid? ImageId {  get; set; }
        public Image? Image { get; set; }

    }
}
