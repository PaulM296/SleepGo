using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.HotelDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Features.Hotels.Commands
{
    public record UpdateHotelCommand(Guid hotelId, UpdateHotelDto updateHotelDto) : IRequest<ResponseHotelDto>;

    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, ResponseHotelDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateHotelCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public UpdateHotelCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateHotelCommandHandler> logger, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ResponseHotelDto> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            var getHotel = await _unitOfWork.HotelRepository.GetByIdAsync(request.hotelId);

            if(getHotel == null)
            {
                throw new HotelNotFoundException($"The hotel with the ID {request.hotelId} has not been found!");
            }

            var hotelUser = await _userManager.FindByIdAsync(getHotel.UserId.ToString());
            if (hotelUser == null)
            {
                throw new UserProfileNotFoundException($"Associated AppUser for Hotel ID {request.hotelId} was not found.");
            }

            if (request.updateHotelDto.HotelImageFile != null)
            {
                var allowedFormats = new[] { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(request.updateHotelDto.HotelImageFile.FileName).ToLower();
                if (!allowedFormats.Contains(fileExtension))
                {
                    throw new Exception("Invalid image format. Only .png, .jpg, and .jpeg are allowed.");
                }

                byte[] imageData;
                using (var memoryStream = new MemoryStream())
                {
                    await request.updateHotelDto.HotelImageFile.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                }

                var image = new Image
                {
                    Id = Guid.NewGuid(),
                    Name = request.updateHotelDto.HotelImageFile.FileName,
                    Type = request.updateHotelDto.HotelImageFile.ContentType,
                    Data = imageData
                };

                await _unitOfWork.ImageRepository.UploadImage(image);
                await _unitOfWork.SaveAsync();

                getHotel.ImageId = image.Id;
            }

            getHotel.HotelName = request.updateHotelDto.HotelName;
            getHotel.HotelDescription = request.updateHotelDto.HotelDescription;
            getHotel.City = request.updateHotelDto.City;
            getHotel.Country = request.updateHotelDto.Country;
            getHotel.ZipCode = request.updateHotelDto.ZipCode;
            getHotel.Address = request.updateHotelDto.Address;
            getHotel.Latitude = request.updateHotelDto.Latitude;
            getHotel.Longitude = request.updateHotelDto.Longitude;

            if (hotelUser.Email != request.updateHotelDto.Email)
            {
                var emailResult = await _userManager.SetEmailAsync(hotelUser, request.updateHotelDto.Email);
                if (!emailResult.Succeeded)
                {
                    var errors = string.Join(", ", emailResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update email: {errors}");
                }
            }

            if (hotelUser.PhoneNumber != request.updateHotelDto.PhoneNumber)
            {
                var phoneResult = await _userManager.SetPhoneNumberAsync(hotelUser, request.updateHotelDto.PhoneNumber);
                if (!phoneResult.Succeeded)
                {
                    var errors = string.Join(", ", phoneResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update phone number: {errors}");
                }
            }

            var updatedHotel = await _unitOfWork.HotelRepository.UpdateAsync(getHotel);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Hotel updated successfully!");

            return _mapper.Map<ResponseHotelDto>(updatedHotel);
        }
    }
}
