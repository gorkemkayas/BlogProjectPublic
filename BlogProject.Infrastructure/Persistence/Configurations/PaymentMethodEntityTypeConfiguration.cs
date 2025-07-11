using BlogProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.Infrastructure.Persistence.Configurations
{
    public class PaymentMethodEntityTypeConfiguration : IEntityTypeConfiguration<PaymentMethodEntity>
    {
        public void Configure(EntityTypeBuilder<PaymentMethodEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Details)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(e => e.IsDeleted)
                   .HasDefaultValue(false);

            builder.Property(e => e.IsDefault).HasDefaultValue(false);

            builder.Property(e => e.PaymentMethodType).IsRequired();

            builder.HasOne(e => e.Donation)
                   .WithMany(d => d.PaymentMethods)
                   .HasForeignKey(e => e.DonationId);
        }
    }
}
