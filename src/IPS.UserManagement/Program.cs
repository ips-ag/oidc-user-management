using System.Net;
using IPS.UserManagement;
using IPS.UserManagement.Application.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(
        (ctx, lc) => lc
            .WriteTo.Console()
            .ReadFrom.Configuration(ctx.Configuration));
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddIdentityServer()
        .AddInMemoryClients(Config.GetClients())
        .AddInMemoryIdentityResources(Config.GetIdentityResources())
        .AddInMemoryApiScopes(Config.GetScopes())
        .AddInMemoryApiResources(Config.GetApis());
    builder.Services
        .AddAuthentication(
            auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(
            options =>
            {
                options.Authority = "http://localhost:54321";
                options.TokenValidationParameters = new TokenValidationParameters { ValidAudience = "usermanagement" };
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = c =>
                    {
                        c.HandleResponse();
                        if (!c.HttpContext.Response.HasStarted)
                        {
                            c.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(
                            $"An error occurred processing your authentication: {c.Exception.Message}");
                    }
                };
            });

    builder.Services.AddApplication();

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseRouting();
    app.UseIdentityServer();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Error running application");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program
{
}
