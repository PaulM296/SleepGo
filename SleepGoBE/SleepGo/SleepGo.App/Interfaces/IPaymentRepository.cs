using SleepGo.Domain.Entities;

namespace SleepGo.App.Interfaces
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<Payment?> GetByPaymentIntentIdAsync(string paymentIntentId);
        Task<Payment?> GetByReservationIdAsync(Guid reservationId);
    }
}
