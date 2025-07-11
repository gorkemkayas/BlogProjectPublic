using BlogProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.Infrastructure.Persistence.Configurations
{
    public class NotificationEntityTypeConfiguration : BaseEntityTypeConfiguration<NotificationEntity>
    {
        public override void Configure(EntityTypeBuilder<NotificationEntity> builder)
        {
            // Message
            builder.Property(e => e.Message)
                   .IsRequired()
                   .HasMaxLength(500);

            // IsRead
            builder.Property(e => e.IsRead)
                   .HasDefaultValue(false);

            // NotificationType
            builder.Property(e => e.NotificationType)
                   .IsRequired();

            // Notification - User relation
            builder.HasOne(e => e.User)
                   .WithMany(u => u.Notifications)
                   .HasForeignKey(f => f.UserId);

            base.Configure(builder);
        }
    }
}
