#region Using Directives
using Application.Services;
using Domain.Entities.Ownership;
using Domain.Interfaces;
using Domain.Interfaces.Repositories.Ownership;
using Infrastructure.Implementations.Persistence.Contexts;
#endregion

namespace Infrastructure.Implementations.Persistence.Repositories.Ownership;

public sealed class OrganizationRepository(AppDBContext dbContext,
    ICurrentUser currentUser,
    IDateTimeService dateTimeService)
    : Repository<Organization, Guid>(dbContext, currentUser, dateTimeService),
    IOrganizationRepository
{
}
