using FuzzySearchNet;
using Searchify.Benchmarks.TestUtilities;
using Searchify.Metrics;

namespace Searchify.Benchmarks.Metrics;

public class LevenshteinTests
{
    [ParamsSource(nameof(TestData))]
    public (string Input, string Pattern) Parameters { get; set; } = default;

    protected string Pattern { get; set; } = string.Empty;

    protected string Input { get; set; } = string.Empty;

    protected int MaxDistance { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Input = Content.Format(Parameters.Input);
        Pattern = Content.Format(Parameters.Pattern);
        MaxDistance = (int)(Input.Length * 0.25F);
    }

    public static IEnumerable<(string Input, string Pattern)> TestData()
    {
        yield return ("Word", "World");
        yield return ("quick browny", "The quick brown fox jumps over the lazy dog");
        yield return ("laser dot", "The quick brown fox jumps over the lazy dog");
        yield return ("Parade is lost", "#plrabn12.txt");
        yield return ("Dat men may know they dwell", "#plrabn12.txt");
        yield return ("Thought Eden took their solitaire path", "#plrabn12.txt");
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Distance")]
    public int Distance_FuzzySearchNet()
    {
        int totalDistance = Pattern.Length;
        foreach (MatchResult match in FuzzySearch.Find(Input, Pattern))
            totalDistance += match.Distance + match.StartIndex - match.EndIndex;

        return totalDistance;
    }

    [Benchmark, BenchmarkCategory("Distance")]
    public int Distance_Searchify() => Levenshtein.Distance(Input, Pattern);

    [Benchmark(Baseline = true), BenchmarkCategory("Ratio")]
    public float Ratio_FuzzySearchNet()
    {
        int totalDistance = Pattern.Length;
        foreach (MatchResult match in FuzzySearch.Find(Input, Pattern))
            totalDistance += match.Distance + match.StartIndex - match.EndIndex;

        return 1f - totalDistance / Math.Max(Math.Max(Input.Length, Pattern.Length), 1f);
    }

    [Benchmark, BenchmarkCategory("Ratio")]
    public float Ratio_Searchify() => Levenshtein.Ratio(Input, Pattern);

    [Benchmark(Baseline = true), BenchmarkCategory("SubsequenceDistance")]
    public int SubsequenceDistance_FuzzySearchNet() => FuzzySearch.Find(Input, Pattern).Min(static x => x.Distance);

    [Benchmark, BenchmarkCategory("SubsequenceDistance")]
    public int SubsequenceDistance_Searchify() => Levenshtein.SubsequenceDistance(Input, Pattern);

    [Benchmark(Baseline = true), BenchmarkCategory("SubsequenceRatio")]
    public float SubsequenceRatio_FuzzySearchNet() => FuzzySearch.Find(Input, Pattern).Min(static x => x.Distance) / Math.Max(Input.Length, 1f);

    [Benchmark, BenchmarkCategory("SubsequenceRatio")]
    public float SubsequenceRatio_Searchify() => Levenshtein.Ratio(Input, Pattern);

    [Benchmark(Baseline = true), BenchmarkCategory("Count")]
    public int Count_FuzzySearchNet() => FuzzySearch.Find(Input, Pattern, MaxDistance).Count();

    [Benchmark, BenchmarkCategory("Count")]
    public int Count_Searchify() => Levenshtein.Count(Input, Pattern, maxDistance: MaxDistance);

    [Benchmark(Baseline = true), BenchmarkCategory("IsMatch")]
    public bool IsMatch_FuzzySearchNet() => FuzzySearch.Find(Input, Pattern, MaxDistance).Any();

    [Benchmark, BenchmarkCategory("IsMatch")]
    public bool IsMatch_Searchify() => Levenshtein.IsMatch(Input, Pattern, MaxDistance);

    [Benchmark(Baseline = true), BenchmarkCategory("Match")]
    public MatchResult? Match_FuzzySearchNet() => FuzzySearch.Find(Input, Pattern, MaxDistance).FirstOrDefault();

    [Benchmark, BenchmarkCategory("Match")]
    public LevenshteinMatch<int> Match_Searchify() => Levenshtein.Match(Input, Pattern, maxDistance: MaxDistance);

    [Benchmark(Baseline = true), BenchmarkCategory("Matches")]
    public MatchResult[] Matches_FuzzySearchNet() => FuzzySearch.Find(Input, Pattern, MaxDistance).ToArray();

    [Benchmark, BenchmarkCategory("Matches")]
    public LevenshteinMatchCollection<int> Matches_Searchify() => Levenshtein.Matches(Input, Pattern, maxDistance: MaxDistance);

    [Benchmark(Baseline = true), BenchmarkCategory("EnumerateMatches")]
    public int EnumerateMatches_FuzzySearchNet()
    {
        int totalDistance = 0;
        foreach (MatchResult match in FuzzySearch.Find(Input, Pattern, MaxDistance))
            totalDistance += match.Distance;

        return totalDistance;
    }

    [Benchmark, BenchmarkCategory("EnumerateMatches")]
    public int EnumerateMatches_Searchify()
    {
        int totalDistance = 0;
        foreach (LevenshteinMatch<int> match in Levenshtein.EnumerateMatches(Input, Pattern, maxDistance: MaxDistance))
            totalDistance += match.Distance;

        return totalDistance;
    }
}
