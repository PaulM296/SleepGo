using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.ReviewDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Reviews.Queries
{
    public record GetAllHotelReviewsQuery(Guid HotelId, PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseReviewDto>>;

    public class GetAllHotelReviewsQueryHandler : IRequestHandler<GetAllHotelReviewsQuery, PaginationResponseDto<ResponseReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllHotelReviewsQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllHotelReviewsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllHotelReviewsQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<ResponseReviewDto>> Handle(GetAllHotelReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _unitOfWork.ReviewRepository
                .GetAllPagedReviewsByHotelIdAsync(request.HotelId, request.paginationRequestDto.PageIndex, request.paginationRequestDto.PageSize);

            if(reviews.Items.Count == 0)
            {
                throw new ReviewNotFoundException($"Could not retireve any reviews from HotelId {request.HotelId}, because it doesn't have any yet!");
            }

            var reviewDtos = new PaginationResponseDto<ResponseReviewDto>(
                items: _mapper.Map<List<ResponseReviewDto>>(reviews.Items),
                pageIndex: reviews.PageIndex,
                totalPages: reviews.TotalPages);

            _logger.LogInformation("All reviews have been successfully retrieved!");

            return reviewDtos;
        }
    }
}
