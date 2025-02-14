using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.HotelDtos;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Enums;

namespace SleepGo.App.Features.Users.Queries
{
    public record SearchHotelsQuery(string query, PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseHotelUserDto>>;

    public class SearchHotelsQueryHandler : IRequestHandler<SearchHotelsQuery, PaginationResponseDto<ResponseHotelUserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchHotelsQuery> _logger;
        private readonly IMapper _mapper;

        public SearchHotelsQueryHandler(IUnitOfWork unitOfWork, ILogger<SearchHotelsQuery> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<ResponseHotelUserDto>> Handle(SearchHotelsQuery request, CancellationToken cancellationToken)
        {
            var query = request.query.ToLower();
            var filteredHotels = await _unitOfWork.UserRepository
                .FindAsync(u => u.Role == Role.Hotel && 
                (u.Hotel.HotelName.ToLower().Contains(query) ||
                u.Hotel.Country.ToLower().Contains(query) ||
                u.Hotel.City.ToLower().Contains(query)));

            if (!filteredHotels.Any())
            {
                _logger.LogInformation("No hotels found matching the query.");
                return new PaginationResponseDto<ResponseHotelUserDto>(new List<ResponseHotelUserDto>(), request.paginationRequestDto.PageIndex, 0);
            }

            var totalCount = filteredHotels.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.paginationRequestDto.PageSize);
            var paginatedHotels = filteredHotels
                .Skip((request.paginationRequestDto.PageIndex - 1) * request.paginationRequestDto.PageSize)
                .Take(request.paginationRequestDto.PageSize)
                .ToList();

            var hotelUsersDtos = _mapper.Map<IEnumerable<ResponseHotelUserDto>>(paginatedHotels);

            _logger.LogInformation("Hotels successfully retrieved.");
            return new PaginationResponseDto<ResponseHotelUserDto>(hotelUsersDtos.ToList(), request.paginationRequestDto.PageIndex, totalPages);
        }
    }
}
