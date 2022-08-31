using Serilog;
using Serilog.Exceptions;

namespace IPS.UserManagement;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSerilog(ConfigureLogging)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }

    private static void ConfigureLogging(
        HostBuilderContext ctx,
        IServiceProvider services,
        LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .ReadFrom.Configuration(ctx.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
            .WriteTo.Console();
    }
}
