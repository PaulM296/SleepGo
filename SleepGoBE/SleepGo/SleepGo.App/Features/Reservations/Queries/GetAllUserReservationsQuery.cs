using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.HotelDtos;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.PaymentDtos;
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

            var reservationDtos = reservations.Items.Select(r => new ResponseReservationDto
            {
                Id = r.Id,
                UserId = r.UserId,
                RoomId = r.RoomId,
                CheckIn = r.CheckIn,
                CheckOut = r.CheckOut,
                Price = r.Price,
                Status = r.Status,
                HotelName = r.Room.Hotel.HotelName,
                Payment = r.Payment != null ? new PaymentDto
                {
                    ClientSecret = r.Payment.ClientSecret,
                    PaymentIntentId = r.Payment.PaymentIntentId,
                    Status = r.Payment.Status,
                    PaidAt = r.Payment.PaidAt,
                    Amount = r.Payment.Amount,
                    Currency = r.Payment.Currency
                } : null
            }).ToList();

            var pagedReservationDtos = new PaginationResponseDto<ResponseReservationDto>(
                items: reservationDtos,
                pageIndex: reservations.PageIndex,
                totalPages: reservations.TotalPages
                );

            _logger.LogInformation("All user reservations have been successfully retrieved!");

            return pagedReservationDtos;
        }
    }
}
