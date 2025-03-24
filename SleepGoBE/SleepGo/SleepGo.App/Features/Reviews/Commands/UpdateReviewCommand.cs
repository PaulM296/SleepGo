using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.ReviewDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Reviews.Commands
{
    public record UpdateReviewCommand(Guid userId, Guid reviewId, UpdateReviewDto updateReviewDto) : IRequest<ResponseReviewDto>;

    public class UpdateReviewCommandHander : IRequestHandler<UpdateReviewCommand, ResponseReviewDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateReviewCommandHander> _logger;
        private readonly IMapper _mapper;

        public UpdateReviewCommandHander(IUnitOfWork unitOfWork, ILogger<UpdateReviewCommandHander> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseReviewDto> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var getReview = await _unitOfWork.ReviewRepository.GetByIdAsync(request.reviewId);

            if(getReview == null)
            {
                throw new ReviewNotFoundException($"The review with ID {request.reviewId} doesn't exist and it could not be updated!");
            }

            getReview.ReviewText = request.updateReviewDto.ReviewText;
            bool ratingUpdated = false;

            if (request.updateReviewDto.Rating.HasValue)
            {
                getReview.Rating = request.updateReviewDto.Rating.Value;
                ratingUpdated = true;
            }

            var updatedReview = await _unitOfWork.ReviewRepository.UpdateAsync(getReview);
            await _unitOfWork.SaveAsync();

            if (ratingUpdated)
            {
                await _unitOfWork.HotelRepository.UpdateHotelRatingAsync(updatedReview.HotelId);
            }

            _logger.LogInformation("Review successfully updated!");

            return _mapper.Map<ResponseReviewDto>(updatedReview);
        }
    }
}
