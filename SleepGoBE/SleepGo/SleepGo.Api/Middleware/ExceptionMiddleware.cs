using SleepGo.Api.Contracts;
using SleepGo.App.Exceptions;
using SleepGo.Infrastructure.Exceptions;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace SleepGo.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) when (ex is EntityNotFoundException || ex is ReviewNotFoundException
            || ex is HotelNotFoundException || ex is AmenityNotFoundException || ex is RoomNotFoundException
            || ex is ReservationNotFoundException || ex is UserNotFoundException 
            || ex is UserProfileNotFoundException || ex is ImageNotFoundException)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (InvalidCredentialsException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized);
            }
            catch (Exception ex) when (ex is EntityAlreadyExistsException || ex is EmailAlreadyExistsException)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Conflict);
            }
            catch (InvalidImageFormatException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.UnsupportedMediaType);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode httpStatusCode)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;

            var error = new Error
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedError = JsonSerializer.Serialize(error, options);

            await context.Response.WriteAsync(serializedError);
        }
    }
}
