using Microsoft.EntityFrameworkCore;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;
using SleepGo.Infrastructure.Exceptions;

namespace SleepGo.Infrastructure.Repositories
{
    public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(SleepGoDbContext context) : base(context)
        {

        }

        public async Task<UserProfile> GetUserProfileByUserId(Guid userId)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserId == userId);
        }
    }
}
