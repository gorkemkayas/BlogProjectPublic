using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class PostEntityTypeConfiguration : BaseEntityTypeConfiguration<PostEntity>
    {
        public override void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            // Title
            builder.Property(e => e.Title)
                   .IsRequired()
                   .HasMaxLength(50);

            // Subtitle
            builder.Property(e => e.Subtitle)
                   .IsRequired()
                   .HasMaxLength(100);

            // Content
            builder.Property(e => e.Content)
                   .IsRequired()
                   .HasMaxLength(5000);

            // CoverImageUrl
            
            // IsDraft
            builder.Property(e => e.IsDraft).HasDefaultValue(true);

            // ViewCount
            builder.Property(e => e.ViewCount).HasDefaultValue(0);

            // One to many relation
            builder.HasOne(e => e.Author)
                   .WithMany(a => a.Posts)
                   .HasForeignKey(f => f.AuthorId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
