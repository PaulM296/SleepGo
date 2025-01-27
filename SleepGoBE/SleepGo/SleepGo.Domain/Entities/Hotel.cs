using SleepGo.Domain.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace SleepGo.Domain.Entities
{
    public class Hotel : BaseEntity
    {
        public string HotelName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string HotelDescription { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Rating { get; set; }
        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid? ImageId { get; set; }
        public Image? Image { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Amenity> Amenities { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
