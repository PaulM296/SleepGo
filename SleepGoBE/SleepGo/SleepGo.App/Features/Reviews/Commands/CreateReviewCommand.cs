using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.ReviewDtos;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Features.Reviews.Commands
{
    public record CreateReviewCommand(Guid userId, CreateReviewDto createReviewDto) : IRequest<ResponseReviewDto>;

    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ResponseReviewDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateReviewCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateReviewCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateReviewCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ResponseReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new Review()
            {
                UserId = request.userId,
                HotelId = request.createReviewDto.HotelId,
                ReviewText = request.createReviewDto.ReviewText,
                CreatedAt = DateTime.UtcNow,
                Rating = request.createReviewDto.Rating ?? 0
            };

            var createdReview = await _unitOfWork.ReviewRepository.AddAsync(review);
            await _unitOfWork.SaveAsync();

            await _unitOfWork.HotelRepository.UpdateHotelRatingAsync(request.createReviewDto.HotelId);

            _logger.LogInformation("Review added successfully!");

            return _mapper.Map<ResponseReviewDto>(createdReview);
        }
    }
}
