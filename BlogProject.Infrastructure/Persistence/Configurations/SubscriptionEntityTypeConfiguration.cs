using BlogProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.Infrastructure.Persistence.Configurations
{
    public class SubscriptionEntityTypeConfiguration : IEntityTypeConfiguration<SubscriptionEntity>
    {
        public void Configure(EntityTypeBuilder<SubscriptionEntity> builder)
        {
            builder.HasKey(k => new { k.FollowerId, k.FollowingId });

            builder.Property(e => e.IsDeleted)
                   .HasDefaultValue(false);

            builder.Property(e => e.IsActive).HasDefaultValue(false);

            builder.Property(e => e.SubscriptionDate)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(s => s.Follower)
                   .WithMany(f => f.Subscribers)
                   .HasForeignKey(s => s.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Following)
                   .WithMany(f => f.Subscriptions)
                   .HasForeignKey(n => n.FollowingId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
