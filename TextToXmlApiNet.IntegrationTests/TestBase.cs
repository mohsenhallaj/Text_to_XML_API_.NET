using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.IO;
using Xunit;

namespace TextToXmlApiNet.IntegrationTests;

public class TestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient _client;

    public TestBase(WebApplicationFactory<Program> factory)
    {
        // ✅ Updated: Point to the root folder, not subfolder
        var projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

        _client = factory
            .WithWebHostBuilder(webBuilder =>
            {
                webBuilder.UseSetting(WebHostDefaults.ContentRootKey, projectDir);
                webBuilder.ConfigureAppConfiguration((context, configBuilder) =>
                {
                    context.HostingEnvironment.EnvironmentName = "Development";
                });
            })
            .CreateClient();
    }
}
