using AutoMapper;
using SleepGo.App.DTOs.UserProfileDtos;
using SleepGo.Domain.Entities;

namespace SleepGo.App.MappingProfiles
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings() 
        {
            CreateMap<UserProfile, ResponseUserProfileDto>();
        }
    }
}
