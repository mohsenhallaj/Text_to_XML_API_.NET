using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace TextToXmlApiNet.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Allow access to Swagger and Hangfire dashboard without API key
            if (path.StartsWith("/swagger") || path.StartsWith("/docs") || path.StartsWith("/jobs"))
            {
                await _next(context);
                return;
            }

            // Check for API key
            if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key is missing.");
                return;
            }

            var configuredApiKey = configuration["ApiKey"];

            if (configuredApiKey != extractedApiKey)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }

            await _next(context);
        }
    }
}
