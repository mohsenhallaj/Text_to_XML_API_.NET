namespace TextToXmlApiNet.Models.Validation
{
    public class UnifiedValidationRequest
    {
        public string? FieldName { get; set; }
        public string? FieldValue { get; set; }

        public Dictionary<string, string>? Fields { get; set; }
    }
}
