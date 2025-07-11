using BlogProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.Infrastructure.Persistence.Configurations
{
    public class TagEntityTypeConfiguration : BaseEntityTypeConfiguration<TagEntity>
    {
        public override void Configure(EntityTypeBuilder<TagEntity> builder)
        {
            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.IsDraft)
                   .HasDefaultValue(false);

            builder.Property(e => e.UsageCount)
                   .HasDefaultValue(0);

            base.Configure(builder);
        }
    }
}
