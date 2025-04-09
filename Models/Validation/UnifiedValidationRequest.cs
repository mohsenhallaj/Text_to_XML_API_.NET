namespace TextToXmlApiNet.Models.Validation
{
    public class UnifiedValidationRequest
    {
        public string FieldName { get; set; } = string.Empty;
        public string FieldValue { get; set; } = string.Empty;
        public Dictionary<string, string> Fields { get; set; } = new();
    }
}
