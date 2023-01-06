using FluentAssertions;
using IntegrationTests.Infrastructure;
using Xunit;

namespace IntegrationTests;

public class IntegrationTests : IClassFixture<TestServer>
{
    private readonly TestServer testServer;

    public IntegrationTests(TestServer testServer)
    {
        this.testServer = testServer;
    }

    [Fact]
    public async Task Get_Endpoint()
    {
        // Arrange
        var httpClient = testServer.CreateClient();

        // Act
        var response = await httpClient.GetAsync("/");
        
        // Assert
        response.EnsureSuccessStatusCode();
        (await response.Content.ReadAsStringAsync()).Should().Be("Hello World!");
    }
}