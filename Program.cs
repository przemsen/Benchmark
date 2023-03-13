using BenchmarkDotNet.Attributes;
using BenchmarkDotNet;
using BenchmarkDotNet.Running;
using System;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

BenchmarkRunner.Run<Benchmark>(new FastConfig());

[MemoryDiagnoser]
public class Benchmark
{
    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark(Baseline = true)]
    public void Classes()
    {
        var a = new Item { Str = "aaa", Int = 21 };
        var b = new Item { Str = "bbb", Int = 31 };

        var result = a == b;
    }

    [Benchmark]
    public void StructRecords()
    {
        var a = new SItem { Str = "aaa", Int = 21 };
        var b = new SItem { Str = "bbb", Int = 31 };

        var r = a.Equals(b);
    }


    class Item { public string Str { get; set; } public int Int { get; set; } }

    struct SItem { public string Str { get; set; } public int Int { get; set; } }
}

public class FastConfig : ManualConfig
{
    public FastConfig()
    {
        Add(DefaultConfig.Instance);
        AddJob(Job.Default
            .WithLaunchCount(1)
            .WithIterationTime(new(100, Perfolizer.Horology.TimeUnit.Millisecond))
            .WithWarmupCount(3)
            .WithIterationCount(3)
            .WithPowerPlan(BenchmarkDotNet.Environments.PowerPlan.UserPowerPlan)
        );
    }
}
