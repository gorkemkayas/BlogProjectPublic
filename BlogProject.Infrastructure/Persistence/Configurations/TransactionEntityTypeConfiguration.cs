using BlogProject.Domain.Entities;
using BlogProject.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.Infrastructure.Persistence.Configurations
{
    public class TransactionEntityTypeConfiguration : BaseEntityTypeConfiguration<TransactionEntity>
    {
        public override void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            // Amount
            builder.Property(e => e.Amount)
                   .IsRequired()
                   .HasPrecision(18, 2);

            // Status
            builder.Property(e => e.Status)
                   .IsRequired()
                   .HasDefaultValue(Status.Pending);

            // TransactionType
            builder.Property(e =>e.TransactionType)
                   .IsRequired();

            // One to Many
            builder.HasOne(tr => tr.Wallet)
                   .WithMany(w => w.TransactionEntities)
                   .HasForeignKey(f => f.WalletId);

            base.Configure(builder);
        }
    }
}
