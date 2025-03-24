using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Enums;
using System.Runtime.InteropServices;

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


            if(userToRemove.Role == Role.User)
            {
                var reviews = await _unitOfWork.ReviewRepository.GetAllReviewsByUserIdAsync(userToRemove.Id);
                if (reviews != null)
                {
                    foreach (var review in reviews)
                    {
                        await _unitOfWork.ReviewRepository.RemoveAsync(review);

                    }
                }

                var reservations = await _unitOfWork.ReservationRepository.GetAllReservationsByUserIdAsync(userToRemove.Id);
                if (reservations != null)
                {
                    foreach (var reservation in reservations)
                    {
                        await _unitOfWork.ReservationRepository.RemoveAsync(reservation);

                        var room = await _unitOfWork.RoomRepository.GetByIdAsync(reservation.RoomId);
                        if (room != null)
                        {
                            room.Reservations = room.Reservations.Where(r => r.Id != reservation.Id).ToList();
                            await _unitOfWork.RoomRepository.UpdateAsync(room);
                        }
                    }
                }
            }
            
            if(userToRemove.Role == Role.Hotel)
            {
                var hotelToRemove = await _unitOfWork.HotelRepository.GetHotelProfileByUserId(userToRemove.Id);

                if (hotelToRemove != null) 
                {
                    var rooms = await _unitOfWork.RoomRepository.GetRoomsByHotelIdAsync(hotelToRemove.Id);
                    if(rooms != null)
                    {
                        foreach(var room in rooms)

                        {
                            var roomReservations = await _unitOfWork.ReservationRepository.GetReservationsByRoomIdAsync(room.Id);

                            if(roomReservations != null)
                            {
                                foreach(var res in roomReservations)
                                {
                                    await _unitOfWork.ReservationRepository.RemoveAsync(res);
                                }
                            }

                            await _unitOfWork.RoomRepository.RemoveAsync(room);
                        }
                    }

                    var amenities = await _unitOfWork.AmenityRepository.GetHotelAmenitiesByHotelIdAsync(hotelToRemove.Id);
                    if (amenities != null)
                    {
                        foreach (var amenity in amenities)
                        {
                            await _unitOfWork.AmenityRepository.RemoveAsync(amenity);
                        }
                    }

                    var hotelReviews = await _unitOfWork.ReviewRepository.GetAllReviewsByHotelIdAsync(hotelToRemove.Id);
                    if(hotelReviews != null)
                    {
                        foreach(var review in hotelReviews)
                        {
                            await _unitOfWork.ReviewRepository.RemoveAsync(review);
                        }
                    }

                    await _unitOfWork.HotelRepository.RemoveAsync(hotelToRemove);
                }
            }

            await _unitOfWork.UserRepository.RemoveUserAsync(userToRemove);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"User with ID {userToRemove.Id} and all related data have been successfully removed.");

            return Unit.Value;
        }
    }
}
