#region Using Directives
using Application.Features.Expenditure.Bills.Commands.AddEdit;
using Application.Features.Expenditure.Bills.Commands.Delete;
using Application.Features.Expenditure.Bills.Queries.GetAll;
#endregion

namespace EndPoints.Controllers.Expenditure.Bills.v1;

[ApiVersion("1")]
public sealed partial class BillsController(IMediator mediator) 
    : BaseController
{
    [HttpPost(nameof(Add))]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Add(AddEditBillCommand request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));
    
    [HttpPut(nameof(Edit))]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Edit(AddEditBillCommand request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));
    
    [HttpDelete(nameof(Delete))]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Delete(DeleteBillCommand request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));
    
    [HttpGet(nameof(GetAll))]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllBillsQuery request,
        CancellationToken cancellationToken)
        => ApiResult(await mediator.Send(request, cancellationToken));
}