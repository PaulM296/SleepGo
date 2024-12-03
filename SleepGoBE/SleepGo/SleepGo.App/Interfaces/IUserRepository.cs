using SleepGo.Domain.Entities;
using System.Linq.Expressions;

namespace SleepGo.App.Interfaces
{
    public interface IUserRepository
    {
        void CreateUser(UserProfile userProfile);
        void ChangePassword(Guid id, string newPassword);
        Task<AppUser> GetByIdAsync(Guid id);
        Task<AppUser> RemoveUserAsync(AppUser user);
        Task<AppUser> UpdateUserAsync(AppUser updatedUser);
    }
}
