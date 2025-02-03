using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Users.Queries
{
    public record GetPaginatedHotelsByIdQuery(PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseHotelUserDto>>;

    public class GetPaginatedHotelsByIdQueryHandler : IRequestHandler<GetPaginatedHotelsByIdQuery, PaginationResponseDto<ResponseHotelUserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetPaginatedHotelsByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetPaginatedHotelsByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPaginatedHotelsByIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<PaginationResponseDto<ResponseHotelUserDto>> Handle(GetPaginatedHotelsByIdQuery request, CancellationToken cancellationToken)
        {
            var paginatedHotels = await _unitOfWork.UserRepository.GetPaginatedHotelsByIdAsync(request.paginationRequestDto.PageIndex, request.paginationRequestDto.PageSize);

            if (paginatedHotels.Items.Count == 0)
            {
                throw new UserNotFoundException($"There are no hotels on the platform!");
            }

            var mappedHotels = _mapper.Map<List<ResponseHotelUserDto>>(paginatedHotels.Items);

            var response = new PaginationResponseDto<ResponseHotelUserDto>(
                items: mappedHotels,
                pageIndex: paginatedHotels.PageIndex,
                totalPages: paginatedHotels.TotalPages
            );

            _logger.LogInformation($"Users successfully retrieved!");

            return response;
        }
    }
}
