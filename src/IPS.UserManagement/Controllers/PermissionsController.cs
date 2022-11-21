using System.ComponentModel.DataAnnotations;
using IPS.UserManagement.Application.Features.Permissions.Commands;
using IPS.UserManagement.Application.Features.Permissions.Models;

namespace IPS.UserManagement.Controllers;

[Route("[controller]")]
[ApiController]
public class PermissionsController : ControllerBase
{
    private IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();

    [HttpPost]
    [Authorize(Policy = "permissions:full")]
    public async Task<IActionResult> CreatePermission(
        [FromBody] CreatePermissionCommandModel commandModel,
        CancellationToken cancel)
    {
        CreatePermissionCommand command = new(commandModel);
        var model = await Mediator.Send(command, cancel);
        return Ok(model);
    }

    [HttpGet]
    [Authorize(Policy = "permissions:read")]
    public async Task<IActionResult> GetPermissions([FromQuery][Required] string resource, CancellationToken cancel)
    {
        GetPermissionsCommand command = new(resource);
        var models = await Mediator.Send(command, cancel);
        return Ok(models);
    }
}
