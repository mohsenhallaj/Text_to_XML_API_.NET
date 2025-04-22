using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TextToXmlApiNet.IntegrationTests;

public class SystemControllerTests : TestBase
{
    public SystemControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task GetSystemInfo_ShouldReturn200AndValidData()
    {
        _client.DefaultRequestHeaders.Add("X-API-KEY", "super-secret-123");

        var response = await _client.GetAsync("/api/system/info");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("os");
        content.Should().Contain("processor");
    }
}
