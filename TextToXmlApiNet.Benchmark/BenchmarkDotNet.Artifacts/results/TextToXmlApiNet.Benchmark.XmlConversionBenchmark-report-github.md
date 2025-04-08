```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3624)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 9.0.202
  [Host]     : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2


```
| Method                 | Mean     | Error    | StdDev   | Gen0    | Gen1   | Allocated |
|----------------------- |---------:|---------:|---------:|--------:|-------:|----------:|
| &#39;XDocument + LINQ&#39;     | 94.83 μs | 1.092 μs | 0.968 μs | 22.0947 | 6.5918 | 203.55 KB |
| XmlWriter              | 47.08 μs | 0.430 μs | 0.381 μs | 15.2588 | 3.7842 | 140.78 KB |
| &#39;StringBuilder Manual&#39; | 12.83 μs | 0.223 μs | 0.208 μs | 12.9852 | 2.5940 | 119.84 KB |
