using IPS.UserManagement.Tests.Fixtures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IPS.UserManagement.Tests;

public class HostTests : IClassFixture<HostFixture>
{
    private readonly HostFixture _fixture;

    public HostTests(HostFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _fixture.TestOutputHelper = output;
    }

    [Fact]
    public void ShouldStartHost()
    {
        // _fixture.Services.GetRequiredService<IConfiguration>();
        var client = _fixture.Client;
    }
    
}
