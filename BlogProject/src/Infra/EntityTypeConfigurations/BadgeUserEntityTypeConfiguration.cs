using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class BadgeUserEntityTypeConfiguration : IEntityTypeConfiguration<BadgeUserEntity>
    {
        public void Configure(EntityTypeBuilder<BadgeUserEntity> builder)
        {
            builder.HasKey(k => new {k.BadgeId, k.UserId});

            builder.Property(e => e.AwardDate)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(e => e.Badge)
                   .WithMany(b => b.BadgeUsers)
                   .HasForeignKey(f => f.BadgeId);

            builder.HasOne(e => e.User)
                   .WithMany(u => u.BadgeUsers)
                   .HasForeignKey(f => f.UserId);

            builder.Property(e => e.IsDeleted)
                   .HasDefaultValue(false);
        }
    }
}
