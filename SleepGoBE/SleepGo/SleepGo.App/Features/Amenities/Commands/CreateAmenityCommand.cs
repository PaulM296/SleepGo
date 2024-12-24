using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.AmenityDtos;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Features.Amenities.Commands
{
    public record CreateAmenityCommand(CreateAmenityDto createAmenityDto) : IRequest<ResponseAmenityDto>;

    public class CreateAmenityCommandHandler : IRequestHandler<CreateAmenityCommand, ResponseAmenityDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateAmenityCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateAmenityCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateAmenityCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseAmenityDto> Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
        {
            var amenity = new Amenity()
            {
                HotelId = request.createAmenityDto.HotelId,
                Pool = request.createAmenityDto.Pool,
                Restaurant = request.createAmenityDto.Restaurant,
                Fitness = request.createAmenityDto.Fitness,
                WiFi = request.createAmenityDto.WiFi,
                RoomService = request.createAmenityDto.RoomService,
                Bar = request.createAmenityDto.Bar
            };

            var createdAmenity = await _unitOfWork.AmenityRepository.AddAsync(amenity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Amenities successfully added!");

            return _mapper.Map<ResponseAmenityDto>(createdAmenity);
        }
    }
}
