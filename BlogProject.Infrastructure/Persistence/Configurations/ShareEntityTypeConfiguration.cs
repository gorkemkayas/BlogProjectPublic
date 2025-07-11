using BlogProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.Infrastructure.Persistence.Configurations
{
    public class ShareEntityTypeConfiguration :BaseEntityTypeConfiguration<ShareEntity>
    {
        public override void Configure(EntityTypeBuilder<ShareEntity> builder)
        {
            builder.Property(e => e.Platform).IsRequired();

            builder.Property(e => e.ShareLink).IsRequired();

            builder.Property(e => e.CreatedTime).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(s => s.Post)
                   .WithMany(p => p.Shares)
                   .HasForeignKey(p => p.PostId);

            builder.HasOne(s => s.User)
                   .WithMany(u => u.Shares)
                   .HasForeignKey(f => f.UserId);

            base.Configure(builder);
        }
    }
}
