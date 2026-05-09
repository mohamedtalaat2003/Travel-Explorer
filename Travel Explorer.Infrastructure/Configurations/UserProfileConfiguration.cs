using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Travel_Explorer.Infrastructure.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("user_profiles");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Bio)
                .HasMaxLength(1000);

            builder.Property(p => p.AvatarUrl)
                .HasMaxLength(500);

            builder.Property(p => p.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(p => p.Country)
                .HasMaxLength(100);

            builder.Property(p => p.PassportNumber)
                .HasMaxLength(50);

            builder.HasQueryFilter(p => !p.IsDeleted);

            // One-to-one relationship is configured in ApplicationUserConfiguration, 
            // but we can also define it here for clarity.
            builder.HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
