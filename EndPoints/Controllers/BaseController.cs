namespace EndPoints.Controllers;

[ApiController]
[Route("api/[Controller]")]
public abstract class BaseController() : ControllerBase
{
    protected IActionResult ApiResult(Result result)
    {
        return Ok(result);
    }
}
