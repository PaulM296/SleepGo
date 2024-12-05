using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.ImageDtos
{
    public class CreateImageDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public IFormFile Data { get; set; }
    }
}
