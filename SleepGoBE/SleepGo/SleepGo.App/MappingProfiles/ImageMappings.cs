using AutoMapper;
using SleepGo.App.DTOs.ImageDtos;
using SleepGo.Domain.Entities;

namespace SleepGo.App.MappingProfiles
{
    public class ImageMappings : Profile
    {
        public ImageMappings()
        {
            CreateMap<Image, ImageResponseDto>();
        }
    }
}
