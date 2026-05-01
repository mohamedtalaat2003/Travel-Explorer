using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Travel_Explorer.Domain.Entities;

namespace Travel_Explorer.Infrastructure.Configurations
{
    public class ActivitieConfiguration : IEntityTypeConfiguration<Activitie>
    {
        public void Configure(EntityTypeBuilder<Activitie> builder)
        {
            builder.ToTable("activities");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name).IsRequired().HasMaxLength(150);
            builder.Property(a => a.Description).HasMaxLength(1000);
            builder.Property(a => a.Icon).HasMaxLength(200);

            builder.HasIndex(a => a.IsDeleted).HasDatabaseName("IX_activities_IsDeleted");

            builder.HasQueryFilter(a => !a.IsDeleted);

            builder.HasOne(a => a.Destination)
                .WithMany(d => d.Activities)
                .HasForeignKey(a => a.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
