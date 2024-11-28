using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder
            .Property(res => res.Price)
            .HasColumnType("decimal(18,2)");

            builder
                .HasOne(res => res.AppUser)
                .WithMany(u => u.Reservations)
                .HasForeignKey(res => res.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(res => res.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(res => res.RoomId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
