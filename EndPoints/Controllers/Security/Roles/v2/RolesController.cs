namespace EndPoints.Controllers.Security.Roles.v2;

[ApiVersion("2")]
public sealed partial class RolesController(IMediator mediator)
    : BaseController
{
    [HttpDelete("Delete")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Deletev2(DeleteRoleCommand request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));
}
