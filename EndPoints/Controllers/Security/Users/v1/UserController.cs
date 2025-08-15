#region Using Directives
using Application.Features.Security.Identity.Commands.Register;
using Application.Features.Security.Users.Commands.Delete;
using Application.Features.Security.Users.Queries.GetAll;
#endregion

namespace EndPoints.Controllers.Security.Users.v1;

[ApiVersion("1")]
public sealed partial class UserController(IMediator mediator) 
    : BaseController
{

    [HttpPost(nameof(Create))]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Create([FromBody] RegisterCommand request,
           CancellationToken cancellationToken)
           => ApiResult(await mediator.Send(request, cancellationToken));

    [HttpDelete(nameof(Delete))]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Delete([FromBody] DeleteUserCommand request,
           CancellationToken cancellationToken)
           => ApiResult(await mediator.Send(request, cancellationToken));

    [HttpGet("Users")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Get([FromQuery] GetAllUsersQuery request,
           CancellationToken cancellationToken)
           => ApiResult(await mediator.Send(request, cancellationToken));
}