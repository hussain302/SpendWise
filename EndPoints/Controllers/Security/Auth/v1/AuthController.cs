#region Using Directives
using Application.Features.Security.Identity.Commands.ActivateUser;
using Application.Features.Security.Identity.Commands.ConfirmEmail;
using Application.Features.Security.Identity.Commands.Login;
using Application.Features.Security.Identity.Commands.ManageRoles;
using Application.Features.Security.Identity.Commands.Register;
#endregion

namespace EndPoints.Controllers.Security.Auth.v1;

[ApiVersion("1")]
public sealed partial class AuthController(IMediator mediator) : BaseController
{
    [HttpPost(nameof(Login))]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request,
           CancellationToken cancellationToken)
           => ApiResult(await mediator.Send(request, cancellationToken));

    [HttpPost(nameof(Register))]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand request,
           CancellationToken cancellationToken)
           => ApiResult(await mediator.Send(request, cancellationToken));

    [HttpPatch(nameof(ConfirmEmail))]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand request,
           CancellationToken cancellationToken)
           => ApiResult(await mediator.Send(request, cancellationToken));
    
    [HttpPatch(nameof(Activate))]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Activate([FromBody] ActivateUserCommand request,
           CancellationToken cancellationToken)
           => ApiResult(await mediator.Send(request, cancellationToken));

    [HttpPut(nameof(ManageUserRoles))]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> ManageUserRoles([FromBody] ManageUserRolesCommand request,
           CancellationToken cancellationToken)
           => ApiResult(await mediator.Send(request, cancellationToken));
}