using System.Text.Json.Serialization;

namespace TextToXmlApiNet.Models.Validation
{
    public class FieldDefinition
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("patterns")]
        public List<string> Patterns { get; set; } = new();

        [JsonPropertyName("min_length")]
        public int MinLength { get; set; }

        [JsonPropertyName("max_length")]
        public int? MaxLength { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; } = true;

        [JsonPropertyName("validation_regex")]
        public string ValidationRegex { get; set; } = "";
    }
}
