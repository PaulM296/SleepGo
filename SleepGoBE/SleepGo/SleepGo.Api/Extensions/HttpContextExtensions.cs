using System.Security.Claims;

namespace SleepGo.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetUserIdClaimValue(this HttpContext context)
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity?.FindFirst("userId")?.Value;

            if(string.IsNullOrEmpty(userIdClaim))
            {
                throw new InvalidOperationException("User ID claim not found.");
            }

            if(!Guid.TryParse(userIdClaim, out var userId))
            {
                throw new InvalidOperationException("Invalid user ID claim format.");
            }

            return userId;
        }
    }
}
