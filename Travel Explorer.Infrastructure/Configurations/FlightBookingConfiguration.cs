using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            
            builder.HasIndex(fb => fb.FlightScheduleId).HasDatabaseName("IX_flightBookings_FlightScheduleId");
            builder.HasIndex(fb => fb.UserId).HasDatabaseName("IX_flightBookings_UserId");
            builder.HasIndex(fb => fb.PaymentId).HasDatabaseName("IX_flightBookings_PaymentId");
            builder.HasIndex(a => a.IsDeleted).HasDatabaseName("IX_flightBookings_IsDeleted");
            
            builder.HasOne(fb => fb.User)
                .WithMany(u => u.FlightBookings)
                .HasForeignKey(fb => fb.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(fb => fb.FlightSchedule)
                .WithMany(fs => fs.Bookings)
                .HasForeignKey(fb => fb.FlightScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(fb => fb.Payment)
                .WithOne()
                .HasForeignKey<FlightBooking>(fb => fb.PaymentId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
