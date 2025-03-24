using SleepGo.Domain.Entities.BaseEntities;

namespace SleepGo.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; } 
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "usd";
    }
}
