using SleepGo.Domain.Entities;
using System.Security.Claims;

namespace SleepGo.App.Interfaces
{
    public interface IIdentityService
    {
        string CreateSecurityToken(ClaimsIdentity identity);
        ClaimsIdentity CreateClaimsIdentity(AppUser newUser);
    }
}
