using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class CommentEntityTypeConfiguration : BaseEntityTypeConfiguration<CommentEntity>
    {
        public override void Configure(EntityTypeBuilder<CommentEntity> builder)
        {
            // Title
            builder.Property(e => e.Title)
                   .IsRequired(false)
                   .HasMaxLength(50);

            // Content
            builder.Property(e => e.Content)
                   .IsRequired()
                   .HasMaxLength(500);

            // Post - Comment Relation
            builder.HasOne(c => c.Post)
                   .WithMany(p => p.Comments)
                   .HasForeignKey(f => f.PostId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

            // Author - Comment Relation
            builder.HasOne(c => c.Author)
                   .WithMany(a => a.Comments)
                   .HasForeignKey(f => f.AuthorId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

            // ParentComment - Comment Relation
            builder.HasOne(c => c.ParentComment)
                   .WithMany(pc => pc.Replies)
                   .HasForeignKey(f => f.ParentCommentId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);


            base.Configure(builder);
        }
    }
}
