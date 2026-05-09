using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Travel_Explorer.Infrastructure.Configurations
{
    public class DestinationConfiguration : IEntityTypeConfiguration<Destination>
    {
        public void Configure(EntityTypeBuilder<Destination> builder)
        {
            builder.ToTable("destinations");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Description)
                .IsRequired();

            builder.Property(d => d.Location)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(d => d.PricePerNight)
                .HasPrecision(18, 2);


            builder.HasQueryFilter(d => !d.IsDeleted);

            // Indexes
            builder.HasIndex(d => d.Name).HasDatabaseName("IX_destinations_Name");
            builder.HasIndex(d => d.CategoryId).HasDatabaseName("IX_destinations_CategoryId");
            builder.HasIndex(a => a.IsDeleted).HasDatabaseName("IX_destinations_IsDeleted");

            // Relationships
            builder.HasOne(d => d.Category)
                .WithMany(c => c.Destinations)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
