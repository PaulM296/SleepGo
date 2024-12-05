using SleepGo.Domain.Entities;

namespace SleepGo.App.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<ICollection<Review>> GetAllAsync(Guid hotelId);
    }
}
