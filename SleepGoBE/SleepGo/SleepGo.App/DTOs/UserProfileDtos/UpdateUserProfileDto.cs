using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.UserProfileDtos
{
    public class UpdateUserProfileDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
