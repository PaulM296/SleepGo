using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder
                .HasMany(h => h.Rooms)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(h => h.Reviews)
                .WithOne(rv => rv.Hotel)
                .HasForeignKey(rv => rv.HotelId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(h => h.Amenities)
                .WithOne(a => a.Hotel)
                .HasForeignKey(a => a.HotelId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
