using System.Text.Json.Serialization;

namespace TextToXmlApiNet.Models.Validation
{
    public class FieldValidationResult
    {
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool IsValid { get; set; } = false;

        [JsonIgnore] // Hides this from Swagger and JSON responses
        public List<string> MatchedPatterns { get; set; } = new();
    }
}
