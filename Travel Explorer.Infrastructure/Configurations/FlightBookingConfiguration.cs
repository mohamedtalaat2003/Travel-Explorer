using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Travel_Explorer.Domain.Entities;

namespace Travel_Explorer.Infrastructure.Configurations
{
    public class FlightBookingConfiguration : IEntityTypeConfiguration<FlightBooking>
    {
        public void Configure(EntityTypeBuilder<FlightBooking> builder)
        {
            builder.ToTable("flight_bookings");

            builder.HasKey(fb => fb.Id);

            builder.Property(fb => fb.TotalPrice)
                .HasPrecision(18, 2);

            builder.Property(fb => fb.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(fb => fb.Class)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(fb => fb.SeatPreference)
                .HasMaxLength(100);

            builder.HasQueryFilter(fb => !fb.IsDeleted);

            // Indexes
            builder.HasIndex(fb => fb.FlightScheduleId).HasDatabaseName("IX_flightBookings_FlightScheduleId");
            builder.HasIndex(fb => fb.UserId).HasDatabaseName("IX_flightBookings_UserId");
            builder.HasIndex(a => a.IsDeleted).HasDatabaseName("IX_flightBookings_IsDeleted");
            // Relationships
            builder.HasOne(fb => fb.User)
                .WithMany(u => u.FlightBookings)
                .HasForeignKey(fb => fb.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(fb => fb.FlightSchedule)
                .WithMany(fs => fs.Bookings)
                .HasForeignKey(fb => fb.FlightScheduleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
