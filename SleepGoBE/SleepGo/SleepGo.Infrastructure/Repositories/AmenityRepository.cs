using Microsoft.EntityFrameworkCore;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;
using SleepGo.Infrastructure.Exceptions;

namespace SleepGo.Infrastructure.Repositories
{
    public class AmenityRepository : BaseRepository<Amenity>, IAmenityRepository
    {
        public AmenityRepository(SleepGoDbContext context) : base(context)
        {

        }

        public async Task<ICollection<Amenity>> GetHotelAmenitiesByHotelIdAsync(Guid hotelId)
        {
            return await _context.Amenities.Where(a => a.HotelId == hotelId).ToListAsync();
        }
    }
}
