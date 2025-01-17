using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class FollowEntityTypeConfiguration : IEntityTypeConfiguration<FollowEntity>
    {
        public virtual void Configure(EntityTypeBuilder<FollowEntity> builder)
        {
            // Composite Key ile takip-takip edilen ilişkisinin unique olarak tanımlanmasını sağlıyoruz.
            builder.HasKey(f => new { f.FollowerId, f.FollowingId });

            builder.HasOne(f => f.Follower)
                   .WithMany(u => u.Followings)
                   .HasForeignKey(f => f.FollowerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Following)
                   .WithMany(u => u.Followers)
                   .HasForeignKey(f => f.FollowingId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.FollowDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
