using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.src.Infra.DataGenerators
{
    public static class DataGenerators
    {

        public static async Task SeedDatabaseAsync(DbContext context, bool isFirstRun, CancellationToken ct)
        {
            if (!context.Set<UserEntity>().Any())
            {
                context.Set<UserEntity>().Add(
                    new UserEntity
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
