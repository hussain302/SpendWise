using Application.Services;
using Domain.Entities.Security;
using Domain.Interfaces;
using Infrastructure.Implementations.Persistence.Contexts;
using Domain.Interfaces.Repositories.Security;
using Microsoft.EntityFrameworkCore;
using Domain.Contracts;
using System.Collections.Generic;


namespace Infrastructure.Implementations.Persistence.Repositories.Security
{
    public class RolesRepository(AppDBContext dbContext//, ICurrentUser currentUser, IDateTimeService dateTimeService
        ) 
        : IRolesRepository
    {
        public async Task AddAsync(ApplicationRole role, 
            CancellationToken cancellationToken = default)
        {      
            await dbContext.Roles.AddAsync(role, cancellationToken);
        }

        public async Task DeleteAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            var role = await GetByIdAsync(id, cancellationToken);
            if (role != null)
            {
                dbContext.Roles.Remove(role);
            }
            else
            {
                throw new KeyNotFoundException($"Role with id {id} not found.");
            }
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.Roles.Select(x=> new ApplicationRole 
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToListAsync(cancellationToken);
        }

        public async Task<ApplicationRole> GetByIdAsync(Guid id, 
            CancellationToken cancellationToken = default)
        {
            var role = await dbContext.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (role == null)
            {
                throw new KeyNotFoundException($"Role with id {id} not found.");
            }

            var applicationRole = new ApplicationRole
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };

            return applicationRole;
        }

        public async Task<ApplicationRole?> GetByNameAsync(string roleName, 
            CancellationToken cancellationToken = default)
        {
            return await dbContext.Roles.Select(x => new ApplicationRole
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).AsNoTracking()
              .FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
        }

        public async Task UpdateAsync(ApplicationRole role,
            CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                dbContext.Update(role);
            }, cancellationToken);
        }
    }
}
