using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder
                .HasOne(up => up.Image)
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.ImageId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(up => up.AppUser)
                .WithOne(u => u.UserProfile)
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
