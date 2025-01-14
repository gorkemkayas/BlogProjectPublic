using BlogProject.src.Infra.Entitites.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.src.Infra.EntityTypeConfigurations
{
    public class BaseUserEntityTypeConfiguration<TEntity> : BaseEntityTypeConfiguration<TEntity> where TEntity : BaseUserEntity
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Username
            builder.Property(e => e.Username)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasCheckConstraint("CK_Username_MinLength", "LEN(Username) >=5");

            // Email
            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasCheckConstraint("CK_Email_EndsWith", "Email LIKE '%.com' AND Email LIKE '%@%'");

            // Password
            builder.Property(e => e.Password)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnType("nvarchar(64)");

            // Title
            builder.Property(e => e.Title)
                   .HasMaxLength(50)
                   .HasDefaultValue("");

            // Bio
            builder.Property(e => e.Bio)
                   .HasMaxLength(500)
                   .HasDefaultValue("Biyografi bilgisi girilmedi.");

            // WorkingAt
            builder.Property(e => e.WorkingAt)
                   .HasMaxLength(50)
                   .HasDefaultValue("Çalışılan yer bilgisi girilmedi.");

            // Country
            builder.Property(e => e.Country)
                   .IsRequired()
                   .HasMaxLength(25);

            // Birthdate
            builder.Property(e => e.BirthDate)
                   .IsRequired()
                   .HasColumnType("datetime2");

            // FollowersCount
            builder.Property(e => e.FollowersCount)
                   .HasDefaultValue(0);

            // FollowingCount
            builder.Property(e => e.FollowingCount)
                   .HasDefaultValue(0);

            // NotificationCount
            builder.Property(e => e.NotificationCount)
                .HasDefaultValue(0);

            // ProfilePicture
            // CoverImagePicture

            base.Configure(builder);
        }
    }
}
