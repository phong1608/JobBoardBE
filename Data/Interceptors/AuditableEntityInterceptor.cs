using JobBoard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Principal;

namespace JobBoard.Data.Interceptors
{
    public class AuditableEntityInterceptor:SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;



            foreach (var entry in context.ChangeTracker.Entries())
            {
                if(entry.State == EntityState.Added)
                {
                    if(entry.Entity is JobListing job)
                    {
                        job.PostedDate = DateTime.UtcNow;
                    }
                    if(entry.Entity is Application application)
                    {
                        application.ApplicationDate = DateTime.UtcNow;
                    }
                }
                
            }

        }
    }
}
