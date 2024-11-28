using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder
                .HasOne(rv => rv.AppUser)
                .WithMany(u => u.Reviews)
                .HasForeignKey(rv => rv.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(rv => rv.Hotel)
                .WithMany(h => h.Reviews)
                .HasForeignKey(rv => rv.HotelId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
