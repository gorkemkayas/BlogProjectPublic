using BlogProject.src.Infra.Entitites;
using BlogProject.src.Infra.Entitites.PartialEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public virtual void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(p => p.RegisteredDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
