using System.Net.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TextToXmlApiNet.IntegrationTests;

public class TestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;

    public TestBase(WebApplicationFactory<Program> factory)
    {
        var projectDir = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.FullName;

        Client = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.UseContentRoot(projectDir); // ✅ fix: tell the test where the app is
        }).CreateClient();
    }
}
