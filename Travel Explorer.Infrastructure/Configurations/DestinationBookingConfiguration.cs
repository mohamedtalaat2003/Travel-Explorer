using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Travel_Explorer.Infrastructure.Configurations
{
    public class DestinationBookingConfiguration : IEntityTypeConfiguration<DestinationBooking>
    {
        public void Configure(EntityTypeBuilder<DestinationBooking> builder)
        {
            builder.ToTable("destination_bookings");

            builder.HasKey(db => db.Id);

            builder.Property(db => db.TotalPrice)
                .HasPrecision(18, 2);

            builder.Property(db => db.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(db => db.Notes)
                .HasMaxLength(1000);

            builder.HasQueryFilter(db => !db.IsDeleted);

            // Indexes
            builder.HasIndex(db => db.DestinationId).HasDatabaseName("IX_destinationBookings_DestinationId");
            builder.HasIndex(db => db.UserId).HasDatabaseName("IX_dstinationBookings_UserId");
            builder.HasIndex(db => db.Status).HasDatabaseName("IX_destinationBookings_Status");
            builder.HasIndex(a => a.IsDeleted).HasDatabaseName("IX_destinationBokings_IsDeleted");

            // Relationships
            builder.HasOne(db => db.User)
                .WithMany(u => u.DestinationBookings)
                .HasForeignKey(db => db.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(db => db.Destination)
                .WithMany(d => d.Bookings)
                .HasForeignKey(db => db.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
