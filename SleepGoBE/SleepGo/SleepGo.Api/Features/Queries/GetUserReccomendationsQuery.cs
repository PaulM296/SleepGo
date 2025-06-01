//using AutoMapper;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using SleepGo.Api.Interfaces;
//using SleepGo.Api.Services;
//using SleepGo.App.DTOs.HotelRecommendationResultDtos;
//using SleepGo.App.Exceptions;
//using SleepGo.App.Interfaces;

//namespace SleepGo.Api.Features.Queries
//{
//    public record GetUserRecommendationsQuery(Guid userId) : IRequest<List<HotelRecommendationResultDto>>;

//    public class GetUserRecommendationsQueryHandler : IRequestHandler<GetUserRecommendationsQuery, List<HotelRecommendationResultDto>>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly ILogger<GetUserRecommendationsQueryHandler> _logger;
//        private readonly IMapper _mapper;
//        private readonly IRecommendationService _recommendationService;

//        public GetUserRecommendationsQueryHandler(
//            IUnitOfWork unitOfWork,
//            ILogger<GetUserRecommendationsQueryHandler> logger,
//            IMapper mapper,
//            IRecommendationService recommendationService)
//        {
//            _unitOfWork = unitOfWork;
//            _logger = logger;
//            _mapper = mapper;
//            _recommendationService = recommendationService;
//        }

//        public async Task<List<HotelRecommendationResultDto>> Handle(GetUserRecommendationsQuery request, CancellationToken cancellationToken)
//        {
//            var rooms = await _unitOfWork.RoomRepository
//                .GetQueryable()
//                .Include(r => r.Hotel)
//                .Where(r => !r.IsReserved)
//                .ToListAsync(cancellationToken);

//            if (!rooms.Any())
//            {
//                _logger.LogWarning("No available rooms found to recommend.");
//                throw new HotelRecommendationException("No available hotels for recommendation at the moment.");
//            }

//            var predictions = _recommendationService.RecommendTopHotels(request.userId, rooms);

//            var result = _mapper.Map<List<HotelRecommendationResultDto>>(predictions);

//            _logger.LogInformation("Hotel recommendations successfully retrieved.");
//            return result;
//        }
//    }
//}
