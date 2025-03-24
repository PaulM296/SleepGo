using SleepGo.Domain.Entities;

namespace SleepGo.App.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> CreateOrUpdatePaymentIntent(Guid reservationId);
        Task<bool> ConfirmPayment(string paymentIntentId);
    }
}
