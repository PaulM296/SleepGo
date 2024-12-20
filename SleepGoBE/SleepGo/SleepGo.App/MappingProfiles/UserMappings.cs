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
        }
    }
}
