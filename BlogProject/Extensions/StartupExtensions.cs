using BlogProject.CustomValidators;
using BlogProject.Localizations;
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

                options.Lockout.DefaultLockoutTimeSpan.Add(TimeSpan.FromMinutes(3));
                options.Lockout.MaxFailedAccessAttempts = 5;

            }).AddPasswordValidator<PasswordValidator>()
              .AddUserValidator<UserValidator>()
              .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
              .AddEntityFrameworkStores<BlogDbContext>();
        }
    }
}
