using Searchify.Comparison;

namespace Searchify.Metrics;

public static partial class Levenshtein
{
    /// <inheritdoc cref="Ratio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static float Ratio(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern
    )
        => Ratio<char, int, float, OrdinalCaseSensitiveCharComparer>(input, pattern, default);

    /// <inheritdoc cref="Ratio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static float Ratio<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer
    ) where TComparer : IEqualityComparer<char>
        => Ratio<char, int, float, TComparer>(input, pattern, comparer);

    /// <inheritdoc cref="Ratio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static float Ratio<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => Ratio<char, int, float, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="SubsequenceRatio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static float SubsequenceRatio(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern
    )
        => SubsequenceRatio<char, int, float, OrdinalCaseSensitiveCharComparer>(input, pattern, default);

    /// <inheritdoc cref="SubsequenceRatio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static float SubsequenceRatio<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer
    ) where TComparer : IEqualityComparer<char>
        => SubsequenceRatio<char, int, float, TComparer>(input, pattern, comparer);

    /// <inheritdoc cref="SubsequenceRatio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static float SubsequenceRatio<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => SubsequenceRatio<char, int, float, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Distance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int Distance(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern
    )
        => Distance<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default);

    /// <inheritdoc cref="Distance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int Distance<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer
    ) where TComparer : IEqualityComparer<char>
        => Distance<char, int, TComparer>(input, pattern, comparer);

    /// <inheritdoc cref="Distance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int Distance<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => Distance<char, int, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="SubsequenceDistance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int SubsequenceDistance(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern
    )
        => SubsequenceDistance<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default);

    /// <inheritdoc cref="SubsequenceDistance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int SubsequenceDistance<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer
    ) where TComparer : IEqualityComparer<char>
        => SubsequenceDistance<char, int, TComparer>(input, pattern, comparer);

    /// <inheritdoc cref="SubsequenceDistance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int SubsequenceDistance<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => SubsequenceDistance<char, int, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="IsMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer)"/>
    public static bool IsMatch(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern
    )
        => IsMatch<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default);

    /// <inheritdoc cref="IsMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer)"/>
    public static bool IsMatch<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer
    ) where TComparer : IEqualityComparer<char>
        => IsMatch<char, int, TComparer>(input, pattern, comparer);

    /// <inheritdoc cref="IsMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsMatch(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int maxDistance
    )
        => IsMatch<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default, maxDistance);

    /// <inheritdoc cref="IsMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsMatch<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int maxDistance
    ) where TComparer : IEqualityComparer<char>
        => IsMatch<char, int, TComparer>(input, pattern, comparer, maxDistance);

    /// <inheritdoc cref="IsMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsMatch<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => IsMatch<char, int, TComparer>(input, pattern, comparer, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="IsFullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer)"/>
    public static bool IsFullMatch(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern
    )
        => IsFullMatch<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default);

    /// <inheritdoc cref="IsFullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer)"/>
    public static bool IsFullMatch<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer
    ) where TComparer : IEqualityComparer<char>
        => IsFullMatch<char, int, TComparer>(input, pattern, comparer);

    /// <inheritdoc cref="IsFullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsFullMatch(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int maxDistance
    )
        => IsFullMatch<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default, maxDistance);

    /// <inheritdoc cref="IsFullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsFullMatch<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int maxDistance
    ) where TComparer : IEqualityComparer<char>
        => IsFullMatch<char, int, TComparer>(input, pattern, comparer, maxDistance);

    /// <inheritdoc cref="IsFullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsFullMatch<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => IsFullMatch<char, int, TComparer>(input, pattern, comparer, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Match{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> Match(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Match(input, pattern, default(OrdinalCaseSensitiveCharComparer), direction: 1, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Match{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> Match<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => Match(input, pattern, comparer, direction: 1, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="LastMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> LastMatch(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Match(input, pattern, default(OrdinalCaseSensitiveCharComparer), direction: -1, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="LastMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> LastMatch<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => Match(input, pattern, comparer, direction: -1, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Match{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> Match(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Match(input, pattern, default(OrdinalCaseSensitiveCharComparer), direction: 1, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Match{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> Match<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => Match(input, pattern, comparer, direction: 1, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="LastMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> LastMatch(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Match(input, pattern, default(OrdinalCaseSensitiveCharComparer), direction: -1, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="LastMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> LastMatch<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => Match(input, pattern, comparer, direction: -1, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="FullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> FullMatch(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => FullMatch<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="FullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<int> FullMatch<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => FullMatch<char, int, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Matches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatchCollection<int> Matches(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Matches<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Matches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatchCollection<int> Matches<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => Matches<char, int, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Matches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatchCollection<int> Matches(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Matches<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Matches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatchCollection<int> Matches<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => Matches<char, int, TComparer>(input, pattern, comparer, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="EnumerateMatches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, Span{int})"/>
    public static LevenshteinMatchEnumerator<int> EnumerateMatches(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1,
        Span<int> buffer = default
    )
        => EnumerateMatches<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default, deletionCost, insertionCost, substitutionCost, buffer);

    /// <inheritdoc cref="EnumerateMatches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, Span{int})"/>
    public static LevenshteinMatchEnumerator<int> EnumerateMatches<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1,
        Span<int> buffer = default
    ) where TComparer : IEqualityComparer<char>
        => EnumerateMatches<char, int, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost, buffer);

    /// <inheritdoc cref="EnumerateMatches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance, Span{int})"/>
    public static LevenshteinMatchEnumerator<int> EnumerateMatches(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1,
        Span<int> buffer = default
    )
        => EnumerateMatches<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default, maxDistance, deletionCost, insertionCost, substitutionCost, buffer);

    /// <inheritdoc cref="EnumerateMatches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance, Span{int})"/>
    public static LevenshteinMatchEnumerator<int> EnumerateMatches<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost,
        int insertionCost,
        int substitutionCost,
        Span<int> buffer = default
    ) where TComparer : IEqualityComparer<char>
        => EnumerateMatches<char, int, TComparer>(input, pattern, comparer, maxDistance, deletionCost, insertionCost, substitutionCost, buffer);

    /// <inheritdoc cref="Count{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int Count(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Count<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Count{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int Count<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => Count<char, int, TComparer>(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Count{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static int Count(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        => Count<char, int, OrdinalCaseSensitiveCharComparer>(input, pattern, default, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="Count{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static int Count<TComparer>(
        scoped ReadOnlySpan<char> input,
        scoped ReadOnlySpan<char> pattern,
        TComparer comparer,
        int maxDistance,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    ) where TComparer : IEqualityComparer<char>
        => Count<char, int, TComparer>(input, pattern, comparer, deletionCost, maxDistance, insertionCost, substitutionCost);
}
