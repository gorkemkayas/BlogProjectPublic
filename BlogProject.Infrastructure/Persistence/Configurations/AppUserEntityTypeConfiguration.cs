using BlogProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.Infrastructure.Persistence.Configurations
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
