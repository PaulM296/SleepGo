using System.Security.Claims;

namespace SleepGo.App.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserIdClaimValue(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
