using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class PostTagEntityTypeConfiguration : IEntityTypeConfiguration<PostTagEntity>
    {
        public void Configure(EntityTypeBuilder<PostTagEntity> builder)
        {
            builder.HasKey(k => new {k.PostId,k.TagId});

            builder.Property(e => e.AssignedDate)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(e => e.Post)
                   .WithMany(p => p.TagPosts)
                   .HasForeignKey(f => f.PostId);

            builder.HasOne(e => e.Tag)
                   .WithMany(t => t.TagPosts)
                   .HasForeignKey(f => f.TagId);
        }
    }
}
