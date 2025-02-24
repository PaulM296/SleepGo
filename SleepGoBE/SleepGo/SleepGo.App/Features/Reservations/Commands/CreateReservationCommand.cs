using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.ReservationDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Features.Reservations.Commands
{
    public record CreateReservationCommand(Guid userId, CreateReservationDto createReservationDto) : IRequest<ResponseReservationDto>;

    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ResponseReservationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateReservationCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateReservationCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateReservationCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseReservationDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var availableRoom = await _unitOfWork.RoomRepository.GetAvailableRoomsFromHotelByRoomTypeAsync(request.createReservationDto.HotelId,
                request.createReservationDto.RoomType);

            if(availableRoom == null)
            {
                throw new RoomNotFoundException($"No available room was found for RoomType {request.createReservationDto.RoomType} at HotelId {request.createReservationDto.HotelId}");
            }

            int totalStayingDays = (request.createReservationDto.CheckOut - request.createReservationDto.CheckIn).Days;

            if(request.createReservationDto.CheckIn < DateTime.Today)
            {
                throw new InvalidOperationException("Check-in date cannot be in the past.");
            }

            if(totalStayingDays <= 0)
            {
                throw new InvalidOperationException("Check-out date must be after the check-in date!");
            }

            var totalStayingPrice = availableRoom.Price * totalStayingDays;

            var reservation = new Reservation()
            {
                UserId = request.userId,
                RoomId = availableRoom.Id,
                CheckIn = request.createReservationDto.CheckIn,
                CheckOut = request.createReservationDto.CheckOut,
                Price = totalStayingPrice,
                Status = request.createReservationDto.Status
            };

            availableRoom.IsReserved = true;

            await _unitOfWork.ReservationRepository.AddAsync(reservation);
            await _unitOfWork.RoomRepository.UpdateAsync(availableRoom);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"Reservation has been created for User {request.userId} for room {availableRoom.Id}");

            return _mapper.Map<ResponseReservationDto>(reservation);
        }
    }
}
