using Microsoft.Extensions.Configuration;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;
using Stripe;

namespace SleepGo.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
        }

        public async Task<Payment> CreateOrUpdatePaymentIntent(Guid reservationId)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(reservationId)
                ?? throw new ReservationNotFoundException("Reservation has not been found");

            var existingPayment = await _unitOfWork.PaymentRepository.GetByReservationIdAsync(reservationId);

            if (existingPayment != null)
            {
                return existingPayment;
            }

            var totalAmount = (long)(reservation.Price * 100);
            var service = new PaymentIntentService();

            var options = new PaymentIntentCreateOptions
            {
                Amount = totalAmount,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };

            var intent = await service.CreateAsync(options);

            Payment payment = new Payment
            {
                ReservationId = reservationId,
                Amount = reservation.Price,
                Currency = "usd",
                Status = "Pending",
                PaymentIntentId = intent.Id,
                ClientSecret = intent.ClientSecret
            };

            await _unitOfWork.PaymentRepository.AddAsync(payment);
            await _unitOfWork.SaveAsync();

            return payment;
        }

        public async Task<bool> ConfirmPayment(string paymentIntentId)
        {
            var payment = await _unitOfWork.PaymentRepository.GetByPaymentIntentIdAsync(paymentIntentId);

            if (payment != null)
            {
                payment.Status = "Succeeded";
                payment.PaidAt = DateTime.UtcNow;

                var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(payment.ReservationId);
                if (reservation != null)
                {
                    reservation.Status = "Successful";
                    await _unitOfWork.ReservationRepository.UpdateAsync(reservation);
                }

                await _unitOfWork.PaymentRepository.UpdateAsync(payment);
                await _unitOfWork.SaveAsync();

                return true;
            }

            return false;
        }


    }
}
