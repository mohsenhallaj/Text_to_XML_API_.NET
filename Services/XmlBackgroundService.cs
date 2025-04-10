using System.Xml.Linq;
using System.Xml.Schema;
using TextToXmlApiNet.Data;
using TextToXmlApiNet.Models.XmlStorage;

namespace TextToXmlApiNet.Services
{
    public class XmlBackgroundService
    {
        private readonly ApplicationDbContext _db;

        public XmlBackgroundService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void ProcessAndStoreXml(Dictionary<string, string> fields)
        {
            var errors = new List<string>();
            string xmlOutput = "";

            try
            {
                var doc = new XDocument(
                    new XElement("People", fields.Select(f => new XElement(f.Key, f.Value)))
                );

                xmlOutput = doc.ToString();

                string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "schema.xsd");
                if (File.Exists(xsdPath))
                {
                    var schemas = new XmlSchemaSet();
                    schemas.Add("", xsdPath);
                    doc.Validate(schemas, (o, e) => errors.Add($"Schema Error: {e.Message}"));
                }

                _db.StoredXmls.Add(new StoredXml
                {
                    Content = xmlOutput,
                    CreatedAt = DateTime.UtcNow
                });

                _db.SaveChanges();
                Console.WriteLine(" Background job saved XML to database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Background job failed: " + ex.Message);
            }
        }
    }
}
