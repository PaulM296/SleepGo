using AutoMapper;
using SleepGo.App.DTOs.ReservationDtos;
using SleepGo.App.DTOs.RoomDtos;
using SleepGo.Domain.Entities;

namespace SleepGo.App.MappingProfiles
{
    public class RoomMappings : Profile
    {
        public RoomMappings()
        {
            CreateMap<Room, ResponseRoomDto>()
                .ForMember(dest => dest.Reservations, opt => opt.MapFrom(src => src.Reservations));

            CreateMap<Reservation, ResponseReservationDto>();
        }
    }
}
