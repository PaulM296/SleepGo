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

            getHotel.HotelName = request.updateHotelDto.HotelName;
            getHotel.HotelDescription = request.updateHotelDto.HotelDescription;

            if (!string.IsNullOrEmpty(request.updateHotelDto.Email) && getHotel.AppUser.Email != request.updateHotelDto.Email)
            {
                var emailResult = await _userManager.SetEmailAsync(getHotel.AppUser, request.updateHotelDto.Email);
                if (!emailResult.Succeeded)
                {
                    throw new Exception($"Failed to update email: {string.Join(", ", emailResult.Errors.Select(e => e.Description))}");
                }
            }

            if (!string.IsNullOrEmpty(request.updateHotelDto.PhoneNumber) && getHotel.AppUser.PhoneNumber != request.updateHotelDto.PhoneNumber)
            {
                var phoneResult = await _userManager.SetPhoneNumberAsync(getHotel.AppUser, request.updateHotelDto.PhoneNumber);
                if (!phoneResult.Succeeded)
                {
                    throw new Exception($"Failed to update phone number: {string.Join(", ", phoneResult.Errors.Select(e => e.Description))}");
                }
            }


            var updatedHotel = await _unitOfWork.HotelRepository.UpdateAsync(getHotel);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Hotel updated successfully!");

            return _mapper.Map<ResponseHotelDto>(updatedHotel);
        }
    }
}
