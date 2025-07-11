using BlogProject.Application.CustomMethods.Interfaces;
using BlogProject.Application.Interfaces;
using BlogProject.Localizations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BlogProject.Extensions
{
    public static class StartupExtensions
    {
        public static void AddIdentityWithExtension(this IServiceCollection services)
        {

            // Oluşturulan tokenlerın ömrünü belirliyoruz.
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(1);
            });
            // SecurityStamp değerinin hangi aralıklar ile kontrol edileceğini belirliyoruz.
            services.Configure<SecurityStampValidatorOptions>(opt =>
            {
                opt.ValidationInterval = TimeSpan.FromMinutes(30);
            });

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
              .AddDefaultTokenProviders()
              .AddEntityFrameworkStores<BlogDbContext>();
        }

        public static void AddServicesWithLifeTimes(this IServiceCollection services)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IUsernameGenerator, UsernameGenerator>();
            services.AddScoped<IUserTokenGenerator, UserTokenGenerator>();
            services.AddScoped<IUrlGenerator, UrlGenerator>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
        }
    }
}
