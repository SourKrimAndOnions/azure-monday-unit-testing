using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests.Infrastructure;

public class TestServer : WebApplicationFactory<WebApplicationExample.Program>
{
}