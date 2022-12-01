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

    [HttpGet]
    [Authorize(Policy = "users:read")]
    [Produces(typeof(List<UserQueryModel>))]
    public async Task<List<UserQueryModel>> GetUsers(
        [FromQuery(Name = "id")] string? id,
        [FromQuery(Name = "userName")] string? userName,
        CancellationToken cancel)
    {
        GetUsersCommand command = new(id, userName);
        var models = await Mediator.Send(command, cancel);
        return models;
    }

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
