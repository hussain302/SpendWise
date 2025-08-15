#region Using Directives
using Application.Services;
using Domain.Entities.Expenditure;
using Domain.Interfaces;
using Domain.Interfaces.Repositories.Expenditure;
using Infrastructure.Implementations.Persistence.Contexts;
#endregion

namespace Infrastructure.Implementations.Persistence.Repositories.Expenditure;

public sealed class BillDetailsRepository(AppDBContext dbContext,
    ICurrentUser currentUser,
    IDateTimeService dateTimeService)
    : Repository<BillDetail, Guid>(dbContext, currentUser, dateTimeService),
    IBillsDetailsRepository
{
}
