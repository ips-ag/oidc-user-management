using System.ComponentModel.DataAnnotations;
using IPS.UserManagement.Application.Features.Roles.Models;
using IPS.UserManagement.Application.Features.Users.Commands;
using IPS.UserManagement.Application.Features.Users.Models;

namespace IPS.UserManagement.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();

    [HttpPost("{id}/roles")]
    [Authorize(Policy = "role-assignments:full")]
    [Produces(typeof(RoleQueryModel))]
    public async Task<RoleQueryModel> CreateRoleAssignment(
        [FromRoute(Name = "id")] string id,
        [FromBody][Required] CreateRoleAssignmentCommandModel commandModel,
        CancellationToken cancel)
    {
        CreateRoleAssignmentCommand command = new(id, commandModel);
        var model = await Mediator.Send(command, cancel);
        return model;
    }

    [HttpGet("{id}/roles")]
    [Authorize(Policy = "role-assignments:read")]
    [Produces(typeof(List<RoleQueryModel>))]
    public async Task<List<RoleQueryModel>> GetRoleAssignments(
        [FromRoute(Name = "id")] string id,
        CancellationToken cancel)
    {
        GetRoleAssignmentsCommand command = new(id);
        var models = await Mediator.Send(command, cancel);
        return models;
    }
}
