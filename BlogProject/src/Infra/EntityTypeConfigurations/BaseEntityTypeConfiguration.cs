using BlogProject.src.Infra.Entitites.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedTime)
                   .HasColumnType("datetime2")
                   .IsRequired();
            builder.Property(e => e.ModifiedTime)
                   .HasColumnType("datetime2")
                   .IsRequired(false);
        }
    }
}
