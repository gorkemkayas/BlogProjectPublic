using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class MembershipTypeEntityTypeConfiguration : BaseEntityTypeConfiguration<MemberShipTypeEntity>
    {
        public override void Configure(EntityTypeBuilder<MemberShipTypeEntity> builder)
        {
            // Name
            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            // Description
            builder.Property(e => e.Description)
                   .IsRequired()
                   .HasMaxLength(500);

            // One to Many 
            builder.HasOne(mt => mt.UserMemberShipEntity)
                   .WithMany(um => um.MembershipTypes)
                   .HasForeignKey(f => f.UserMemberShipId);

            base.Configure(builder);
        }
    }
}
