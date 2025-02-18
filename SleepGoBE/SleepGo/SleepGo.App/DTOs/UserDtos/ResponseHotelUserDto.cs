using SleepGo.App.DTOs.AmenityDtos;
using SleepGo.App.DTOs.ReviewDtos;
using SleepGo.App.DTOs.RoomDtos;
using SleepGo.Domain.Enums;

namespace SleepGo.App.DTOs.UserDtos
{
    public class ResponseHotelUserDto
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
        public string HotelName { get; set; }
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
        public Guid? ImageId { get; set; }
        public bool isBlocked { get; set; }
    }
}
