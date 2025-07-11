using BlogProject.Extensions;
using BlogProject.Models;
using BlogProject.Services.Abstract;
using BlogProject.Services.Concrete;
using BlogProject.Services.CustomMethods.Abstract;
using BlogProject.Services.CustomMethods.Concrete;
using BlogProject.Services.DTOs.MappingProfile;
using BlogProject.src.Infra.Context;
using BlogProject.src.Infra.Entitites;
using BlogProject.src.Infra.Interceptors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(opt =>
{
    opt.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); // CSRF saldırılarına karşı koruma için ekledik.
});

// Service sınıflarından IUrlHelper ve IActionContextAccessor'ı kullanabilmek için ekledik.
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")), ServiceLifetime.Scoped);


// app.settings.dev deki email uygulama tarafında temsil edebilmek için bir sınıf ile eşleştiriyorum.
// Ne zaman EmailSettings sınıfına ihtiyaç duyarsam bana appsetting.json dosyasındaki EmailSettings bölümünü verecek.
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddIdentityWithExtension();
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "BlogCookie";

    opt.ExpireTimeSpan = TimeSpan.FromDays(30);
    opt.SlidingExpiration = true;

    opt.LoginPath = new PathString("/User/SignIn");
    opt.LogoutPath = new PathString("/User/Logout");
    opt.AccessDeniedPath = new PathString("/User/AccessDenied");
});

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddServicesWithLifeTimes();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 50 MB
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 MB
});


var app = builder.Build();

app.ConfigureExceptionHandler();

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

app.UseHttpsRedirection(); // eğer http:local... li istek gelirse https:local... e yönlendirmesini söylüyoruz.
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
