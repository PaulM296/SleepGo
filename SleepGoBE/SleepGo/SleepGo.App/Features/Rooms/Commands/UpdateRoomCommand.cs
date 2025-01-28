using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.RoomDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Rooms.Commands
{
    public record UpdateRoomCommand(Guid roomId, UpdateRoomDto updateRoomDto) : IRequest<ResponseRoomDto>;

    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, ResponseRoomDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateRoomCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateRoomCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateRoomCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseRoomDto> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var getRoom = await _unitOfWork.RoomRepository.GetByIdAsync(request.roomId);

            if(getRoom == null)
            {
                throw new RoomNotFoundException($"The room with ID {request.roomId} doesn't exist and it could not be updated!");
            }

            getRoom.RoomType = request.updateRoomDto.RoomType;
            getRoom.Price = request.updateRoomDto.Price;
            getRoom.RoomNumber = request.updateRoomDto.RoomNumber;
            getRoom.Balcony = request.updateRoomDto.Balcony;
            getRoom.AirConditioning = request.updateRoomDto.AirConditioning;
            getRoom.Kitchenette = request.updateRoomDto.Kitchenette;
            getRoom.Hairdryer = request.updateRoomDto.Hairdryer;
            getRoom.TV = request.updateRoomDto.TV;
            getRoom.IsReserved = request.updateRoomDto.IsReserved;

            var updatedRoom = await _unitOfWork.RoomRepository.UpdateAsync(getRoom);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Room successfully updated!");

            return _mapper.Map<ResponseRoomDto>(updatedRoom);
        }
    }
}
