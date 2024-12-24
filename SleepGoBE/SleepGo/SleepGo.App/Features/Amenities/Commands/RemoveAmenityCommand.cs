using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Amenities.Commands
{
    public record RemoveAmenityCommand(Guid id) : IRequest<Unit>;

    public class RemoveAmenityCommandHandler : IRequestHandler<RemoveAmenityCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveAmenityCommandHandler> _logger;

        public RemoveAmenityCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveAmenityCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveAmenityCommand request, CancellationToken cancellationToken)
        {
            var amenityToRemove = await _unitOfWork.AmenityRepository.GetByIdAsync(request.id);

            if(amenityToRemove == null)
            {
                throw new AmenityNotFoundException($"The amenity with id {request.id} has not been found and therefore could not be removed!");
            }

            await _unitOfWork.AmenityRepository.RemoveAsync(amenityToRemove);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Amenity successfully removed!");

            return Unit.Value;
        }
    }
}
