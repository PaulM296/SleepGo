using System.ComponentModel.DataAnnotations;

namespace SleepGo.App.DTOs.PaginationDtos
{
    public class PaginationRequestDto
    {
        [Required]
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
