using BlogProject.Services.Abstract;
using BlogProject.Services.Concrete;
using BlogProject.Services.CustomMethods.Abstract;
using BlogProject.Services.CustomMethods.Concrete;
using BlogProject.Services.DTOs.MappingProfile;
using BlogProject.src.Infra.Context;
using BlogProject.src.Infra.Entitites;
using BlogProject.src.Infra.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")), ServiceLifetime.Scoped);

builder.Services.AddIdentity<AppUser,AppRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    options.Password.RequireNonAlphanumeric = false;
    
})
                .AddEntityFrameworkStores<BlogDbContext>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUsernameGenerator, UsernameGenerator>();



var app = builder.Build();



//bekleyen migrationları otomatik veritabanına göndermek için.
using (var scope = app.Services.CreateScope())
{

    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<BlogDbContext>(); 


        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations updated successfully!");
        }
    }
    catch (Exception)
    {

        Console.WriteLine("There are errors while updating migrations to database.");
        throw;
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
