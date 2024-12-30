using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.ReviewDtos;
using SleepGo.App.DTOs.RoomDtos;
using SleepGo.App.Features.Reviews.Commands;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Features.Rooms.Commands
{
    public record CreateRoomCommand(CreateRoomDto createRoomDto) : IRequest<ResponseRoomDto>;

    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, ResponseRoomDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateRoomCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateRoomCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateRoomCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseRoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = new Room()
            {
                HotelId = request.createRoomDto.HotelId,
                RoomType = request.createRoomDto.RoomType,
                Price = request.createRoomDto.Price,
                RoomNumber = request.createRoomDto.RoomNumber,
                Balcony = request.createRoomDto.Balcony,
                AirConditioning = request.createRoomDto.AirConditioning,
                Kitchenette = request.createRoomDto.Kitchenette,
                Hairdryer = request.createRoomDto.Hairdryer,
                TV = request.createRoomDto.TV,
                IsReserved = request.createRoomDto.IsReserved
            };

            var createdRoom = await _unitOfWork.RoomRepository.AddAsync(room);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Room added successfully!");

            return _mapper.Map<ResponseRoomDto>(createdRoom);
        }
    }
}
