```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.26100.3624)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 9.0.202
  [Host]     : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2


```
| Method                 | Mean     | Error    | StdDev   | Gen0    | Gen1   | Allocated |
|----------------------- |---------:|---------:|---------:|--------:|-------:|----------:|
| &#39;XDocument + LINQ&#39;     | 96.09 μs | 1.433 μs | 1.270 μs | 22.0947 | 5.8594 | 203.56 KB |
| XmlWriter              | 48.83 μs | 0.959 μs | 1.464 μs | 15.2588 | 3.7842 | 140.78 KB |
| &#39;StringBuilder Manual&#39; | 12.37 μs | 0.231 μs | 0.181 μs | 12.9852 | 2.5940 | 119.84 KB |
