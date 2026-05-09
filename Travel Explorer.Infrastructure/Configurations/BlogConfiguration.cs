using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Travel_Explorer.Infrastructure.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("blogs");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(b => b.Content)
                .IsRequired();

            builder.Property(b => b.ImageUrl)
                .HasMaxLength(500);

            builder.HasQueryFilter(b => !b.IsDeleted);

            // Indexes
            builder.HasIndex(b => b.AuthorId).HasDatabaseName("IX_Blogs_AuthorId");
            builder.HasIndex(b => b.CategoryId).HasDatabaseName("IX_Blogs_CategoryId");
            builder.HasIndex(a => a.IsDeleted).HasDatabaseName("IX_blogs_IsDeleted");

            // Relationships
            builder.HasOne(b => b.Author)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Category)
                .WithMany(c => c.Blogs)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
