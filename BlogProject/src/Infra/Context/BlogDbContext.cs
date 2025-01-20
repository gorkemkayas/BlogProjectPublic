using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.src.Infra.Context
{
    public class BlogDbContext : DbContext
    {
        public DbSet<BadgeEntity> Badges { get; set; }
        public DbSet<BadgeUserEntity> BadgeUsers { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<DonationEntity> Donations { get; set; }
        public DbSet<FollowEntity> Follows { get; set; }
        public DbSet<LikeEntity> Likes { get; set; }
        public DbSet<MemberShipTypeEntity> MemberShipTypes { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<PaymentMethodEntity> PaymentMethods { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<PostTagEntity> PostTags { get; set; }
        public DbSet<ReportEntity> Reports { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<SavedPostEntity> SavedPosts { get; set; }
        public DbSet<SettingsEntity> Settings { get; set; }
        public DbSet<ShareEntity> Shares { get; set; }
        public DbSet<SubscriptionEntity> Subscriptions { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserMemberShipEntity> UserMemberShips { get; set; }
        public DbSet<WalletEntity> Wallets { get; set; }

        public BlogDbContext(DbContextOptions options) : base(options)
        {

        }
        public BlogDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ef");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly, predicate => predicate.Namespace == "BlogProject.src.Infra.EntityTypeConfigurations");

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connString = configuration.GetConnectionString("SqlServer");

            optionsBuilder.UseSqlServer(connString, options =>
            {
                options.CommandTimeout(5_000);
                options.EnableRetryOnFailure(5);
            });


        }
    }
}
