using AutoMapper;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.Domain.Entities;

namespace SleepGo.App.MappingProfiles
{
    public class UserMappings : Profile
    {
        public UserMappings() 
        {
            CreateMap<AppUser, ResponseUserDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserProfile.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserProfile.LastName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.UserProfile.DateOfBirth))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.UserProfile.ProfilePicture))
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.UserProfile.ImageId));

            CreateMap<AppUser, ResponseHotelUserDto>()
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.HotelName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Hotel.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Hotel.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Hotel.Country))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Hotel.ZipCode))
                .ForMember(dest => dest.HotelDescription, opt => opt.MapFrom(src => src.Hotel.HotelDescription))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Hotel.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Hotel.Longitude))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Hotel.Rating))
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Hotel.Rooms))
                .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.Hotel.Amenities))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Hotel.Reviews))
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.Hotel.ImageId));

        }
    }
}
