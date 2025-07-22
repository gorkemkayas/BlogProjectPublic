using BlogProject.Domain.Entities;
using BlogProject.Infrastructure.Interceptors;
using BlogProject.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BlogProject.Infrastructure.Persistence
{
    public class BlogDbContext : IdentityDbContext<AppUser,AppRole,Guid>
    {
        public DbSet<FeedbackEntity> Feedbacks { get; set; }
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
        public DbSet<SavedPostEntity> SavedPosts { get; set; }
        public DbSet<SettingsEntity> Settings { get; set; }
        public DbSet<ShareEntity> Shares { get; set; }
        public DbSet<SubscriptionEntity> Subscriptions { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }

        public DbSet<NotificationSubscribeEntity> NotificationSubscribes { get; set; }

        // Identity kendi içinde 'Users' tablosu içeriyor.
        //public DbSet<AppUser> Users { get; set; }
        public DbSet<UserMemberShipEntity> UserMemberShips { get; set; }
        public DbSet<WalletEntity> Wallets { get; set; }

        public DbSet<AuditLogEntity> AuditLogs { get; set; }

        public BlogDbContext(DbContextOptions options) : base(options)
        {

        }
        public BlogDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ef");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly, predicate => predicate.Namespace == "BlogProject.Infrastructure.Persistence.Configurations");


            // Daha sonra global query filter in tüm entityler için olan hali eklenecek.
            modelBuilder.Entity<PostEntity>().HasQueryFilter(p => !p.IsDeleted);


            base.OnModelCreating(modelBuilder);
        }

        //private readonly StreamWriter _logStream = new StreamWriter("BlogDbLogs.txt", append: true);
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseAsyncSeeding(async (BlogDbContext, _, ct) =>
            //{
            //    await DataGenerators.DataGenerators.SeedDatabaseAsync(BlogDbContext, _, ct);
            //});

            optionsBuilder.LogTo(m => Debug.WriteLine(m),LogLevel.Warning);
            optionsBuilder.AddInterceptors(new AuditLogInterceptor());
            
        }
    }
}

//dotnet ef migrations add YeniMigration --project BlogProject.Infrastructure --startup-project BlogProject.Web


public class DbContextFactory : IDesignTimeDbContextFactory<BlogDbContext>
{
    public BlogDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BlogProject")).AddJsonFile("appsettings.json").Build();
        var connString = configuration.GetConnectionString("SqlServer");

        var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
        optionsBuilder.UseSqlServer(connString, options =>
        {
            options.CommandTimeout(5_000);
            options.EnableRetryOnFailure(5);
        });

        return new BlogDbContext(optionsBuilder.Options);
    }
}
