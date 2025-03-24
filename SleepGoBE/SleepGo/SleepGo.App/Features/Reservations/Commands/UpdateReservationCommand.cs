using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.ReservationDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Features.Hotels.Commands;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Reservations.Commands
{
    public record UpdateReservationCommand(Guid reservationId, UpdateReservationDto updateReservationDto) : IRequest<ResponseReservationDto>;

    public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, ResponseReservationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateReservationCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateReservationCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateReservationCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ResponseReservationDto> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {
            var getReservation = await _unitOfWork.ReservationRepository.GetByIdAsync(request.reservationId);

            if(getReservation == null)
            {
                throw new ReservationNotFoundException($"The reservation with ID {request.reservationId} has not been found!");
            }

            getReservation.Status = request.updateReservationDto.Status;

            if (getReservation.Payment != null && request.updateReservationDto.Status == "Successful")
            {
                getReservation.Payment.Status = "Succeeded";
                getReservation.Payment.PaidAt = DateTime.UtcNow;
            }

            var updatedReservation = await _unitOfWork.ReservationRepository.UpdateAsync(getReservation);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Reservation successfully updated!");

            return _mapper.Map<ResponseReservationDto>(updatedReservation);
        }
    }
}
