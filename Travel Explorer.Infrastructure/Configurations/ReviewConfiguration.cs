using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Travel_Explorer.Domain.Entities;

namespace Travel_Explorer.Infrastructure.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("reviews");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.Property(r => r.Comment)
                .HasMaxLength(1000);

            builder.HasQueryFilter(r => !r.IsDeleted);

            // Indexes
            builder.HasIndex(r => r.DestinationId).HasDatabaseName("IX_Reviews_DestinationId");
            builder.HasIndex(r => r.UserId).HasDatabaseName("IX_Reviews_UserId");
            builder.HasIndex(r => r.IsDeleted).HasDatabaseName("IX_Reviews_IsDeleted");

            // Relationships
            builder.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Destination)
                .WithMany(d => d.Reviews)
                .HasForeignKey(r => r.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
