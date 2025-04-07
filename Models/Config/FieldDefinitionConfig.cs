using TextToXmlApiNet.Models.Validation;


namespace TextToXmlApiNet.Models.Config
{
    public class FieldDefinitionConfig
    {
        public string Root { get; set; } = "structure"; // optional if unused
        public List<FieldDefinition> Structure { get; set; } = new();
    }
}
