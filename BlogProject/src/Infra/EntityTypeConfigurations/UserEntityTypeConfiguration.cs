using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class UserEntityTypeConfiguration :BaseUserEntityTypeConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {


            base.Configure(builder);
        }
    }
}
