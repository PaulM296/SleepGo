using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SleepGo.App.Interfaces;
using Stripe;

namespace SleepGo.Api.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController(IPaymentService paymentService, IConfiguration config, ILogger<PaymentController> logger) : ControllerBase
    {
        
        private readonly string _whSecret = config["StripeSettings:WhSecret"]!;

        [HttpPost("create-payment-intent/{reservationId}")]
        public async Task<IActionResult> CreatePaymentIntent(Guid reservationId)
        {
            try
            {
                var payment = await paymentService.CreateOrUpdatePaymentIntent(reservationId);

                if (payment == null || string.IsNullOrEmpty(payment.ClientSecret))
                {
                    logger.LogWarning("Payment intent creation failed or returned no client secret for reservationId: {ReservationId}", reservationId);
                    return BadRequest(new { message = "Failed to create payment intent." });
                }

                return Ok(new { clientSecret = payment.ClientSecret });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating payment intent for reservationId: {ReservationId}", reservationId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            logger.LogInformation("Stripe webhook received: {Json}", json);

            try
            {
                var stripeEvent = ConstructStripeEvent(json);
                logger.LogInformation("Stripe event type: {EventType}", stripeEvent.Type);

                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    var intent = stripeEvent.Data.Object as PaymentIntent;

                    if (intent != null)
                    {
                        logger.LogInformation("PaymentIntent succeeded: {IntentId}, Amount: {Amount}, Status: {Status}",
                            intent.Id, intent.Amount, intent.Status);

                        var result = await paymentService.ConfirmPayment(intent.Id);

                        logger.LogInformation("Payment confirmed and reservation updated for intent: {IntentId}", intent.Id);


                        logger.LogInformation("Payment status update result: {Result}", result);
                    }
                    else
                    {
                        logger.LogWarning("PaymentIntent was null in webhook.");
                    }
                }
                else
                {
                    logger.LogInformation("Unhandled Stripe event type: {Type}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                logger.LogError(ex, "Stripe webhook error.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Webhook error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }


        private Event ConstructStripeEvent(string json)
        {
            try
            {
                return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"],
                    _whSecret);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to construct stripe event");
                throw new StripeException("Invalid signature");
            }
        }
    }
}
