using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Reservations.Commands
{
    public record RemoveReservationCommand(Guid reservationId) : IRequest<Unit>;

    public class RemoveReservationCommandHandler : IRequestHandler<RemoveReservationCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveReservationCommandHandler> _logger;

        public RemoveReservationCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveReservationCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveReservationCommand request, CancellationToken cancellationToken)
        {
            var getReservation = await _unitOfWork.ReservationRepository.GetByIdAsync(request.reservationId);

            if(getReservation == null)
            {
                throw new ReservationNotFoundException($"The reservation with ID {request.reservationId} has not been found!");
            }

            var reservedRoom = await _unitOfWork.RoomRepository.GetByIdAsync(getReservation.RoomId);
            
            if(reservedRoom != null) 
            {
                reservedRoom.IsReserved = false;
                await _unitOfWork.RoomRepository.UpdateAsync(reservedRoom);
            }

            await _unitOfWork.ReservationRepository.RemoveAsync(getReservation);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("The reservation has been successfully removed!");

            return Unit.Value;
        }
    }
}
