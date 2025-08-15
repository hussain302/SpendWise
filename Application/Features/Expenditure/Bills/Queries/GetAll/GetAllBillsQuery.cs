#region Usings Directives
using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Domain.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Domain.Dtos;
using System.ComponentModel.DataAnnotations;
#endregion

namespace Application.Features.Expenditure.Bills.Queries.GetAll;

public partial class GetAllBillsQuery
    : IRequest<Result<IReadOnlyList<BillDto>>>
{
    public Guid OrganizationId { get; set; }
}

internal sealed class GetAllBillsQueryHandler(IAppUnitOfWork unitOfWork)
    : IRequestHandler<GetAllBillsQuery, Result<IReadOnlyList<BillDto>>>
{
    public async Task<Result<IReadOnlyList<BillDto>>> Handle(GetAllBillsQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var billsResult = await unitOfWork.BillsRepository.GetAllBillsByOrganizationId(query.OrganizationId,
                cancellationToken);

            return billsResult;
        }
        catch (Exception ex)
        {
            throw new SqlDomainException("An error occurred while fetching the Bills.", new Dictionary<string, string[]>
            {
                { "InnerException", new[] { ex.InnerException?.ToString() ?? ex.Message } }
            });
        }
    }
}

