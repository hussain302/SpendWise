//using MediatR;
//using Shared.Wrapper;
//using Shared.Exceptions;
//using Domain.Interfaces.UnitOfWorks;

//namespace Application.Features.Security.Roles.Queries.GetById;

//public partial class GetAllPaginatedRolesQuery : IRequest<Result<PagedList<RoleDto>>>
//{
//    public int PageSize { get; set; }
//    public int PageNumber { get; set; }
//}

//internal class GetAllPaginatedRolesQueryHandler(IAppUnitOfWork unitOfWork)
//    : IRequestHandler<GetAllPaginatedRolesQuery, Result<PagedList<RoleDto>>>
//{
//    public async Task<Result<PagedList<RoleDto>>> Handle(GetAllPaginatedRolesQuery command,
//        CancellationToken cancellationToken)
//    {
//        var result = new Result<PagedList<RoleDto>>();

//        try
//        {
//            var totalCount = await unitOfWork.RolesRepository.GetCountAsync(cancellationToken);

//            if (totalCount < 1)
//            {
//                result.NotFound($"Roles not found.");
//                return result;
//            }

//            var roles = await unitOfWork.RolesRepository.GetPagedAsync(
//                command.PageNumber,
//                command.PageSize,
//                cancellationToken);

//            var roleDtos = roles.Select(x => x.ToDto()).ToList();

//            var pagedRoles = PagedList<RoleDto>.Create(
//                pageSize: command.PageSize,
//                pageNumber: command.PageNumber,
//                totalCount: totalCount,
//                data: roleDtos);

//            result.AddValue(pagedRoles);
//            result.AddSuccessMessage($"Roles have been successfully retrieved.");
//            result.OK();
//        }
//        catch (Exception ex)
//        {
//            throw new SqlDomainException("An error occurred while fetching the roles.", new Dictionary<string, string[]>
//            {
//                { "InnerException", new[] { ex.InnerException?.ToString() ?? ex.Message } }
//            });
//        }

//        return result;
//    }   
//}
