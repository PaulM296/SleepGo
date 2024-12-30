using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Exceptions;
using SleepGo.App.Features.Reviews.Commands;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Rooms.Commands
{
    public record RemoveRoomCommand(Guid roomId) : IRequest<Unit>;

    public class RemoveRoomCommandHandler : IRequestHandler<RemoveRoomCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveRoomCommandHandler> _logger;

        public RemoveRoomCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveRoomCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveRoomCommand request, CancellationToken cancellationToken)
        {
            var roomToRemove = await _unitOfWork.RoomRepository.GetByIdAsync(request.roomId);

            if(roomToRemove == null)
            {
                throw new RoomNotFoundException($"The room with roomId {request.roomId} has not been found and therefore could not be removed!");
            }

            await _unitOfWork.RoomRepository.RemoveAsync(roomToRemove);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("The room has been successfully removed!");

            return Unit.Value;
        }
    }
}
