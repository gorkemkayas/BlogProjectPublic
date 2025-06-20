using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class CategoryEntityTypeConfiguration : BaseEntityTypeConfiguration<CategoryEntity>
    {
        public override void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            // Title
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Description
            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            // IsDraft
            builder.Property(e => e.IsDraft)
                   .HasDefaultValue(false);

            builder.HasMany(c => c.Posts)
                   .WithOne(p => p.Category)
                   .HasForeignKey(f => f.CategoryId);

            base.Configure(builder);
        }
    }
}
