using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(e => e.Content)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(e => e.IsRead)
                   .HasDefaultValue(false);

            builder.Property(e => e.SentDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(e => e.Sender)
                   .WithMany(s => s.SentMessages)
                   .HasForeignKey(e => e.SenderId);

            builder.HasOne(e => e.Receiver)
                   .WithMany(r => r.ReceivedMessages)
                   .HasForeignKey(f => f.ReceiverId);
        }
    }
}
