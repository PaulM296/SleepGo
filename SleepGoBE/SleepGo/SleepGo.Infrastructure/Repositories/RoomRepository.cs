using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(SleepGoDbContext context) : base(context)
        {

        }
    }
}
