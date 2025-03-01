using BlogProject.src.Infra.Context;
using BlogProject.src.Infra.Entitites;

namespace BlogProject.Extensions
{
    public static class StartupExtensions
    {
        public static void AddIdentityWithExtension(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                options.Password.RequireNonAlphanumeric = false;

            }).AddEntityFrameworkStores<BlogDbContext>();
        }
    }
}
