using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces.UnitOfWorks;
using Infrastructure.Implementations.Persistence.Contexts;

namespace Infrastructure.Implementations.Persistence.UnitOfWorks;
public class UnitOfWork(DBContext dbContext) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;
        return result;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction is in progress.");

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction is in progress.");

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public DbContext GetDbContext()
    {
        return dbContext;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        dbContext.Dispose();
    }
}
