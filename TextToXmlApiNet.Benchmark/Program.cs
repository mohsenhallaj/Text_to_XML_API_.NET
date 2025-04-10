using BenchmarkDotNet.Running;
using TextToXmlApiNet.Benchmark;

namespace TextToXmlApiNet.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<XmlConversionBenchmark>();
        }
    }
}
