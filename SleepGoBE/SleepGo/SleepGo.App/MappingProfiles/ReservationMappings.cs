using AutoMapper;
using SleepGo.App.DTOs.ReservationDtos;
using SleepGo.Domain.Entities;

namespace SleepGo.App.MappingProfiles
{
    public class ReservationMappings : Profile
    {
        public ReservationMappings()
        {
            CreateMap<Reservation, ResponseReservationDto>()
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Room.Hotel.HotelName));
        }
    }
}
