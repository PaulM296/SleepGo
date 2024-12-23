using AutoMapper;
using MediatR;
using SleepGo.App.DTOs.ImageDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Images.Queries
{
    public record GetImageByIdQuery(Guid id) : IRequest<ImageResponseDto>;

    public class GetImageByIdQueryHandler : IRequestHandler<GetImageByIdQuery, ImageResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetImageByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ImageResponseDto> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
        {
            var image = await _unitOfWork.ImageRepository.GetImageById(request.id);

            if(image == null)
            {
                throw new ImageNotFoundException("Image not found!");
            }

            return _mapper.Map<ImageResponseDto>(image);
        }
    }
}
