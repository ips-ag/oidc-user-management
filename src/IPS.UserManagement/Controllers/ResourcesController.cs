﻿using IPS.UserManagement.Application.Features.Resources.Commands;

namespace IPS.UserManagement.Controllers;

[Route("[controller]")]
[ApiController]
public class ResourcesController : ControllerBase
{
    private IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();

    [HttpPost]
    [Authorize(Policy = "resources:full")]
    public async Task<IActionResult> CreateResource(CancellationToken cancel)
    {
        CreateResourceCommand command = new();
        var model = await Mediator.Send(command, cancel);
        return Ok(model);
    }

    [HttpGet]
    [Authorize(Policy = "resources:read")]
    public async Task<IActionResult> GetResources(CancellationToken cancel)
    {
        GetResourcesCommand command = new();
        var models = await Mediator.Send(command, cancel);
        return Ok(models);
    }
}
