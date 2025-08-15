using Domain.Dtos;
using Domain.Entities.Expenditure;
using Shared.Wrapper;

namespace Domain.Interfaces.Repositories.Expenditure;
public interface IBillsRepository : IRepository<Bill, Guid>
{
    Task<Result<IReadOnlyList<BillDto>>> GetAllBillsByOrganizationId(Guid OrganizationId,
        CancellationToken cancellationToken);
}