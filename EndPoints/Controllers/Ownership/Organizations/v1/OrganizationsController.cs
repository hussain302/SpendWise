#region Using Directives
using Application.Features.Ownership.Organizations.Commands.AddEdit;
using Application.Features.Ownership.Organizations.Commands.Delete;
using Application.Features.Ownership.Organizations.Queries.GetAll;
using Application.Features.Ownership.Organizations.Queries.GetById;
using EndPoints.Infrastructure.Attributes;
#endregion

namespace EndPoints.Controllers.Ownership.Organizations.v1;

[ApiVersion("1")]
public sealed partial class OrganizationsController(IMediator mediator) 
    : BaseController
{
    [HttpPost("Add")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Add(AddEditOrganizationCommand request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));

    [HttpPut("Edit")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Edit(AddEditOrganizationCommand request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));

    [HttpDelete("Delete")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Delete([FromQuery] DeleteOrganizationCommand request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));

    [HttpGet("GetById")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetById([FromQuery] GetOrganizationByIdQuery request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));

    [CustomSpendWiseAuthorize]
    [HttpGet("GetAll")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(new GetAllOrganizationsQuery(), cancellationToken));
}
