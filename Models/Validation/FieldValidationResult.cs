namespace TextToXmlApiNet.Models.Validation
{
    public class FieldValidationResult
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public bool IsValid { get; set; }
        public List<string> MatchedPatterns { get; set; }
    }
}
