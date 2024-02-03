using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

if (Debugger.IsAttached)
{
    BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args, new DebugInProcessConfig());
    return;
}

Job job = Job.Default
    .WithRuntime(CoreRuntime.Latest);

IConfig config = DefaultConfig.Instance
    .HideColumns(Column.EnvironmentVariables, Column.RatioSD, Column.Error,
        Column.Categories, Column.IterationCount, Column.WarmupCount,
        Column.Toolchain, Column.CompletedWorkItems, Column.LockContentions)
    .AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByCategory, BenchmarkLogicalGroupRule.ByParams)
    .AddDiagnoser(new MemoryDiagnoser(new(displayGenColumns: false)))
    .AddJob(job);

BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args, config);
