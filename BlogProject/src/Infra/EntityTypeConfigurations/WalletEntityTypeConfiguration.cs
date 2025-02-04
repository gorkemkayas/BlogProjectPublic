using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class WalletEntityTypeConfiguration : BaseEntityTypeConfiguration<WalletEntity>
    {
        public override void Configure(EntityTypeBuilder<WalletEntity> builder)
        {
            // Balance
            builder.Property(e => e.Balance)
                .IsRequired()
                .HasDefaultValue(0)
                .HasPrecision(18, 2);

            // Currency
            builder.Property(e => e.Currency)
                   .IsRequired();

            // LastUpdated
            builder.Property(e => e.LastUpdated)
                   .ValueGeneratedOnUpdate();

            // RowVersion
            builder.Property(e => e.RowVersion)
                   .IsRowVersion();

            // One to Many
            builder.HasOne(w => w.User)
                   .WithMany(u => u.Wallets)
                   .HasForeignKey(f => f.UserId);

            base.Configure(builder);
        }
    }
}
