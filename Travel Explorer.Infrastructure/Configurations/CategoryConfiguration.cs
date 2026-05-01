using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Travel_Explorer.Domain.Entities;

namespace Travel_Explorer.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.Property(c => c.IconUrl)
                .HasMaxLength(500);

            builder.HasQueryFilter(c => !c.IsDeleted);

            builder.HasIndex(c => c.Name).IsUnique().HasDatabaseName("IX_categories_Name");
            builder.HasIndex(a => a.IsDeleted).HasDatabaseName("IX_categories_IsDeleted");
        }
    }
}
