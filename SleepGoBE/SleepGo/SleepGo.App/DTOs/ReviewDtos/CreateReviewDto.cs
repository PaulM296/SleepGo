using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.ReviewDtos
{
    public class CreateReviewDto
    {
        [Required]
        public Guid HotelId { get; set; }
        [Required]
        [StringLength(1500, ErrorMessage = "The review must have 1500 characters or fewer!")]
        public string ReviewText { get; set; }
        [Range(1, 10)]
        public int? Rating { get; set; }
    }
}
