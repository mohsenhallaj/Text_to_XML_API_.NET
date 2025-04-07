using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System.Xml.Schema;
using TextToXmlApiNet.Models.Validation;
using TextToXmlApiNet.Data;
using TextToXmlApiNet.Models.XmlStorage;
using TextToXmlApiNet.Services;

namespace TextToXmlApiNet.Controllers
{
    [ApiController]
    [Route("api/convert")]
    public class ConvertController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ConvertController(ApplicationDbContext db, IBackgroundJobClient backgroundJobClient)
        {
            _db = db;
            _backgroundJobClient = backgroundJobClient;
        }

        /// <summary>
        /// Enqueues a background job to convert JSON to XML and validate.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(202)]
        public IActionResult ConvertAsync([FromBody] ConvertRequest request)
        {
            var fields = request.Fields ?? new Dictionary<string, string>();

            //  Enqueue job for background processing
            _backgroundJobClient.Enqueue<XmlBackgroundService>(svc =>
                svc.ProcessAndStoreXml(fields)
            );

            return Accepted(new
            {
                message = " Your XML conversion job has been queued for background processing.",
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Converts JSON to validated XML and returns as raw XML if valid.
        /// </summary>
        [HttpPost("xml")]
        [Produces("application/xml")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult ConvertToXml([FromBody] ConvertRequest request)
        {
            var fields = request.Fields ?? new Dictionary<string, string>();
            var errors = new List<string>();

            try
            {
                var doc = new XDocument(
                    new XElement("People", fields.Select(f => new XElement(f.Key, f.Value)))
                );

                string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "schema.xsd");
                if (!System.IO.File.Exists(xsdPath))
                {
                    return BadRequest($" schema.xsd not found at path: {xsdPath}");
                }

                var schemas = new XmlSchemaSet();
                schemas.Add("", xsdPath);
                doc.Validate(schemas, (o, e) => errors.Add($"❌ Schema Error: {e.Message}"));

                if (errors.Any())
                {
                    return BadRequest(new { isValid = false, errors });
                }

                _db.StoredXmls.Add(new StoredXml
                {
                    Content = doc.ToString(),
                    CreatedAt = DateTime.UtcNow
                });
                _db.SaveChanges();

                return Content(doc.ToString(), "application/xml");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Get the latest XML stored in the database.
        /// </summary>
        [HttpGet("stored")]
        public IActionResult GetLatestXml()
        {
            var latest = _db.StoredXmls
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    x.Id,
                    x.Content,
                    x.CreatedAt
                })
                .FirstOrDefault();

            return Ok(latest);
        }
    }
}
