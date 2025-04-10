using Microsoft.AspNetCore.Mvc;

namespace TextToXmlApiNet.Controllers
{
    /// <summary>
    /// API health status
    /// </summary>
    [ApiController]
    [Route("[controller]")]
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
        /// Welcome endpoint
        /// </summary>
        [HttpGet("/")]
        public IActionResult Home()
        {
            return Ok("Welcome to TextToXmlApiNet API 🚀");
        }
    }
}
