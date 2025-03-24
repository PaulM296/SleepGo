using Microsoft.EntityFrameworkCore;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(SleepGoDbContext context) : base(context)
        {

        }

        public async Task<Payment?> GetByPaymentIntentIdAsync(string paymentIntentId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId);
        }

        public async Task<Payment?> GetByReservationIdAsync(Guid reservationId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.ReservationId == reservationId);
        }
    }
}
