#region Using Directives
using Application.Features.Security.Roles.Queries.GetAll;
using Application.Features.Security.Roles.Queries.GetById;
#endregion

namespace EndPoints.Controllers.Security.Roles.v1;

[ApiVersion("1")]
public sealed partial class RolesController(IMediator mediator) : BaseController
{
    [HttpPost("Add")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Add(AddEditRoleCommand request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));

    [HttpPut("Edit")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Edit(AddEditRoleCommand request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));

    [HttpDelete("Delete")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Delete([FromQuery] DeleteRoleCommand request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));

    [HttpGet("GetById")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetById([FromQuery] GetRoleByIdQuery request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));

    [HttpGet("GetAll")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(new GetAllRolesQuery(), cancellationToken));
}
