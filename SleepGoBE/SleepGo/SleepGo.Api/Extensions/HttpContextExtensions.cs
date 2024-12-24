using System.Security.Claims;

namespace SleepGo.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserIdClaimValue(this HttpContext context)
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity?.FindFirst("userId")?.Value;

            if(string.IsNullOrEmpty(userIdClaim))
            {
                throw new InvalidOperationException("User ID claim not found.");
            }

            return userIdClaim;
        }
    }
}
