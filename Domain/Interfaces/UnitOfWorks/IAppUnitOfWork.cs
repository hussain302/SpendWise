using Domain.Interfaces.Repositories.Expenditure;
using Domain.Interfaces.Repositories.Ownership;
using Domain.Interfaces.Repositories.Security;

namespace Domain.Interfaces.UnitOfWorks;
public interface IAppUnitOfWork : IUnitOfWork
{
    public IRolesRepository RolesRepository { get; init; }
    public IOrganizationRepository OrganizationRepository { get; init; }
    public IBillsRepository BillsRepository { get; init; }
    public IBillsDetailsRepository BillDetailsRepository { get; init; }
}
