using AutoMapper;
using SleepGo.App.DTOs.ReviewDtos;
using SleepGo.Domain.Entities;

namespace SleepGo.App.MappingProfiles
{
    public class ReviewMappings : Profile
    {
        public ReviewMappings() 
        {
            CreateMap<Review, ResponseReviewDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.HotelName));
        }
    }
}
