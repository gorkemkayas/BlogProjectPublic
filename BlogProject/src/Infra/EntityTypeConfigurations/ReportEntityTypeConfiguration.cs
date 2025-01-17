using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class ReportEntityTypeConfiguration : BaseEntityTypeConfiguration<ReportEntity>
    {
        public override void Configure(EntityTypeBuilder<ReportEntity> builder)
        {
            builder.Property(e => e.Reason)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(e => e.Status).IsRequired();

            builder.HasOne(e => e.ReporterUser)
                   .WithMany(r => r.CreatedReports)
                   .HasForeignKey(e => e.ReporterId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);

            builder.HasOne(e => e.ReportedUser)
                   .WithMany(ru => ru.ReceivedReports)
                   .HasForeignKey(f => f.ReporteduserId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);

            builder.HasOne(e => e.ReportedPost)
                   .WithMany(rp => rp.Reports)
                   .HasForeignKey(f => f.ReportedPostId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
