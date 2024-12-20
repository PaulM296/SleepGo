using SleepGo.Domain.Entities;

namespace SleepGo.App.DTOs.HotelDtos
{
    public class HotelResponseDto
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
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Amenity> Amenities { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
