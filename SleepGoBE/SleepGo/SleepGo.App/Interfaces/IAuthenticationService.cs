using SleepGo.Domain.Entities;

namespace SleepGo.App.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AppUser> Register(AppUser newUser, UserProfile newUserProfile, string password);
        Task<AppUser> LoginUser(string Email, string Password);
    }
}
