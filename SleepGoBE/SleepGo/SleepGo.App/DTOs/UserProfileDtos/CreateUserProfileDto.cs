using SleepGo.App.DTOs.ImageDtos;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.UserProfileDtos
{
    public class CreateUserProfileDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public CreateImageDto ProfilePicture { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
