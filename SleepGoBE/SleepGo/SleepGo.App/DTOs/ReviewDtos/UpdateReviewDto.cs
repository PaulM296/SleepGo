using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.ReviewDtos
{
    public class UpdateReviewDto
    {
        [Required]
        [StringLength(1500, ErrorMessage = "The review must have 1500 characters or fewer!")]
        public string ReviewText { get; set; }
        public int? Rating { get; set; }
    }
}
