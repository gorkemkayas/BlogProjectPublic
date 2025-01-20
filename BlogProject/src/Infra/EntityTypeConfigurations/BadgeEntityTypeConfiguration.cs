using BlogProject.src.Infra.Entitites;
using BlogProject.src.Infra.Entitites.PartialEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class BadgeEntityTypeConfiguration : IEntityTypeConfiguration<BadgeEntity>
    {
        public void Configure(EntityTypeBuilder<BadgeEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.BadgeType)
                   .IsRequired()
                   .HasDefaultValue(BadgeType.Newbie);

            builder.Property(e => e.CreatedDate)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.IsDeleted)
                   .HasDefaultValue(false);
        }
    }
}
