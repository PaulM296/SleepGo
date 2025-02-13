using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.HotelDtos;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Users.Queries
{
    public record SearchHotelsQuery(string query) : IRequest<IEnumerable<ResponseUserDto>>;

    public class SearchHotelsQueryHandler : IRequestHandler<SearchHotelsQuery, IEnumerable<ResponseUserDto>>
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

        public async Task<IEnumerable<ResponseUserDto>> Handle(SearchHotelsQuery request, CancellationToken cancellationToken)
        {
            var query = request.query.ToLower();
            var hotelUsers = await _unitOfWork.UserRepository
                .FindAsync(u => u.Hotel.HotelName.ToLower().Contains(query) ||
                u.Hotel.Country.ToLower().Contains(query) ||
                u.Hotel.City.ToLower().Contains(query));

            if (!hotelUsers.Any())
            {
                _logger.LogInformation("No hotels found matching the query.");
                return Enumerable.Empty<ResponseUserDto>();
            }

            var hotelUsersDtos = _mapper.Map<IEnumerable<ResponseUserDto>>(hotelUsers);

            _logger.LogInformation("Hotels successfully retrieved.");
            return hotelUsersDtos;
        }
    }
}
