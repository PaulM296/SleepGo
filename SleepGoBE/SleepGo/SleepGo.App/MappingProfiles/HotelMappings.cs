using AutoMapper;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SleepGo.App.DTOs.AmenityDtos;
using SleepGo.App.DTOs.HotelDtos;
using SleepGo.App.DTOs.ReviewDtos;
using SleepGo.App.DTOs.RoomDtos;
using SleepGo.Domain.Entities;

namespace SleepGo.App.MappingProfiles
{
    public class HotelMappings : Profile
    {
        public HotelMappings() 
        {
            CreateMap<Hotel, ResponseHotelDto>()
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms))
                .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.Amenities))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));

            CreateMap<Room, ResponseRoomDto>()
                .ForMember(dest => dest.Reservations, opt => opt.MapFrom(src => src.Reservations));

            CreateMap<Amenity, ResponseAmenityDto>();

            CreateMap<Review, ResponseReviewDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUser.UserName));
        }
    }
}
