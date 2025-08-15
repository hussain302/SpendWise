using Domain.Contracts;
using Infrastructure.Implementations.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Persistence.Repositories;
public class RepositoryProperties<TEntity, TId>(
    DBContext dbContext
    ) where TEntity : Entity<TId>
{
    protected readonly DBContext _dbContext = dbContext;

    protected DbSet<TEntity> Set => _dbContext.Set<TEntity>();

    protected IQueryable<TEntity> SetAsNoTracking
    {
        get
        {
            var query = Set.AsNoTracking();

            if (typeof(TEntity).IsSubclassOf(typeof(AuditableEntity<TId>)) || typeof(TEntity).IsSubclassOf(typeof(AuditableEntity<TId>)))
            {
                //is soft delete is enabled
                //query = query.Where(e => !(e as AuditableEntity<TId>)!.IsDeleted);
            }

            return query;
        }
    }
}
