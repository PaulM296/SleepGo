namespace SleepGo.App.DTOs.ReviewDtos
{
    public class ResponseReviewDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid HotelId { get; set; }
        public string ReviewTetx { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public bool isModerated { get; set; }
    }
}
