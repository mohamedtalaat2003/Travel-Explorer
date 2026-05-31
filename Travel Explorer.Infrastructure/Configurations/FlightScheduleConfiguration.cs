using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Travel_Explorer.Infrastructure.Configurations
{
    public class FlightScheduleConfiguration : IEntityTypeConfiguration<FlightSchedule>
    {
        public void Configure(EntityTypeBuilder<FlightSchedule> builder)
        {
            builder.ToTable("flight_schedules");

            builder.HasKey(fs => fs.Id);

            builder.Property(fs => fs.Airline)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(fs => fs.FlightNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(fs => fs.DepartureCity)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(fs => fs.ArrivalCity)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(fs => fs.EconomyPrice)
                .HasPrecision(18, 2);

            builder.Property(fs => fs.BusinessPrice)
                .HasPrecision(18, 2);

            builder.Property(fs => fs.FirstClassPrice)
                .HasPrecision(18, 2);

            builder.HasQueryFilter(fs => !fs.IsDeleted);

            // Optimistic concurrency using PostgreSQL's system xmin column (no extra column added).
            // Makes the seat-decrement DbUpdateConcurrencyException in CreateFlightBooking effective,
            // preventing overselling under concurrent bookings. The xmin mapping is PostgreSQL-specific,
            // so we keep this API despite its generic-EF obsolete notice.
#pragma warning disable CS0618
            builder.UseXminAsConcurrencyToken();
#pragma warning restore CS0618

            builder.HasIndex(fs => fs.FlightNumber).HasDatabaseName("IX_flightschedules_FlightNumber");
             builder.HasIndex(fs => fs.IsDeleted).HasDatabaseName("IX_flightschedules_IsDeleted");
            
        }
    }
}
