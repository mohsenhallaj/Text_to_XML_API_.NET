namespace TextToXmlApiNet.Models.Validation;

public class FieldDefinitionConfig
{
    public string Root { get; set; } = string.Empty;

    public List<FieldDefinition> Structure { get; set; } = new();
}
