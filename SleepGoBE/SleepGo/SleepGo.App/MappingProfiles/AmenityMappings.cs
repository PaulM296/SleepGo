using AutoMapper;
using SleepGo.App.DTOs.AmenityDtos;
using SleepGo.Domain.Entities;

namespace SleepGo.App.MappingProfiles
{
    public class AmenityMappings : Profile
    {
        public AmenityMappings() 
        {
            CreateMap<Amenity, ResponseAmenityDto>();
        }
    }
}
