namespace SleepGo.App.DTOs.UserProfileDtos
{
    public class ResponseUserProfileDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid? ImageId { get; set; }
    }
}
