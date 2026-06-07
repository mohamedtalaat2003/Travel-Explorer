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

            builder.Property(fs => fs.AvailableEconomySeats)
                .IsConcurrencyToken();

            builder.Property(fs => fs.AvailableBusinessSeats)
                .IsConcurrencyToken();

            builder.Property(fs => fs.AvailableFirstClassSeats)
                .IsConcurrencyToken();

            builder.HasQueryFilter(fs => !fs.IsDeleted);

#pragma warning disable CS0618
            builder.UseXminAsConcurrencyToken();
#pragma warning restore CS0618

            builder.HasIndex(fs => fs.FlightNumber).HasDatabaseName("IX_flightschedules_FlightNumber");
             builder.HasIndex(fs => fs.IsDeleted).HasDatabaseName("IX_flightschedules_IsDeleted");
        }
    }
}
