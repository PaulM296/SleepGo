using Microsoft.AspNetCore.Identity;
using SleepGo.Domain.Enums;

namespace SleepGo.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public Role Role { get; set; }
        public required UserProfile UserProfile { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public bool IsBlocked { get; set; }
    }
}
