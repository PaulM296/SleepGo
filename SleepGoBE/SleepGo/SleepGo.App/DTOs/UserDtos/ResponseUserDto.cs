using SleepGo.Domain.Enums;

namespace SleepGo.App.DTOs.UserDtos
{
    public class ResponseUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid ImageId { get; set; }
        public bool isBlocked { get; set; }
    }
}
