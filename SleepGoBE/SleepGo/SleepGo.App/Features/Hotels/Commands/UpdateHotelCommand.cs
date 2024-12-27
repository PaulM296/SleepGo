using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.HotelDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Hotels.Commands
{
    public record UpdateHotelCommand(Guid hotelId, UpdateHotelDto updateHotelDto) : IRequest<ResponseHotelDto>;

    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, ResponseHotelDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateHotelCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateHotelCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateHotelCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseHotelDto> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            var getHotel = await _unitOfWork.HotelRepository.GetByIdAsync(request.hotelId);

            if(getHotel == null)
            {
                throw new HotelNotFoundException($"The hotel with the ID {request.hotelId} has not been found!");
            }

            getHotel.HotelName = request.updateHotelDto.HotelName;
            getHotel.HotelDescription = request.updateHotelDto.HotelDescription;
            getHotel.Email = request.updateHotelDto.Email;
            getHotel.PhoneNumber = request.updateHotelDto.PhoneNumber;

            var updatedHotel = await _unitOfWork.HotelRepository.UpdateAsync(getHotel);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Hotel updated successfully!");

            return _mapper.Map<ResponseHotelDto>(updatedHotel);
        }
    }
}
