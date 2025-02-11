using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace BlogProject.src.Infra.Interceptors
{
    public class AuditLogInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var entries = eventData.Context.ChangeTracker.Entries().ToList();

            var auditLogs = entries
                            .Where(p => p.Entity is not AuditLogEntity)
                            .Where(p => p.State == Microsoft.EntityFrameworkCore.EntityState.Added
                                     || p.State == Microsoft.EntityFrameworkCore.EntityState.Modified
                                     || p.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var auditLogEntities = new List<AuditLogEntity>();
            foreach (var entry in auditLogs)
            {
                var log = new AuditLogEntity()
                {
                    TableName = entry.Metadata.GetTableName(),
                    Operation = entry.State.ToString(),
                    CreatedTime = DateTime.UtcNow
                };

                auditLogEntities.Add(log);

                if (entry.State == EntityState.Modified)
                {

                    var oldValue = entry.OriginalValues.Properties.ToDictionary(p => p.Name, 
                                                                 p => entry.OriginalValues[p]);
                    log.OldValue = JsonSerializer.Serialize(oldValue);


                    var newValue = entry.CurrentValues.Properties.ToDictionary(p => p.Name,
                                                                               p => entry.CurrentValues[p]);
                    log.NewValue = JsonSerializer.Serialize(newValue);

                }
                else if (entry.State == EntityState.Added)
                {

                    var newValue = entry.CurrentValues.Properties.ToDictionary(p => p.Name,
                                                                               p => entry.CurrentValues[p]);
                    log.NewValue = JsonSerializer.Serialize(newValue);

                }
                else
                {

                    var oldValue = entry.OriginalValues.Properties.ToDictionary(p => p.Name,
                                                                                     p => entry.OriginalValues[p]);
                    log.OldValue = JsonSerializer.Serialize(oldValue);

                }
            }

            eventData.Context.Set<AuditLogEntity>().AddRange(auditLogEntities);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
