using Domain.Contracts;
using Domain.Entities.Security;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories.Security;
public interface IRolesRepository
{
    Task<ApplicationRole> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApplicationRole>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ApplicationRole role, CancellationToken cancellationToken = default);
    Task UpdateAsync(ApplicationRole role, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApplicationRole?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default);
}