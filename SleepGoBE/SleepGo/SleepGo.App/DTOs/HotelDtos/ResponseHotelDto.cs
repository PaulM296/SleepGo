using SleepGo.App.DTOs.AmenityDtos;
using SleepGo.App.DTOs.ReviewDtos;
using SleepGo.App.DTOs.RoomDtos;
using SleepGo.Domain.Entities;

namespace SleepGo.App.DTOs.HotelDtos
{
    public class ResponseHotelDto
    {
        public Guid Id { get; set; }
        public string HotelName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string HotelDescription { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Rating { get; set; }
        public ICollection<ResponseRoomDto> Rooms { get; set; }
        public ICollection<ResponseAmenityDto> Amenities { get; set; }
        public ICollection<ResponseReviewDto> Reviews { get; set; }
    }
}
