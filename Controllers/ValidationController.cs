using Microsoft.AspNetCore.Mvc;
using TextToXmlApiNet.Models.Validation;
using TextToXmlApiNet.Services;

namespace TextToXmlApiNet.Controllers
{
    /// <summary>
    /// Handles validation of input fields using custom rules defined in JSON.
    /// </summary>
    [ApiController]
    [Route("api/validate")]
    public class ValidationController : ControllerBase
    {
        private readonly FieldValidationService _fieldValidationService;

        public ValidationController(FieldValidationService fieldValidationService)
        {
            _fieldValidationService = fieldValidationService;
        }

        /// <summary>
        /// Validates a single field or multiple fields using predefined rules.
        /// </summary>
        /// <param name="request">UnifiedValidationRequest object with either FieldName + FieldValue or Fields dictionary.</param>
        /// <returns>Validation result(s)</returns>
        /// <response code="200">Returns validation result(s)</response>
        /// <response code="400">Bad input request</response>
        [HttpPost]
        [ProducesResponseType(typeof(FieldValidationResult), 200)]
        [ProducesResponseType(typeof(BulkValidationResult), 200)]
        [ProducesResponseType(400)]
        public IActionResult Validate([FromBody] UnifiedValidationRequest request)
        {
            if (request.Fields != null && request.Fields.Any())
            {
                //  Bulk field validation
                var bulkResult = new BulkValidationResult
                {
                    Results = new List<FieldValidationResult>()
                };

                foreach (var field in request.Fields)
                {
                    var result = _fieldValidationService.ValidateFieldDetailed(field.Key, field.Value);
                    bulkResult.Results.Add(result);
                }

                return Ok(bulkResult);
            }

            if (!string.IsNullOrEmpty(request.FieldName))
            {
                //  Single field validation
                var result = _fieldValidationService.ValidateFieldDetailed(request.FieldName, request.FieldValue ?? "");
                return Ok(result);
            }

            return BadRequest("Please provide either a field name and value or a dictionary of fields.");
        }
    }
}
