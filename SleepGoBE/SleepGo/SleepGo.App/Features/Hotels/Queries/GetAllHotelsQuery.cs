using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.HotelDtos;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Hotels.Queries
{
    public record GetAllHotelsQuery(PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseHotelDto>>;

    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, PaginationResponseDto<ResponseHotelDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllHotelsQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllHotelsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllHotelsQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<ResponseHotelDto>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
        {
            var hotels = await _unitOfWork.HotelRepository
                .GetAllPagedHotelsAsync(request.paginationRequestDto.PageIndex, request.paginationRequestDto.PageSize);

            if(hotels.Items.Count == 0)
            {
                throw new HotelNotFoundException("Could not retrieve any hotels, because there are not any at the moment!");
            }

            var hotelDtos = new PaginationResponseDto<ResponseHotelDto>(
                items: _mapper.Map<List<ResponseHotelDto>>(hotels.Items),
                pageIndex: hotels.PageIndex,
                totalPages: hotels.TotalPages);

            _logger.LogInformation("All hotels have been successfully retrieved!");

            return hotelDtos;
        }
    }
}
