using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.HotelDtos;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.ReservationDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Features.Hotels.Queries;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Reservations.Queries
{
    public record GetAllUserReservationsQuery(Guid userId, PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseReservationDto>>;

    public class GetAllUserReservationsQueryHandler : IRequestHandler<GetAllUserReservationsQuery, PaginationResponseDto<ResponseReservationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllUserReservationsQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllUserReservationsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllUserReservationsQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<ResponseReservationDto>> Handle(GetAllUserReservationsQuery request, CancellationToken cancellationToken)
        {
            var reservations = await _unitOfWork.ReservationRepository
                .GetAllPagedReservationsByUserIdAsync(request.userId, request.paginationRequestDto.PageIndex, request.paginationRequestDto.PageSize);

            if(reservations.Items.Count == 0)
            {
                throw new ReservationNotFoundException($"Could not get the reservations for UserId {request.userId}, because there aren't any yet!");
            }

            var reservationDtos = new PaginationResponseDto<ResponseReservationDto>(
                items: _mapper.Map<List<ResponseReservationDto>>(reservations.Items),
                pageIndex: reservations.PageIndex,
                totalPages: reservations.TotalPages
                );

            _logger.LogInformation("All user reservations have been successfully retrieved!");

            return reservationDtos;
        }
    }
}
