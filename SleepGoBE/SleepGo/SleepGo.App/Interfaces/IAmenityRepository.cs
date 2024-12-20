using SleepGo.Domain.Entities;
using System.Runtime.InteropServices;

namespace SleepGo.App.Interfaces
{
    public interface IAmenityRepository : IBaseRepository<Amenity>
    {
        Task<ICollection<Amenity>> GetHotelAmenitiesByHotelIdAsync(Guid hotelId);
    }
}
