using Domain.Contracts;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories;

public interface IReadOnlyRepository<TEntity, TId> where TEntity
    : Entity<TId>
{
    Task<TEntity?> GetByIdAsync(TId id,
        CancellationToken cancellationToken = default);
    // Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, bool>>? predicate,
        Func<IQueryable<TEntity>, IQueryable<TResult>>? selector,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<TEntity>> GetPagedAsync(int pageNumber,
        int pageSize, 
        CancellationToken cancellationToken);
}

