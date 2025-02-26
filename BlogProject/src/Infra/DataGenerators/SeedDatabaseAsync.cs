using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.src.Infra.DataGenerators
{
    public static class DataGenerators
    {

        public static async Task SeedDatabaseAsync(DbContext context, bool isFirstRun, CancellationToken ct)
        {
            if (!context.Set<AppUser>().Any())
            {
                context.Set<AppUser>().Add(
                    new AppUser
                    {
                        Bio = string.Empty,
                        BirthDate = DateTime.UtcNow,
                        Comments = { },
                        Country = "TR"
                    });
                await context.SaveChangesAsync(ct);

            }
        }
    }
}
