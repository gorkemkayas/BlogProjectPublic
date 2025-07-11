using BlogProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.Infrastructure.Persistence.Configurations
{
    public class FollowEntityTypeConfiguration : IEntityTypeConfiguration<FollowEntity>
    {
        public virtual void Configure(EntityTypeBuilder<FollowEntity> builder)
        {
            // Composite Key ile takip-takip edilen ilişkisinin unique olarak tanımlanmasını sağlıyoruz.
            builder.HasKey(f => new { f.FollowerId, f.FollowingId });

            builder.Property(e => e.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasOne(f => f.Follower)
                   .WithMany(u => u.Followings)
                   .HasForeignKey(f => f.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Following)
                   .WithMany(u => u.Followers)
                   .HasForeignKey(f => f.FollowingId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.FollowDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
