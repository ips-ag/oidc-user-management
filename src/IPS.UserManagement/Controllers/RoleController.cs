using IPS.UserManagement.Application.Features.Roles.Commands;
using IPS.UserManagement.Application.Features.Roles.Models;

namespace IPS.UserManagement.Controllers;

[Route("[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();

    [HttpPost]
    [Authorize(Policy = "roles:full")]
    public async Task<IActionResult> CreateRole(
        [FromBody] CreateRoleCommandModel commandModel,
        CancellationToken cancel)
    {
        CreateRoleCommand command = new(commandModel);
        var model = await Mediator.Send(command, cancel);
        return Ok(model);
    }

    [HttpGet]
    [Authorize(Policy = "roles:read")]
    public async Task<IActionResult> GetRoles(CancellationToken cancel)
    {
        GetRolesCommand command = new();
        var models = await Mediator.Send(command, cancel);
        return Ok(models);
    }
}
