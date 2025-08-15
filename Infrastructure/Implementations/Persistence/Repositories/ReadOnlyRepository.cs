using Domain.Contracts;
using Domain.Interfaces.Repositories;
using Infrastructure.Implementations.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Implementations.Persistence.Repositories;
public class ReadOnlyRepository<TEntity, TId>(
    DBContext dbContext)
    : RepositoryProperties<TEntity, TId>(dbContext), IReadOnlyRepository<TEntity, TId> where TEntity
    : Entity<TId> where TId : IEquatable<TId>
{
    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await SetAsNoTracking.SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await Set.AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        return await Set.CountAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<TEntity>> GetPagedAsync(int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        return await SetAsNoTracking
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)                   
                    .ToListAsync(cancellationToken);  
    }

    public async Task<IReadOnlyList<TResult>> GetAllAsync<TResult>(
    Expression<Func<TEntity, bool>>? predicate,
    Func<IQueryable<TEntity>, IQueryable<TResult>>? selector,
    CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = SetAsNoTracking.AsQueryable();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (selector is not null)
        {
            return await selector(query).ToListAsync(cancellationToken);
        }

        return await query.Cast<TResult>().ToListAsync(cancellationToken);
    }


    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await SetAsNoTracking.ToListAsync(cancellationToken);
    }
}
