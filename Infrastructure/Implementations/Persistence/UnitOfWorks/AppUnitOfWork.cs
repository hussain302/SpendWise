using Domain.Interfaces.Repositories.Security;
using Infrastructure.Implementations.Persistence.Contexts;
using Domain.Interfaces.UnitOfWorks;
using Domain.Interfaces.Repositories.Ownership;
using Domain.Interfaces.Repositories.Expenditure;

namespace Infrastructure.Implementations.Persistence.UnitOfWorks;
public sealed class AppUnitOfWork(DBContext dbContext,
        IRolesRepository roleModelRepository,
        IOrganizationRepository organizationRepository,
        IBillsRepository billsRepository,
        IBillsDetailsRepository billsDetailsRepository
    ) : UnitOfWork(dbContext), IAppUnitOfWork
{
    public IRolesRepository RolesRepository { get; init; } = roleModelRepository;
    public IOrganizationRepository OrganizationRepository { get; init; } = organizationRepository;
    public IBillsRepository BillsRepository { get; init; } = billsRepository;
    public IBillsDetailsRepository BillDetailsRepository { get; init; } = billsDetailsRepository;
}
