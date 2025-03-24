namespace SleepGo.App.DTOs.PaymentDtos
{
    public class PaymentDto
    {
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
        public string Status { get; set; }
        public DateTime? PaidAt { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
