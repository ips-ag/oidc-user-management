namespace IPS.UserManagement.Tests;

[Collection(CollectionNames.Default)]
public class HostTests
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
        var client = _fixture.UserManagementClient;
    }
}
