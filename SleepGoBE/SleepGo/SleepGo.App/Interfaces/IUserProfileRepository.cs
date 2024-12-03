using SleepGo.Domain.Entities;

namespace SleepGo.App.Interfaces
{
    public interface IUserProfileRepository : IBaseRepository<UserProfile>
    {
        Task<UserProfile> GetUserProfileByUserId(Guid userId);
    }
}
