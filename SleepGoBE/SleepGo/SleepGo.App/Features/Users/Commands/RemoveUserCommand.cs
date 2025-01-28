using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Users.Commands
{
    public record RemoveUserCommand(Guid userId) : IRequest<Unit>;

    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveUserCommandHandler> _logger;

        public RemoveUserCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var userToRemove = await _unitOfWork.UserRepository.GetByIdAsync(request.userId);

            if(userToRemove == null)
            {
                throw new UserNotFoundException($"The user with ID {request.userId} doesn't exist and it could not be removed!");
            }

            var reviews = await _unitOfWork.ReviewRepository.GetAllReviewsByUserIdAsync(userToRemove.Id);
            if(reviews != null)
            {
                foreach(var review in reviews)
                {
                    await _unitOfWork.ReviewRepository.RemoveAsync(review);

                    //Add a recalculating hotel rating once adding a field.
                }
            }

            var reservations = await _unitOfWork.ReservationRepository.GetAllReservationsByUserIdAsync(userToRemove.Id);
            if(reservations != null)
            {
                foreach(var reservation in reservations)
                {
                    var room = await _unitOfWork.RoomRepository.GetByIdAsync(reservation.RoomId);
                    if(room != null)
                    {
                        room.Reservations.Remove(reservation);
                        await _unitOfWork.RoomRepository.UpdateAsync(room);
                    }

                    await _unitOfWork.ReservationRepository.RemoveAsync(reservation);
                }
            }

            await _unitOfWork.UserRepository.RemoveUserAsync(userToRemove);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"User with ID {userToRemove.Id} and all related data have been successfully removed.");

            return Unit.Value;
        }
    }
}
