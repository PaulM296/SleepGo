using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.AmenityDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Amenities.Commands
{
    public record UpdateAmenityCommand(Guid id, UpdateAmenityDto updateAmenityDto) : IRequest<ResponseAmenityDto>;

    public class UpdateAmenityCommandHandler : IRequestHandler<UpdateAmenityCommand, ResponseAmenityDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateAmenityCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateAmenityCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateAmenityCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseAmenityDto> Handle(UpdateAmenityCommand request, CancellationToken cancellationToken)
        {
            var getAmenities = await _unitOfWork.AmenityRepository.GetByIdAsync(request.id);

            if(getAmenities == null)
            {
                throw new AmenityNotFoundException($"The amenities with the id {request.id} don't exist and could not be updated.");
            }

            getAmenities.Pool = request.updateAmenityDto.Pool;
            getAmenities.Restaurant = request.updateAmenityDto.Restaurant;
            getAmenities.WiFi = request.updateAmenityDto.WiFi;
            getAmenities.Fitness = request.updateAmenityDto.Fitness;
            getAmenities.RoomService = request.updateAmenityDto.RoomService;
            getAmenities.Bar = request.updateAmenityDto.Bar;

            var updatedAmenities = await _unitOfWork.AmenityRepository.UpdateAsync(getAmenities);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Amenities successfully updated!");

            return _mapper.Map<ResponseAmenityDto>(updatedAmenities);
        }
    }
}
