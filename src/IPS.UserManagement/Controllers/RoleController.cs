using System.ComponentModel.DataAnnotations;
using IPS.UserManagement.Application.Features.Permissions.Models;
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
    [Produces(typeof(RoleQueryModel))]
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
    [Produces(typeof(List<RoleQueryModel>))]
    public async Task<IActionResult> GetRoles(CancellationToken cancel)
    {
        GetRolesCommand command = new();
        var models = await Mediator.Send(command, cancel);
        return Ok(models);
    }

    [HttpPost("{id}/permissions")]
    [Authorize(Policy = "permission-assignments:full")]
    [Produces(typeof(PermissionQueryModel))]
    public async Task<IActionResult> CreatePermissionAssignment(
        [FromRoute(Name = "id")] string id,
        [FromBody][Required] CreatePermissionAssignmentCommandModel commandModel,
        CancellationToken cancel)
    {
        CreatePermissionAssignmentCommand command = new(id, commandModel);
        var model = await Mediator.Send(command, cancel);
        return Ok(model);
    }

    [HttpGet("{id}/permissions")]
    [Authorize(Policy = "permission-assignments:read")]
    [Produces(typeof(List<PermissionQueryModel>))]
    public async Task<IActionResult> GetPermissionAssignments(
        [FromRoute(Name = "id")] string id,
        CancellationToken cancel)
    {
        GetPermissionAssignmentsCommand command = new(id);
        var models = await Mediator.Send(command, cancel);
        return Ok(models);
    }
}
