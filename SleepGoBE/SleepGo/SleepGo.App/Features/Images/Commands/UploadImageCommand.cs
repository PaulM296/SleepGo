using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.ImageDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Features.Images.Commands
{
    public record UploadImageCommand(UploadImageDto uploadImageDto) : IRequest<ImageResponseDto>;

    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, ImageResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UploadImageCommandHandler> _logger;

        public UploadImageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UploadImageCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ImageResponseDto> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            if(request.uploadImageDto.File == null || request.uploadImageDto.File.Length == 0)
            {
                throw new ImageNotFoundException("The imahe has not been found!");
            }

            if (!IsValidFortmatImage(request.uploadImageDto.File.ContentType))
            {
                throw new InvalidImageFormatException("Invalid file format! Only .png, .jpg and .jpeg formats are allowed!");
            }

            using var memoryStream = new MemoryStream();
            await request.uploadImageDto.File.CopyToAsync(memoryStream, cancellationToken);

            var image = new Image
            {
                Name = request.uploadImageDto.File.Name,
                Type = request.uploadImageDto.File.ContentType,
                Data = memoryStream.ToArray()
            };

            await _unitOfWork.ImageRepository.UploadImage(image);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Image successfully uploaded!");

            return _mapper.Map<ImageResponseDto>(image);
        }

        private bool IsValidFortmatImage(string contentType)
        {
            return contentType == "image/jpeg" || contentType == "image/png" || contentType == "image/jpg";
        }
    }
}
