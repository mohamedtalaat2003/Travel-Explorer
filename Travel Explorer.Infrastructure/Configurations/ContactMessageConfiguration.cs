using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Travel_Explorer.Domain.Entities;

namespace Travel_Explorer.Infrastructure.Configurations
{
    public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> builder)
        {
            builder.ToTable("contact_messages");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.FullName).IsRequired().HasMaxLength(200);
            builder.Property(m => m.Email).IsRequired().HasMaxLength(200);
            builder.Property(m => m.Subject).IsRequired().HasMaxLength(300);
            builder.Property(m => m.Message).IsRequired().HasMaxLength(2000);

            
            builder.HasIndex(m => m.Email).HasDatabaseName("IX_ContactMessages_Email");

            builder.HasIndex(m => m.IsDeleted).HasDatabaseName("IX_ContactMessages_IsDeleted");

            builder.HasQueryFilter(m => !m.IsDeleted);

            builder.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
