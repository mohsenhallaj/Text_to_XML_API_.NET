using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Xml;

namespace TextToXmlApiNet.Benchmark;

public class XmlBenchmark
{
    private const string SampleText = "<root><child>content</child></root>";

    [Benchmark]
    public void ParseXml()
    {
        var doc = new XmlDocument();
        doc.LoadXml(SampleText);
    }

    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<XmlBenchmark>();
    }
}
