using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class RoleEntityTypeConfiguration : BaseEntityTypeConfiguration<RoleEntity>
    {
        public override void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            //RoleId
            builder.Property(e => e.UserId)
                   .IsRequired();

            builder.HasOne(r => r.User)
                   .WithOne(u => u.Role)
                   .HasForeignKey<RoleEntity>(f => f.UserId);

            base.Configure(builder);
        }
    }
}
