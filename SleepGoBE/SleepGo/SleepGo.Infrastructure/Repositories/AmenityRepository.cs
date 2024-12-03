using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Repositories
{
    public class AmenityRepository : BaseRepository<Amenity>, IAmenityRepository
    {
        public AmenityRepository(SleepGoDbContext context) : base(context)
        {

        }
    }
}
