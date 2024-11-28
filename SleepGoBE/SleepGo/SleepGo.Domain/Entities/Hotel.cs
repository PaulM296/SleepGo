using SleepGo.Domain.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.Domain.Entities
{
    public class Hotel : BaseEntity
    {
        public required string HotelName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string ZipCode { get; set; }
        public required string HotelDescription { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Amenity> Amenities { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
