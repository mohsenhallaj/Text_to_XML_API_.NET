using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace TextToXmlApiNet.Controllers
{
    /// <summary>
    /// API health and system info
    /// </summary>
    [ApiController]
    [Route("api/system")]
    public class SystemController : ControllerBase
    {
        /// <summary>
        /// Health Check
        /// </summary>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
        }

        /// <summary>
        /// Returns system diagnostics for debugging and monitoring.
        /// </summary>
        /// <remarks>
        /// - OS and processor count  
        /// - Application version  
        /// - Uptime since process start  
        /// - ASP.NET Core environment name  
        /// - .NET version  
        /// - Machine name  
        /// - Current memory usage
        /// </remarks>
        /// <response code="200">Returns system info including version, uptime, and memory usage</response>
        [HttpGet("info")]
        public IActionResult GetSystemInfo()
        {
            var process = Process.GetCurrentProcess();
            var uptime = DateTime.Now - process.StartTime;

            var info = new
            {
                os = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                processor = Environment.ProcessorCount,
                appVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                uptime = uptime.ToString(@"dd\.hh\:mm\:ss"),
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                dotnetVersion = Environment.Version.ToString(),
                machineName = Environment.MachineName,
                memoryUsageMB = (process.WorkingSet64 / 1024 / 1024) + " MB"
            };

            return Ok(info);
        }

        /// <summary>
        /// Welcome endpoint
        /// </summary>
        [HttpGet("/")]
        public IActionResult Home()
        {
            return Ok("Welcome to TextToXmlApiNet API 🚀");
        }
    }
}
