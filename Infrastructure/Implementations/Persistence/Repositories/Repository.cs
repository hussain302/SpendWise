using Application.Services;
using Domain.Contracts;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Implementations.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Persistence.Repositories;
public class Repository<TEntity, TId>(
    DBContext dbContext,
    ICurrentUser currentUser,
    IDateTimeService dateTimeService)
    : ReadOnlyRepository<TEntity, TId>(dbContext), IRepository<TEntity, TId> where TEntity
    : Entity<TId> where TId : IEquatable<TId>
{
    public async Task AddAsync(TEntity entity,
        CancellationToken cancellationToken = default)
    {
        if (entity is AuditableEntity<TId> trackable)
        {
            trackable.Created(currentUser.UserName,
                createdAtUtc: dateTimeService.DateTimeUtc);
        }

        await Set.AddAsync(entity,
            cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity,
        CancellationToken cancellationToken = default)
    {
        if (entity is AuditableEntity<TId> trackable)
        {
            trackable.Updated(currentUser.UserName,
                updatedAtUtc: dateTimeService.DateTimeUtc);
        }

        await Task.Run(() =>
        {
            Set.Update(entity);
        }, cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity,
        CancellationToken cancellationToken = default)
    {
        if (entity is AuditableEntity<TId> trackable)
        {
            //For soft delete use set.Update() instead of remove
            await Task.Run(() =>
            {
                Set.Remove(entity);
            }, cancellationToken);
        }
        else
        {
            await Task.Run(() =>
            {
                Set.Remove(entity);
            }, cancellationToken);
        }
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            if (entity is AuditableEntity<TId> trackable)
            {
                trackable.Created(currentUser.UserName,
                    createdAtUtc: dateTimeService.DateTimeUtc);
            }

            await Set.AddAsync(entity,
            cancellationToken);
        }
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var idsToDelete = entities.Select(e => e.Id).ToList();

        if (idsToDelete.Count == 0) return;

        //foreach (var entity in entities)
        //{
        //if (entity is AuditableEntity<TId> trackable)
        //{
        //    trackable.Deleted(currentUser.UserName,
        //        dateTimeService.DateTimeUtc);
        //}
        //}

        await Set.Where(entity => idsToDelete.Contains(entity.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }
}