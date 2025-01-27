using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.HotelDtos;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Features.Hotels.Commands
{
    public record CreateHotelCommand(CreateHotelDto createHotelDto) : IRequest<ResponseHotelDto>;

    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, ResponseHotelDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateHotelCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateHotelCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateHotelCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseHotelDto> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            var hotel = new Hotel()
            {
                HotelName = request.createHotelDto.HotelName,
                Address = request.createHotelDto.Address,
                City = request.createHotelDto.City,
                Country = request.createHotelDto.Country,
                ZipCode = request.createHotelDto.ZipCode,
                HotelDescription = request.createHotelDto.HotelDescription,
                Latitude = request.createHotelDto.Latitude,
                Longitude = request.createHotelDto.Longitude,
                Rating = request.createHotelDto.Rating,
            };

            var createdHotel = await _unitOfWork.HotelRepository.AddAsync(hotel);

            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Hotel successfully added!");

            return _mapper.Map<ResponseHotelDto>(createdHotel);
        }
    }
}
