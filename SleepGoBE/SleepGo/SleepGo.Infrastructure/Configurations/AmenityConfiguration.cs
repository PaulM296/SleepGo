using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Configurations
{
    public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
    {
        public void Configure(EntityTypeBuilder<Amenity> builder)
        {
            builder
                .HasOne(a => a.Hotel)
                .WithMany(h => h.Amenities)
                .HasForeignKey(a => a.HotelId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
