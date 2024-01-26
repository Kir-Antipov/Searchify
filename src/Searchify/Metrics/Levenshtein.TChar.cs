using Searchify.Comparison;

namespace Searchify.Metrics;

public static partial class Levenshtein
{
    /// <inheritdoc cref="Ratio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static float Ratio<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    )
        => Ratio<TChar, int, float, DefaultComparer<TChar>>(input, pattern, default);

    /// <inheritdoc cref="Ratio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static float Ratio<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    ) where TComparer : IEqualityComparer<TChar>
        => Ratio<TChar, int, float, TComparer>(input, pattern, comparer);

    /// <inheritdoc cref="Ratio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static float Ratio<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => Ratio<TChar, int, float, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="SubsequenceRatio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static float SubsequenceRatio<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    )
        => SubsequenceRatio<TChar, int, float, DefaultComparer<TChar>>(input, pattern, default);

    /// <inheritdoc cref="SubsequenceRatio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static float SubsequenceRatio<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    ) where TComparer : IEqualityComparer<TChar>
        => SubsequenceRatio<TChar, int, float, TComparer>(input, pattern, comparer);

    /// <inheritdoc cref="SubsequenceRatio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static float SubsequenceRatio<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => SubsequenceRatio<TChar, int, float, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Distance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int Distance<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    )
        => Distance<TChar, int, DefaultComparer<TChar>>(input, pattern, default);

    /// <inheritdoc cref="Distance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int Distance<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    ) where TComparer : IEqualityComparer<TChar>
        => Distance<TChar, int, TComparer>(input, pattern, comparer);

    /// <inheritdoc cref="Distance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int Distance<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => Distance<TChar, int, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="SubsequenceDistance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int SubsequenceDistance<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    )
        => SubsequenceDistance<TChar, int, DefaultComparer<TChar>>(input, pattern, default);

    /// <inheritdoc cref="SubsequenceDistance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int SubsequenceDistance<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    ) where TComparer : IEqualityComparer<TChar>
        => SubsequenceDistance<TChar, int, TComparer>(input, pattern, comparer);

    /// <inheritdoc cref="SubsequenceDistance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int SubsequenceDistance<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => SubsequenceDistance<TChar, int, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="IsMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer)"/>
    public static bool IsMatch<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    )
        => IsMatch<TChar, int, DefaultComparer<TChar>>(input, pattern, default);

    /// <inheritdoc cref="IsMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsMatch<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int maxDistance
    )
        => IsMatch<TChar, int, DefaultComparer<TChar>>(input, pattern, default, maxDistance);

    /// <inheritdoc cref="IsMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsMatch<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int maxDistance
    ) where TComparer : IEqualityComparer<TChar>
        => IsMatch<TChar, int, TComparer>(input, pattern, comparer, maxDistance);

    /// <inheritdoc cref="IsMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsMatch<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => IsMatch<TChar, int, TComparer>(input, pattern, comparer, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="IsFullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer)"/>
    public static bool IsFullMatch<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    )
        => IsFullMatch<TChar, int, DefaultComparer<TChar>>(input, pattern, default);

    /// <inheritdoc cref="IsFullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsFullMatch<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int maxDistance
    )
        => IsFullMatch<TChar, int, DefaultComparer<TChar>>(input, pattern, default, maxDistance);

    /// <inheritdoc cref="IsFullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsFullMatch<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int maxDistance
    ) where TComparer : IEqualityComparer<TChar>
        => IsFullMatch<TChar, int, TComparer>(input, pattern, comparer, maxDistance);

    /// <inheritdoc cref="IsFullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsFullMatch<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => IsFullMatch<TChar, int, TComparer>(input, pattern, comparer, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Match{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> Match<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Match(input, pattern, default(DefaultComparer<TChar>), direction: 1, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Match{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> Match<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => Match(input, pattern, comparer, direction: 1, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="LastMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> LastMatch<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Match(input, pattern, default(DefaultComparer<TChar>), direction: -1, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="LastMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> LastMatch<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => Match(input, pattern, comparer, direction: -1, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Match{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> Match<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Match(input, pattern, default(DefaultComparer<TChar>), direction: deletionCost, insertionCost, substitutionCost, 1);

    /// <inheritdoc cref="Match{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> Match<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => Match(input, pattern, comparer, direction: 1, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="LastMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> LastMatch<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Match(input, pattern, default(DefaultComparer<TChar>), direction: -deletionCost, insertionCost, substitutionCost, 1);

    /// <inheritdoc cref="LastMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> LastMatch<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => Match(input, pattern, comparer, direction: -1, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="FullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> FullMatch<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => FullMatch<TChar, int, DefaultComparer<TChar>>(input, pattern, default, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="FullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> FullMatch<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => FullMatch<TChar, int, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Matches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatchCollection<int> Matches<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Matches<TChar, int, DefaultComparer<TChar>>(input, pattern, default, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Matches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatchCollection<int> Matches<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => Matches<TChar, int, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Matches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatchCollection<int> Matches<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Matches<TChar, int, DefaultComparer<TChar>>(input, pattern, default, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Matches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatchCollection<int> Matches<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => Matches<TChar, int, TComparer>(input, pattern, comparer, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="EnumerateMatches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, Span{int})"/>
    public static LevenshteinMatchEnumerator<int> EnumerateMatches<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1,
        Span<int> buffer = default
    )
        => EnumerateMatches<TChar, int, DefaultComparer<TChar>>(input, pattern, default, deletionCost, insertionCost, substitutionCost, buffer);

    /// <inheritdoc cref="EnumerateMatches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, Span{int})"/>
    public static LevenshteinMatchEnumerator<int> EnumerateMatches<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1,
        Span<int> buffer = default
    ) where TComparer : IEqualityComparer<TChar>
        => EnumerateMatches<TChar, int, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost, buffer);

    /// <inheritdoc cref="EnumerateMatches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance, Span{int})"/>
    public static LevenshteinMatchEnumerator<int> EnumerateMatches<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1,
        Span<int> buffer = default
    )
        => EnumerateMatches<TChar, int, DefaultComparer<TChar>>(input, pattern, default, maxDistance, deletionCost, insertionCost, substitutionCost, buffer);

    /// <inheritdoc cref="EnumerateMatches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance, Span{int})"/>
    public static LevenshteinMatchEnumerator<int> EnumerateMatches<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1,
        Span<int> buffer = default
    ) where TComparer : IEqualityComparer<TChar>
        => EnumerateMatches<TChar, int, TComparer>(input, pattern, comparer, maxDistance, deletionCost, insertionCost, substitutionCost, buffer);

    /// <inheritdoc cref="Count{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int Count<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Count<TChar, int, DefaultComparer<TChar>>(input, pattern, default, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Count{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int Count<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => Count<TChar, int, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Count{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static int Count<TChar>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Count<TChar, int, DefaultComparer<TChar>>(input, pattern, default, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Count{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static int Count<TChar, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<TChar>
        => Count<TChar, int, TComparer>(input, pattern, comparer, maxDistance, deletionCost, insertionCost, substitutionCost);
}
