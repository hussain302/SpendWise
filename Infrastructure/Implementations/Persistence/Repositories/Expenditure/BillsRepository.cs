#region Using Directives
using Application.Services;
using Domain.Entities.Expenditure;
using Domain.Interfaces;
using Domain.Interfaces.Repositories.Expenditure;
using Infrastructure.Implementations.Persistence.Contexts;
using Shared.Wrapper;
using Microsoft.Data.SqlClient;
using Domain.Dtos;
using System.Data;
#endregion

namespace Infrastructure.Implementations.Persistence.Repositories.Expenditure;

public sealed class BillsRepository(AppDBContext dbContext,
    ICurrentUser currentUser,
    IDateTimeService dateTimeService)
    : Repository<Bill, Guid>(dbContext, currentUser, dateTimeService),
    IBillsRepository
{
    public async Task<Result<IReadOnlyList<BillDto>>> GetAllBillsByOrganizationId(Guid OrganizationId,
    CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<BillDto>>();

        try
        {
            var billsQuery = @"
                SELECT 
                    B.Id, 
                    B.Title, 
                    B.BillAmount, 
                    B.Description, 
                    B.BilledOn, 
                    B.IsEmailSent, 
                    O.Id AS OrganizationId, 
                    O.Name AS OrganizationName, 
                    PaidByUser.Id AS PaidByUserId, 
                    PaidByUser.UserName AS PaidByUser
                FROM Expenditure.Bills AS B
                INNER JOIN Ownership.Organizations AS O ON B.OrganizationId = O.Id
                INNER JOIN AspNetUsers AS PaidByUser ON B.PaidById = PaidByUser.Id
                WHERE B.OrganizationId = @OrganizationId";

            var billParameters = new SqlParameter("@OrganizationId", 
                OrganizationId);

            var billRecords = await dbContext.ExecuteRawSqlQueryListAsync<BillDto>(billsQuery,
                cancellationToken, 
                billParameters);

            if (!billRecords.Any())
            {
                result.NotFound("No bills found for the specified organization.");
                return result;
            }

            var billIds = string.Join(",", billRecords.Select(b => $"'{b.Id}'"));

            var billDetailsQuery = string.Format(@"
                SELECT 
                    BD.Id, 
                    BD.BillId, 
                    BD.ShareAmount, 
                    BD.PaidOnSpot, 
                    SharedWithUser.Id AS SharedWithUserId, 
                    SharedWithUser.UserName AS SharedWithUser
                FROM Expenditure.BillDetails AS BD
                LEFT JOIN AspNetUsers AS SharedWithUser ON BD.SharedWithId = SharedWithUser.Id
                WHERE BD.BillId IN ({0})", billIds);

            var billDetailRecords = await dbContext.ExecuteRawSqlQueryListAsync<BillDetailDto>(billDetailsQuery,
                cancellationToken, parameters: []);

            foreach (var bill in billRecords)
            {
                var associatedDetails = billDetailRecords
                    .Where(d => d.BillId == bill.Id)
                    .ToList();

                bill.SharedWithUsers = associatedDetails;  
            }

            result.IsSuccess = true;
            result.AddValue(billRecords);
            result.AddSuccessMessage("Bills and details have been successfully retrieved.");
            result.OK();
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.AddErrorMessage($"An error occurred while fetching the bills: {ex.Message}");
        }

        return result;
    }
}
