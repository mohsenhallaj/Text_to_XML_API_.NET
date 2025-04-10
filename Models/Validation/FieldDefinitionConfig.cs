using System.Text.Json.Serialization;

namespace TextToXmlApiNet.Models.Validation
{
    public class FieldDefinitionConfig
    {
        [JsonPropertyName("root")]
        public string Root { get; set; }

        [JsonPropertyName("structure")]
        public List<FieldDefinition> Structure { get; set; } = new();
    }
}
