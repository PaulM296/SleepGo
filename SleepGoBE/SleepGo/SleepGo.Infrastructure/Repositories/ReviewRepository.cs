using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(SleepGoDbContext context) : base(context)
        {

        }
    }
}
