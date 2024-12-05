using Microsoft.AspNetCore.Http;

namespace SleepGo.App.DTOs.ImageDtos
{
    public class UploadImageDto
    {
        public IFormFile File { get; set; }
        public Guid UserId { get; set; }
        public Guid? UserProfileId { get; set; }
    }
}
