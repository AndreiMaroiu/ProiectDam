
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using PixelizerUI.Models;

BenchmarkRunner.Run<PixelizerBenchmark>();

[MemoryDiagnoser]
public unsafe class PixelizerBenchmark
{

}
