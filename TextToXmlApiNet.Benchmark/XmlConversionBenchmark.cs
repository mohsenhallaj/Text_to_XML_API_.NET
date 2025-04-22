using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace TextToXmlApiNet.Benchmark
{
    [MemoryDiagnoser]
    public class XmlConversionBenchmark
    {
        private Dictionary<string, string> _largeInput = null!;

        [GlobalSetup]
        public void Setup()
        {
            _largeInput = new Dictionary<string, string>(1000);
            for (int i = 0; i < 1000; i++)
            {
                _largeInput[$"Field{i}"] = $"Value{i}";
            }
        }

        [Benchmark(Description = "XDocument + LINQ")]
        public string ConvertWithXDocument()
        {
            var doc = new XDocument(
                new XElement("People",
                    _largeInput.Select(pair => new XElement(pair.Key, pair.Value))
                )
            );

            return doc.ToString(SaveOptions.DisableFormatting);
        }

        [Benchmark(Description = "XmlWriter")]
        public string ConvertWithXmlWriter()
        {
            var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true });

            xmlWriter.WriteStartElement("People");
            foreach (var pair in _largeInput)
            {
                xmlWriter.WriteElementString(pair.Key, pair.Value);
            }
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            return stringWriter.ToString();
        }

        [Benchmark(Description = "StringBuilder Manual")]
        public string ConvertWithStringBuilder()
        {
            var sb = new StringBuilder();
            sb.Append("<People>");
            foreach (var pair in _largeInput)
            {
                sb.Append("<").Append(pair.Key).Append(">")
                  .Append(pair.Value)
                  .Append("</").Append(pair.Key).Append(">");
            }
            sb.Append("</People>");
            return sb.ToString();
        }
    }
}
