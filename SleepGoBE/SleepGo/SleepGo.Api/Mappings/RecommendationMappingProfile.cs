using AutoMapper;
using SleepGo.App.DTOs.HotelRecommendationResultDtos;
using SleepGo.MLTrainer.Models;

namespace SleepGo.Api.Mappings
{
    public class RecommendationMappingProfile : Profile
    {
        public RecommendationMappingProfile()
        {
            CreateMap<(HotelRecommendationData Hotel, HotelRecommendationPrediction Prediction, string HotelName), HotelRecommendationResultDto>()
                .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.Hotel.HotelId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Hotel.PricePaid))
                .ForMember(dest => dest.PredictedLabel, opt => opt.MapFrom(src => src.Prediction.PredictedLabel))
                .ForMember(dest => dest.Probability, opt => opt.MapFrom(src => src.Prediction.Probability))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Hotel.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Hotel.Country))
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.HotelName));
        }
    }
}
