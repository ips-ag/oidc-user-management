using IPS.UserManagement.Application.Features.Resources.Commands;

namespace IPS.UserManagement.Controllers;

[Route("[controller]")]
[ApiController]
public class ResourcesController : ControllerBase
{
    private IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateResource(CancellationToken cancel)
    {
        CreateResourceCommand command = new();
        var model = await Mediator.Send(command, cancel);
        return Ok(model);
    }

    [HttpGet]
    // [Authorize]
    public async Task<IActionResult> GetResources(CancellationToken cancel)
    {
        await ValueTask.CompletedTask;
        return BadRequest();
    }
}
