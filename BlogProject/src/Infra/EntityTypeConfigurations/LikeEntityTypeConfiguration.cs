using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class LikeEntityTypeConfiguration : BaseEntityTypeConfiguration<LikeEntity>
    {
        public override void Configure(EntityTypeBuilder<LikeEntity> builder)
        {
            base.Configure(builder);
        }
    }
}
