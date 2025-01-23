using BlogProject.src.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")),ServiceLifetime.Scoped);




var app = builder.Build();

// bekleyen migrationları otomatik veritabanına göndermek için.
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<BlogDbContext>();

        if(dbContext.Database.GetPendingMigrations().Any())
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
