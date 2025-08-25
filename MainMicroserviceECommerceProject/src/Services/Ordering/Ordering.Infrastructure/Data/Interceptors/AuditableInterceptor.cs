namespace Ordering.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor: SaveChangesInterceptor
{

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }


    private static void UpdateEntities(DbContext? context)
    {
        if (context == null) return;
        var entries = context.ChangeTracker.Entries<IEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = "system";
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.LastModifiedBy = "system";
                entry.Entity.LastModified = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified || entry.State == EntityState.Added || entry.HasChangedOwnEntities())
            {
                entry.Entity.LastModifiedBy = "system";
                entry.Entity.LastModified = DateTime.UtcNow;
            }
        }
    }    
}

public static class Extensions
{
    public static bool HasChangedOwnEntities(this EntityEntry entity)
    {
        return entity.References.Any(e => e.TargetEntry != null
        && e.TargetEntry.Metadata.IsOwned()
        && e.TargetEntry.State is EntityState.Added or EntityState.Modified);

    }
}