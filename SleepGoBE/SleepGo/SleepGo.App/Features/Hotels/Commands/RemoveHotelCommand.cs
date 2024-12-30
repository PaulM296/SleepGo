using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Hotels.Commands
{
    public record RemoveHotelCommand(Guid hotelId) : IRequest<Unit>;

    public class RemoveHotelCommandHandler : IRequestHandler<RemoveHotelCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveHotelCommandHandler> _logger;

        public RemoveHotelCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveHotelCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveHotelCommand request, CancellationToken cancellationToken)
        {
            var hotelToRemove = await _unitOfWork.HotelRepository.GetByIdAsync(request.hotelId);

            if(hotelToRemove == null)
            {
                throw new HotelNotFoundException($"The hotel with ID {request.hotelId} has not been found and therefore could not be removed!");
            }

            await _unitOfWork.HotelRepository.RemoveAsync(hotelToRemove);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Hotel successfully removed!");

            return Unit.Value;
        }
    }
}
