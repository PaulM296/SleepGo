using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Repositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        public HotelRepository(SleepGoDbContext context) : base(context)
        {

        }
    }
}
