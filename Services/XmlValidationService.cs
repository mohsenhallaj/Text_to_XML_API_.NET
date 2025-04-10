using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;

namespace TextToXmlApiNet.Services
{
    public class XmlValidationService
    {
        private readonly string _xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "schema.xsd");

        public (bool IsValid, string Xml, List<string> Errors) ConvertAndValidate(Dictionary<string, string> fields)
        {
            var errors = new List<string>();
            string xml;

            try
            {
                // Build XML manually
                var doc = new XDocument(
                    new XElement("People",
                        fields.Select(kvp => new XElement(kvp.Key, kvp.Value))
                    )
                );
                xml = doc.ToString();

                // Validate
                var schemas = new XmlSchemaSet();
                schemas.Add("", _xsdPath);

                doc.Validate(schemas, (o, e) =>
                {
                    errors.Add(e.Message);
                });

                return (errors.Count == 0, xml, errors);
            }
            catch (Exception ex)
            {
                errors.Add("Conversion or validation error: " + ex.Message);
                return (false, "", errors);
            }
        }
    }
}
