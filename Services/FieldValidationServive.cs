using System.Text.Json;
using System.Text.RegularExpressions;
using TextToXmlApiNet.Models.Validation;

namespace TextToXmlApiNet.Services
{
    public class FieldValidationService
    {
        private readonly List<FieldDefinition> _fields;
        private readonly string _rootElement;

        public FieldValidationService()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "FieldDefinition.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine(" FieldDefinition.json not found at path: " + filePath);
                _fields = new List<FieldDefinition>();
                _rootElement = "Root";
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                var config = JsonSerializer.Deserialize<FieldDefinitionConfig>(json);
                _fields = config?.Structure ?? new List<FieldDefinition>();
                _rootElement = config?.Root ?? "Root";

                Console.WriteLine($" Loaded {_fields.Count} field definitions from JSON.");
                Console.WriteLine($" Root element from config: {_rootElement}");

                foreach (var field in _fields)
                {
                    Console.WriteLine($"• Field: {field.Name}, Required: {field.Required}, MinLength: {field.MinLength}, MaxLength: {field.MaxLength}, Regex: {field.ValidationRegex}");
                }
            }
            catch (Exception ex)
            {
                _fields = new List<FieldDefinition>();
                Console.WriteLine($" Error loading JSON: {ex.Message}");
                _rootElement = "Root";
            }
        }

        public FieldValidationResult ValidateField(string fieldName, string value)
        {
            return ValidateFieldDetailed(fieldName, value);
        }

        public FieldValidationResult ValidateFieldDetailed(string fieldName, string value)
        {
            Console.WriteLine($" Validating field: '{fieldName}' with value: '{value}'");

            var result = new FieldValidationResult
            {
                Field = fieldName,
                Value = value,
                IsValid = true,
                MatchedPatterns = new List<string>()
            };

            var field = _fields.FirstOrDefault(f => f.Name == fieldName);
            if (field == null)
            {
                Console.WriteLine($" No config found for field: {fieldName}");
                result.IsValid = false;
                return result;
            }

            var trimmedValue = value?.Trim() ?? "";

            if (field.Required && string.IsNullOrWhiteSpace(trimmedValue))
            {
                Console.WriteLine(" Value is required but is empty.");
                result.IsValid = false;
                return result;
            }

            if (trimmedValue.Length < field.MinLength)
            {
                Console.WriteLine($" Too short. Min length: {field.MinLength}");
                result.IsValid = false;
                return result;
            }

            if (field.MaxLength.HasValue && trimmedValue.Length > field.MaxLength.Value)
            {
                Console.WriteLine($" Too long. Max length: {field.MaxLength.Value}");
                result.IsValid = false;
                return result;
            }

            if (!string.IsNullOrWhiteSpace(field.ValidationRegex))
            {
                Console.WriteLine($" Validating with regex: {field.ValidationRegex}");
                bool match = Regex.IsMatch(trimmedValue, field.ValidationRegex);
                if (match)
                {
                    result.MatchedPatterns.Add(field.ValidationRegex);
                }
                result.IsValid = match;
                return result;
            }

            foreach (var pattern in field.Patterns ?? new List<string>())
            {
                if (Regex.IsMatch(trimmedValue, pattern))
                {
                    result.MatchedPatterns.Add(pattern);
                }
            }

            result.IsValid = result.MatchedPatterns.Any();
            return result;
        }

        public List<string> MatchDefinedPatterns(string fieldName, string value)
        {
            var matchedPatterns = new List<string>();
            var field = _fields.FirstOrDefault(f => f.Name == fieldName);
            var trimmedValue = value?.Trim() ?? "";

            if (field?.Patterns == null)
            {
                Console.WriteLine($" No patterns for field: {fieldName}");
                return matchedPatterns;
            }

            foreach (var pattern in field.Patterns)
            {
                if (Regex.IsMatch(trimmedValue, pattern))
                {
                    matchedPatterns.Add(pattern);
                    Console.WriteLine($" Pattern matched: {pattern}");
                }
            }

            return matchedPatterns;
        }
    }
}
