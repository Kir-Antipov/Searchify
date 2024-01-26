using System.Buffers;
using System.Numerics;
using System.Runtime.InteropServices;
using Searchify.Comparison;
using Spanned;

namespace Searchify.Metrics;

public static partial class Levenshtein
{
    /// <inheritdoc cref="Ratio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static TRatio Ratio<TChar, TDistance, TRatio>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    ) where TDistance : unmanaged, INumber<TDistance> where TRatio : INumber<TRatio>
        => Ratio<TChar, TDistance, TRatio, DefaultComparer<TChar>>(input, pattern, default);

    /// <inheritdoc cref="Ratio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static TRatio Ratio<TChar, TDistance, TRatio, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TRatio : INumber<TRatio>
        where TComparer : IEqualityComparer<TChar>
    {
        TDistance distance = Distance<TChar, TDistance, TComparer>(input, pattern, comparer);
        return TRatio.One - NormalizeDistance<TDistance, TRatio>(distance, input.Length, pattern.Length);
    }

    /// <summary>
    /// Calculates a normalized similarity ratio between two sequences.
    /// </summary>
    /// <remarks>
    /// The similarity ratio is in the range <c>[0, 1]</c>, where
    /// <c>1</c> indicates complete equality, and
    /// <c>0</c> indicates no similarities.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TRatio">The numeric type used to represent the final normalized similarity ratio.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to compare the <paramref name="input"/> against.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// A normalized similarity ratio between two provided sequences.
    /// </returns>
    public static TRatio Ratio<TChar, TDistance, TRatio, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TRatio : INumber<TRatio>
        where TComparer : IEqualityComparer<TChar>
    {
        TDistance distance = Distance(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);
        return TRatio.One - NormalizeDistance<TDistance, TRatio>(distance, input.Length, pattern.Length);
    }

    /// <inheritdoc cref="SubsequenceRatio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static TRatio SubsequenceRatio<TChar, TDistance, TRatio>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    ) where TDistance : unmanaged, INumber<TDistance> where TRatio : INumber<TRatio>
        => SubsequenceRatio<TChar, TDistance, TRatio, DefaultComparer<TChar>>(input, pattern, default);

    /// <inheritdoc cref="SubsequenceRatio{TChar, TDistance, TRatio, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static TRatio SubsequenceRatio<TChar, TDistance, TRatio, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TRatio : INumber<TRatio>
        where TComparer : IEqualityComparer<TChar>
    {
        TDistance distance = SubsequenceDistance<TChar, TDistance, TComparer>(input, pattern, comparer);
        return TRatio.One - NormalizeDistance<TDistance, TRatio>(distance, input.Length, input.Length);
    }

    /// <summary>
    /// Calculates a normalized subsequence similarity ratio between two sequences.
    /// </summary>
    /// <remarks>
    /// The subsequence similarity ratio is in the range <c>[0, 1]</c>, where
    /// <c>1</c> indicates that the <paramref name="input"/> sequence is a subsequence of the <paramref name="pattern"/>, and
    /// <c>0</c> indicates no subsequence similarities.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TRatio">The numeric type used to represent the final normalized similarity ratio.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for the most similar to the <paramref name="input"/> subsequence.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// A normalized subsequence similarity ratio between two provided sequences.
    /// </returns>
    public static TRatio SubsequenceRatio<TChar, TDistance, TRatio, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TRatio : INumber<TRatio>
        where TComparer : IEqualityComparer<TChar>
    {
        TDistance distance = SubsequenceDistance(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);
        return TRatio.One - NormalizeDistance<TDistance, TRatio>(distance, input.Length, input.Length);
    }

    /// <inheritdoc cref="Distance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static TDistance Distance<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    ) where TDistance : unmanaged, INumber<TDistance>
        => Distance<TChar, TDistance, DefaultComparer<TChar>>(input, pattern, default);

    /// <inheritdoc cref="Distance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static TDistance Distance<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        if (input.Length < pattern.Length)
        {
            ReadOnlySpan<TChar> tmp = pattern;
            pattern = input;
            input = tmp;
        }

        int rowLength = pattern.Length + 1;
        SpanOwner<TDistance> rowOwner0 = SpanOwner<TDistance>.ShouldRent(rowLength) ? SpanOwner<TDistance>.Rent(rowLength) : new(stackalloc TDistance[rowLength]);
        SpanOwner<TDistance> rowOwner1 = SpanOwner<TDistance>.ShouldRent(rowLength) ? SpanOwner<TDistance>.Rent(rowLength) : new(stackalloc TDistance[rowLength]);

        TDistance distance = Distance(input, pattern, rowOwner0.Span, rowOwner1.Span, comparer, false, TDistance.One, TDistance.One, TDistance.One);

        rowOwner0.Dispose();
        rowOwner1.Dispose();

        return distance;
    }

    /// <summary>
    /// Calculates the Levenshtein distance between two sequences.
    /// </summary>
    /// <remarks>
    /// <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see> is the minimum number of
    /// single-character edits (insertions, deletions, or substitutions) required to transform one sequence into another.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to compare the <paramref name="input"/> against.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// The Levenshtein distance between two provided sequences.
    /// </returns>
    public static TDistance Distance<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        if (input.Length < pattern.Length)
        {
            ReadOnlySpan<TChar> tmp = pattern;
            pattern = input;
            input = tmp;

            (deletionCost, insertionCost) = (insertionCost, deletionCost);
        }

        int rowLength = pattern.Length + 1;
        SpanOwner<TDistance> rowOwner0 = SpanOwner<TDistance>.ShouldRent(rowLength) ? SpanOwner<TDistance>.Rent(rowLength) : new(stackalloc TDistance[rowLength]);
        SpanOwner<TDistance> rowOwner1 = SpanOwner<TDistance>.ShouldRent(rowLength) ? SpanOwner<TDistance>.Rent(rowLength) : new(stackalloc TDistance[rowLength]);

        TDistance distance = Distance(input, pattern, rowOwner0.Span, rowOwner1.Span, comparer, false, deletionCost, insertionCost, substitutionCost);

        rowOwner0.Dispose();
        rowOwner1.Dispose();

        return distance;
    }

    /// <inheritdoc cref="SubsequenceDistance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static TDistance SubsequenceDistance<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    ) where TDistance : unmanaged, INumber<TDistance>
        => SubsequenceDistance<TChar, TDistance, DefaultComparer<TChar>>(input, pattern, default);

    /// <inheritdoc cref="SubsequenceDistance{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static TDistance SubsequenceDistance<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        int rowLength = pattern.Length + 1;
        SpanOwner<TDistance> rowOwner0 = SpanOwner<TDistance>.ShouldRent(rowLength) ? SpanOwner<TDistance>.Rent(rowLength) : new(stackalloc TDistance[rowLength]);
        SpanOwner<TDistance> rowOwner1 = SpanOwner<TDistance>.ShouldRent(rowLength) ? SpanOwner<TDistance>.Rent(rowLength) : new(stackalloc TDistance[rowLength]);

        TDistance distance = Distance(input, pattern, rowOwner0.Span, rowOwner1.Span, comparer, true, TDistance.One, TDistance.One, TDistance.One);

        rowOwner0.Dispose();
        rowOwner1.Dispose();

        return distance;
    }

    /// <summary>
    /// Calculates the Levenshtein distance between an <paramref name="input"/> sequence and
    /// the most similar subsequence in the <paramref name="pattern"/> sequence.
    /// </summary>
    /// <remarks>
    /// The most similar subsequence is determined based on the minimum number of single-character edits (insertions, deletions, or substitutions)
    /// required to transform the input sequence into a subsequence of the pattern sequence.
    /// <para/>
    /// <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see> is the minimum number of
    /// single-character edits (insertions, deletions, or substitutions) required to transform one sequence into another.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for the most similar to the <paramref name="input"/> subsequence.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// The Levenshtein distance between the <paramref name="input"/> sequence and
    /// the most similar subsequence in the <paramref name="pattern"/> sequence.
    /// </returns>
    public static TDistance SubsequenceDistance<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
         TComparer comparer,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        int rowLength = pattern.Length + 1;
        SpanOwner<TDistance> rowOwner0 = SpanOwner<TDistance>.ShouldRent(rowLength) ? SpanOwner<TDistance>.Rent(rowLength) : new(stackalloc TDistance[rowLength]);
        SpanOwner<TDistance> rowOwner1 = SpanOwner<TDistance>.ShouldRent(rowLength) ? SpanOwner<TDistance>.Rent(rowLength) : new(stackalloc TDistance[rowLength]);

        TDistance distance = Distance(input, pattern, rowOwner0.Span, rowOwner1.Span, comparer, true, deletionCost, insertionCost, substitutionCost);

        rowOwner0.Dispose();
        rowOwner1.Dispose();

        return distance;
    }

    /// <inheritdoc cref="IsMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer)"/>
    public static bool IsMatch<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    ) where TDistance : unmanaged, INumber<TDistance>
        => IsMatch(input, pattern, default(DefaultComparer<TChar>), TDistance.CreateChecked(input.Length * 0.25f));

    /// <summary>
    /// Determines whether there is a subsequence within the <paramref name="pattern"/> sequence that
    /// can be edited with a relative distance not exceeding 25% of the <paramref name="input"/> sequence length
    /// to match the <paramref name="input"/> sequence.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for a matching subsequence.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <returns>
    /// <c>true</c> if there is a subsequence within the <paramref name="pattern"/> sequence that
    /// can be edited with a relative distance not exceeding 25% of the <paramref name="input"/> sequence length
    /// to match the <paramref name="input"/> sequence; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMatch<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => IsMatch(input, pattern, comparer, TDistance.CreateChecked(input.Length * 0.25f));

    /// <inheritdoc cref="IsMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsMatch<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TDistance maxDistance
    ) where TDistance : unmanaged, INumber<TDistance>
        => IsMatch(input, pattern, default(DefaultComparer<TChar>), maxDistance);

    /// <inheritdoc cref="IsMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsMatch<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        TDistance distance = SubsequenceDistance<TChar, TDistance, TComparer>(input, pattern, comparer);
        return distance <= maxDistance;
    }

    /// <summary>
    /// Determines whether there is a subsequence within the <paramref name="pattern"/> sequence that
    /// can be edited with a maximum specified distance to match the <paramref name="input"/> sequence.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for a matching subsequence.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="maxDistance">The maximum allowed Levenshtein Distance for a subsequence to be considered a match.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// <c>true</c> if there is a subsequence within the <paramref name="pattern"/> sequence that
    /// can be edited with a distance less than or equal to <paramref name="maxDistance"/> to
    /// match the <paramref name="input"/> sequence; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMatch<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        TDistance distance = SubsequenceDistance(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);
        return distance <= maxDistance;
    }

    /// <inheritdoc cref="IsFullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer)"/>
    public static bool IsFullMatch<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    ) where TDistance : unmanaged, INumber<TDistance>
        => IsFullMatch(input, pattern, default(DefaultComparer<TChar>), TDistance.CreateChecked(input.Length * 0.25f));

    /// <summary>
    /// Determines whether the Levenshtein distance between two input sequences
    /// does not exceed 25% of the <paramref name="input"/> sequence length.
    /// </summary>
    /// <remarks>
    /// <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see> is the minimum number of
    /// single-character edits (insertions, deletions, or substitutions) required to transform one sequence into another.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to compare the <paramref name="input"/> against.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <returns>
    /// <c>true</c> if the Levenshtein distance between the two input sequences does not exceed
    /// 25% of the <paramref name="input"/> sequence length; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsFullMatch<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => IsFullMatch(input, pattern, comparer, TDistance.CreateChecked(input.Length * 0.25f));

    /// <inheritdoc cref="IsFullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsFullMatch<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TDistance maxDistance
    ) where TDistance : unmanaged, INumber<TDistance>
        => IsFullMatch(input, pattern, default(DefaultComparer<TChar>), maxDistance);

    /// <inheritdoc cref="IsFullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static bool IsFullMatch<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        TDistance distance = Distance<TChar, TDistance, TComparer>(input, pattern, comparer);
        return distance <= maxDistance;
    }

    /// <summary>
    /// Determines whether the Levenshtein distance between two input sequences
    /// does not exceed the specified maximum distance.
    /// </summary>
    /// <remarks>
    /// <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see> is the minimum number of
    /// single-character edits (insertions, deletions, or substitutions) required to transform one sequence into another.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to compare the <paramref name="input"/> against.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="maxDistance">The maximum allowed Levenshtein distance for the two sequences to be considered a match.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// <c>true</c> if the Levenshtein distance between the two input sequences does not exceed the specified maximum distance;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool IsFullMatch<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        TDistance distance = Distance(input, pattern, comparer, deletionCost, insertionCost, substitutionCost);
        return distance <= maxDistance;
    }

    /// <inheritdoc cref="Match{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<TDistance> Match<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TDistance maxDistance
    ) where TDistance : unmanaged, INumber<TDistance>
        => Match(input, pattern, default(DefaultComparer<TChar>), direction: 1, maxDistance, TDistance.One, TDistance.One, TDistance.One);

    /// <inheritdoc cref="Match{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<TDistance> Match<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => Match(input, pattern, comparer, direction: 1, maxDistance, TDistance.One, TDistance.One, TDistance.One);

    /// <summary>
    /// Searches for the first subsequence within the <paramref name="pattern"/> sequence that
    /// can be edited with a maximum specified distance to match the <paramref name="input"/> sequence.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for a matching subsequence.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="maxDistance">The maximum allowed Levenshtein Distance for a subsequence to be considered a match.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// An object that contains information about the match.
    /// </returns>
    public static LevenshteinMatch<TDistance> Match<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => Match(input, pattern, comparer, direction: 1, maxDistance, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="LastMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<TDistance> LastMatch<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TDistance maxDistance
    ) where TDistance : unmanaged, INumber<TDistance>
        => Match(input, pattern, default(DefaultComparer<TChar>), direction: -1, maxDistance, TDistance.One, TDistance.One, TDistance.One);

    /// <inheritdoc cref="LastMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<TDistance> LastMatch<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => Match(input, pattern, comparer, direction: -1, maxDistance, TDistance.One, TDistance.One, TDistance.One);

    /// <summary>
    /// Searches for the last subsequence within the <paramref name="pattern"/> sequence that
    /// can be edited with a maximum specified distance to match the <paramref name="input"/> sequence.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for a matching subsequence.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="maxDistance">The maximum allowed Levenshtein Distance for a subsequence to be considered a match.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// An object that contains information about the match.
    /// </returns>
    public static LevenshteinMatch<TDistance> LastMatch<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => Match(input, pattern, comparer, direction: -1, maxDistance, deletionCost, insertionCost, substitutionCost);

    private static LevenshteinMatch<TDistance> Match<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int direction,
        TDistance maxDistance,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        int rowLength = pattern.Length + 1;
        SpanOwner<LevenshteinTransform> rowOwner0 = SpanOwner<LevenshteinTransform>.ShouldRent(rowLength) ? SpanOwner<LevenshteinTransform>.Rent(rowLength) : new(stackalloc LevenshteinTransform[rowLength]);
        SpanOwner<LevenshteinTransform> rowOwner1 = SpanOwner<LevenshteinTransform>.ShouldRent(rowLength) ? SpanOwner<LevenshteinTransform>.Rent(rowLength) : new(stackalloc LevenshteinTransform[rowLength]);

        Span<LevenshteinTransform> transforms = Transforms(input, pattern, rowOwner0.Span, rowOwner1.Span, comparer, true, deletionCost, insertionCost, substitutionCost);
        int transformIndex = IndexOfNextTransform(transforms, direction > 0 ? -1 : transforms.Length, direction, input.Length, maxDistance, deletionCost, insertionCost, substitutionCost, out (int Index, int Length, TDistance Distance) data);
        LevenshteinMatch<TDistance> match;
        if ((uint)transformIndex < (uint)transforms.Length)
        {
            ref LevenshteinTransform transform = ref transforms[transformIndex];
            match = new(data.Index, data.Length, transform.Deletions, transform.Insertions, transform.Substitutions, data.Distance);
        }
        else
        {
            match = default;
        }

        rowOwner0.Dispose();
        rowOwner1.Dispose();

        return match;
    }

    /// <inheritdoc cref="Match{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<TDistance> Match<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    ) where TDistance : unmanaged, INumber<TDistance>
        => Match(input, pattern, default(DefaultComparer<TChar>), direction: 1, TDistance.One, TDistance.One, TDistance.One);

    /// <inheritdoc cref="Match{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<TDistance> Match<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => Match(input, pattern, comparer, direction: 1, TDistance.One, TDistance.One, TDistance.One);

    /// <summary>
    /// Searches for the first subsequence within the <paramref name="pattern"/> sequence that
    /// requires the fewest edits to match the <paramref name="input"/> sequence.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for a matching subsequence.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// An object that contains information about the match.
    /// </returns>
    public static LevenshteinMatch<TDistance> Match<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => Match(input, pattern, comparer, direction: 1, deletionCost, insertionCost, substitutionCost);

    /// <inheritdoc cref="LastMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<TDistance> LastMatch<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    ) where TDistance : unmanaged, INumber<TDistance>
        => Match(input, pattern, default(DefaultComparer<TChar>), direction: -1, TDistance.One, TDistance.One, TDistance.One);

    /// <inheritdoc cref="LastMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<TDistance> LastMatch<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => Match(input, pattern, comparer, direction: -1, TDistance.One, TDistance.One, TDistance.One);

    /// <summary>
    /// Searches for the last subsequence within the <paramref name="pattern"/> sequence that
    /// requires the fewest edits to match the <paramref name="input"/> sequence.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for a matching subsequence.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// An object that contains information about the match.
    /// </returns>
    public static LevenshteinMatch<TDistance> LastMatch<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => Match(input, pattern, comparer, direction: -1, deletionCost, insertionCost, substitutionCost);

    private static LevenshteinMatch<TDistance> Match<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        int direction,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        int rowLength = pattern.Length + 1;
        SpanOwner<LevenshteinTransform> rowOwner0 = SpanOwner<LevenshteinTransform>.ShouldRent(rowLength) ? SpanOwner<LevenshteinTransform>.Rent(rowLength) : new(stackalloc LevenshteinTransform[rowLength]);
        SpanOwner<LevenshteinTransform> rowOwner1 = SpanOwner<LevenshteinTransform>.ShouldRent(rowLength) ? SpanOwner<LevenshteinTransform>.Rent(rowLength) : new(stackalloc LevenshteinTransform[rowLength]);

        Span<LevenshteinTransform> transforms = Transforms(input, pattern, rowOwner0.Span, rowOwner1.Span, comparer, true, deletionCost, insertionCost, substitutionCost);
        int transformIndex = IndexOfNextTransform(transforms, direction > 0 ? -1 : transforms.Length, direction, input.Length, deletionCost, insertionCost, substitutionCost, out (int Index, int Length, TDistance Distance) data);
        LevenshteinMatch<TDistance> match;
        if ((uint)transformIndex < (uint)transforms.Length)
        {
            ref LevenshteinTransform transform = ref transforms[transformIndex];
            match = new(data.Index, data.Length, transform.Deletions, transform.Insertions, transform.Substitutions, data.Distance);
        }
        else
        {
            match = default;
        }

        rowOwner0.Dispose();
        rowOwner1.Dispose();

        return match;
    }

    /// <inheritdoc cref="FullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<TDistance> FullMatch<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    ) where TDistance : unmanaged, INumber<TDistance>
        => FullMatch(input, pattern, default(DefaultComparer<TChar>), TDistance.One, TDistance.One, TDistance.One);

    /// <inheritdoc cref="FullMatch{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatch<TDistance> FullMatch<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => FullMatch(input, pattern, comparer, TDistance.One, TDistance.One, TDistance.One);

    /// <summary>
    /// Compares two sequences, returning a match information that contains the number of edits needed
    /// to transform the <paramref name="input"/> sequence into the <paramref name="pattern"/> sequence.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to compare the <paramref name="input"/> against.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// An object that contains information about the match.
    /// </returns>
    public static LevenshteinMatch<TDistance> FullMatch<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        int rowLength = pattern.Length + 1;
        SpanOwner<LevenshteinTransform> rowOwner0 = SpanOwner<LevenshteinTransform>.ShouldRent(rowLength) ? SpanOwner<LevenshteinTransform>.Rent(rowLength) : new(stackalloc LevenshteinTransform[rowLength]);
        SpanOwner<LevenshteinTransform> rowOwner1 = SpanOwner<LevenshteinTransform>.ShouldRent(rowLength) ? SpanOwner<LevenshteinTransform>.Rent(rowLength) : new(stackalloc LevenshteinTransform[rowLength]);

        Span<LevenshteinTransform> transforms = Transforms(input, pattern, rowOwner0.Span, rowOwner1.Span, comparer, false, deletionCost, insertionCost, substitutionCost);
        ref LevenshteinTransform transform = ref transforms[pattern.Length];
        TDistance distance = transform.ToDistance(deletionCost, insertionCost, substitutionCost);
        LevenshteinMatch<TDistance> match = new(0, pattern.Length, transform.Deletions, transform.Insertions, transform.Substitutions, distance);

        rowOwner0.Dispose();
        rowOwner1.Dispose();

        return match;
    }

    /// <inheritdoc cref="Matches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatchCollection<TDistance> Matches<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    ) where TDistance : unmanaged, INumber<TDistance>
        => Matches(input, pattern, default(DefaultComparer<TChar>), TDistance.One, TDistance.One, TDistance.One);

    /// <inheritdoc cref="Matches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatchCollection<TDistance> Matches<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => Matches(input, pattern, comparer, TDistance.One, TDistance.One, TDistance.One);

    /// <summary>
    /// Searches for all subsequences within the <paramref name="pattern"/> sequence that
    /// requires the fewest edits to match the <paramref name="input"/> sequence.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for matching subsequences.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// A collection of the <see cref="LevenshteinMatchCollection{TDistance}"/> objects found by the
    /// search. If no matches are found, the method returns an empty collection object.
    /// </returns>
    public static LevenshteinMatchCollection<TDistance> Matches<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        int rowLength = pattern.Length + 1;
        LevenshteinTransform[] rowOwner0 = ArrayPool<LevenshteinTransform>.Shared.Rent(rowLength);
        LevenshteinTransform[] rowOwner1 = ArrayPool<LevenshteinTransform>.Shared.Rent(rowLength);
        Span<LevenshteinTransform> row0 = rowOwner0.AsSpan(0, rowLength);
        Span<LevenshteinTransform> row1 = rowOwner1.AsSpan(0, rowLength);

        Span<LevenshteinTransform> transforms = Transforms(input, pattern, row0, row1, comparer, true, deletionCost, insertionCost, substitutionCost);
        (rowOwner0, rowOwner1) = transforms == row0 ? (rowOwner0, rowOwner1) : (rowOwner1, rowOwner0);
        ArrayPool<LevenshteinTransform>.Shared.Return(rowOwner1);

        return new(rowOwner0, rowLength, input.Length, deletionCost, insertionCost, substitutionCost);
    }

    /// <inheritdoc cref="Matches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatchCollection<TDistance> Matches<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TDistance maxDistance
    ) where TDistance : unmanaged, INumber<TDistance>
        => Matches(input, pattern, default(DefaultComparer<TChar>), maxDistance, TDistance.One, TDistance.One, TDistance.One);

    /// <inheritdoc cref="Matches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static LevenshteinMatchCollection<TDistance> Matches<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => Matches(input, pattern, comparer, maxDistance, TDistance.One, TDistance.One, TDistance.One);

    /// <summary>
    /// Searches for all subsequences within the <paramref name="pattern"/> sequence that
    /// can be edited with a maximum specified distance to match the <paramref name="input"/> sequence.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for matching subsequences.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="maxDistance">The maximum allowed Levenshtein Distance for a subsequence to be considered a match.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// A collection of the <see cref="LevenshteinMatchCollection{TDistance}"/> objects found by the
    /// search. If no matches are found, the method returns an empty collection object.
    /// </returns>
    public static LevenshteinMatchCollection<TDistance> Matches<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        int rowLength = pattern.Length + 1;
        LevenshteinTransform[] rowOwner0 = ArrayPool<LevenshteinTransform>.Shared.Rent(rowLength);
        LevenshteinTransform[] rowOwner1 = ArrayPool<LevenshteinTransform>.Shared.Rent(rowLength);
        Span<LevenshteinTransform> row0 = rowOwner0.AsSpan(0, rowLength);
        Span<LevenshteinTransform> row1 = rowOwner1.AsSpan(0, rowLength);

        Span<LevenshteinTransform> transforms = Transforms(input, pattern, row0, row1, comparer, true, deletionCost, insertionCost, substitutionCost);
        (rowOwner0, rowOwner1) = transforms == row0 ? (rowOwner0, rowOwner1) : (rowOwner1, rowOwner0);
        ArrayPool<LevenshteinTransform>.Shared.Return(rowOwner1);

        return new(rowOwner0, rowLength, input.Length, maxDistance, deletionCost, insertionCost, substitutionCost);
    }

    /// <inheritdoc cref="EnumerateMatches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, Span{int})"/>
    public static LevenshteinMatchEnumerator<TDistance> EnumerateMatches<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        Span<int> buffer = default
    ) where TDistance : unmanaged, INumber<TDistance>
        => EnumerateMatches(input, pattern, default(DefaultComparer<TChar>), TDistance.One, TDistance.One, TDistance.One, buffer);

    /// <inheritdoc cref="EnumerateMatches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, Span{int})"/>
    public static LevenshteinMatchEnumerator<TDistance> EnumerateMatches<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        Span<int> buffer = default
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => EnumerateMatches(input, pattern, comparer, TDistance.One, TDistance.One, TDistance.One, buffer);

    /// <summary>
    /// Searches for all subsequences within the <paramref name="pattern"/> sequence that
    /// requires the fewest edits to match the <paramref name="input"/> sequence.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for matching subsequences.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <param name="buffer">
    /// The buffer required to perform the calculations.
    /// You can provide it to reduce the number of allocations.
    /// <para/>
    /// Call the <see cref="GetBufferSize(int)"/> method, passing the length of the
    /// <paramref name="pattern"/> sequence, to obtain the required buffer size.
    /// </param>
    /// <returns>
    /// A <see cref="LevenshteinMatchEnumerator{TDistance}"/> to iterate over the matches.
    /// </returns>
    public static LevenshteinMatchEnumerator<TDistance> EnumerateMatches<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost,
        Span<int> buffer = default
    )
        where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        int rowLength = pattern.Length + 1;
        Span<LevenshteinTransform> transformBuffer = MemoryMarshal.Cast<int, LevenshteinTransform>(buffer);
        SpanOwner<LevenshteinTransform> rowOwner0 = transformBuffer.Length < rowLength ? SpanOwner<LevenshteinTransform>.Rent(rowLength) : new(transformBuffer.Slice(0, rowLength));
        SpanOwner<LevenshteinTransform> rowOwner1 = transformBuffer.Length < rowLength * 2 ? SpanOwner<LevenshteinTransform>.Rent(rowLength) : new(transformBuffer.Slice(rowLength, rowLength));

        Span<LevenshteinTransform> transforms = Transforms(input, pattern, rowOwner0.Span, rowOwner1.Span, comparer, true, deletionCost, insertionCost, substitutionCost);
        if (transforms == rowOwner0.Span)
            rowOwner1.Dispose();
        else
            rowOwner0.Dispose();

        return new(transforms, input.Length, deletionCost, insertionCost, substitutionCost);
    }

    /// <inheritdoc cref="EnumerateMatches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance, Span{int})"/>
    public static LevenshteinMatchEnumerator<TDistance> EnumerateMatches<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TDistance maxDistance,
        Span<int> buffer = default
    ) where TDistance : unmanaged, INumber<TDistance>
        => EnumerateMatches(input, pattern, default(DefaultComparer<TChar>), maxDistance, TDistance.One, TDistance.One, TDistance.One, buffer);

    /// <inheritdoc cref="EnumerateMatches{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance, Span{int})"/>
    public static LevenshteinMatchEnumerator<TDistance> EnumerateMatches<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance,
        Span<int> buffer = default
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => EnumerateMatches(input, pattern, comparer, maxDistance, TDistance.One, TDistance.One, TDistance.One, buffer);

    /// <summary>
    /// Searches for all subsequences within the <paramref name="pattern"/> sequence that
    /// can be edited with a maximum specified distance to match the <paramref name="input"/> sequence.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for matching subsequences.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="maxDistance">The maximum allowed Levenshtein Distance for a subsequence to be considered a match.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <param name="buffer">
    /// The buffer required to perform the calculations.
    /// You can provide it to reduce the number of allocations.
    /// <para/>
    /// Call the <see cref="GetBufferSize(int)"/> method, passing the length of the
    /// <paramref name="pattern"/> sequence, to obtain the required buffer size.
    /// </param>
    /// <returns>
    /// A <see cref="LevenshteinMatchEnumerator{TDistance}"/> to iterate over the matches.
    /// </returns>
    public static LevenshteinMatchEnumerator<TDistance> EnumerateMatches<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost,
        Span<int> buffer = default
    )
        where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        int rowLength = pattern.Length + 1;
        Span<LevenshteinTransform> transformBuffer = MemoryMarshal.Cast<int, LevenshteinTransform>(buffer);
        SpanOwner<LevenshteinTransform> rowOwner0 = transformBuffer.Length < rowLength ? SpanOwner<LevenshteinTransform>.Rent(rowLength) : new(transformBuffer.Slice(0, rowLength));
        SpanOwner<LevenshteinTransform> rowOwner1 = transformBuffer.Length < rowLength * 2 ? SpanOwner<LevenshteinTransform>.Rent(rowLength) : new(transformBuffer.Slice(rowLength, rowLength));

        Span<LevenshteinTransform> transforms = Transforms(input, pattern, rowOwner0.Span, rowOwner1.Span, comparer, true, deletionCost, insertionCost, substitutionCost);
        if (transforms == rowOwner0.Span)
            rowOwner1.Dispose();
        else
            rowOwner0.Dispose();

        return new(transforms, input.Length, maxDistance, deletionCost, insertionCost, substitutionCost);
    }

    /// <inheritdoc cref="Count{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int Count<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern
    ) where TDistance : unmanaged, INumber<TDistance>
        => Count(input, pattern, default(DefaultComparer<TChar>), TDistance.One, TDistance.One, TDistance.One);

    /// <inheritdoc cref="Count{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance)"/>
    public static int Count<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => Count(input, pattern, comparer, TDistance.One, TDistance.One, TDistance.One);

    /// <summary>
    /// Searches for all subsequences within the <paramref name="pattern"/> sequence that
    /// requires the fewest edits to match the <paramref name="input"/> sequence
    /// and returns the number of matches.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for matching subsequences.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// The number of matches.
    /// </returns>
    public static int Count<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
    {
        int count = 0;
        int bufferSize = GetBufferSize(pattern.Length);
        SpanOwner<int> bufferOwner = SpanOwner<int>.ShouldRent(bufferSize)
            ? SpanOwner<int>.ShouldRent(bufferSize / 2)
            ? SpanOwner<int>.Rent(bufferSize)
                : new(stackalloc int[bufferSize / 2])
                : new(stackalloc int[bufferSize]);

        LevenshteinMatchEnumerator<TDistance> enumerator = EnumerateMatches(input, pattern, comparer, deletionCost, insertionCost, substitutionCost, bufferOwner.Span);
        while (enumerator.MoveNext())
            count++;

        enumerator.Dispose();
        bufferOwner.Dispose();
        return count;
    }

    /// <inheritdoc cref="Count{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static int Count<TChar, TDistance>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TDistance maxDistance
    ) where TDistance : unmanaged, INumber<TDistance>
        => Count(input, pattern, default(DefaultComparer<TChar>), maxDistance, TDistance.One, TDistance.One, TDistance.One);

    /// <inheritdoc cref="Count{TChar, TDistance, TComparer}(ReadOnlySpan{TChar}, ReadOnlySpan{TChar}, TComparer, TDistance, TDistance, TDistance, TDistance)"/>
    public static int Count<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance
    ) where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
        => Count(input, pattern, comparer, maxDistance, TDistance.One, TDistance.One, TDistance.One);

    /// <summary>
    /// Searches for all subsequences within the <paramref name="pattern"/> sequence that
    /// can be edited with a maximum specified distance to match the <paramref name="input"/> sequence
    /// and returns the number of matches.
    /// </summary>
    /// <remarks>
    /// The edit distance is calculated using the <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance</see>.
    /// </remarks>
    /// <typeparam name="TChar">The type of characters in the input and pattern sequences.</typeparam>
    /// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
    /// <typeparam name="TComparer">The type implementing <see cref="IEqualityComparer{T}"/> to compare characters for equality.</typeparam>
    /// <param name="input">A span representing the input sequence.</param>
    /// <param name="pattern">A span representing the pattern sequence to search for matching subsequences.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> implementation for comparing characters for equality.</param>
    /// <param name="maxDistance">The maximum allowed Levenshtein Distance for a subsequence to be considered a match.</param>
    /// <param name="deletionCost">The cost associated with deleting a character in the dynamic programming distance calculation.</param>
    /// <param name="insertionCost">The cost associated with inserting a character in the dynamic programming distance calculation.</param>
    /// <param name="substitutionCost">The cost associated with substituting one character for another in the dynamic programming distance calculation.</param>
    /// <returns>
    /// The number of matches.
    /// </returns>
    public static int Count<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> input,
        scoped ReadOnlySpan<TChar> pattern,
        TComparer comparer,
        TDistance maxDistance,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance> where TComparer : IEqualityComparer<TChar>
    {
        int count = 0;
        int bufferSize = GetBufferSize(pattern.Length);
        SpanOwner<int> bufferOwner = SpanOwner<int>.ShouldRent(bufferSize)
            ? SpanOwner<int>.ShouldRent(bufferSize / 2)
            ? SpanOwner<int>.Rent(bufferSize)
                : new(stackalloc int[bufferSize / 2])
                : new(stackalloc int[bufferSize]);

        LevenshteinMatchEnumerator<TDistance> enumerator = EnumerateMatches(input, pattern, comparer, maxDistance, deletionCost, insertionCost, substitutionCost, bufferOwner.Span);
        while (enumerator.MoveNext())
            count++;

        enumerator.Dispose();
        bufferOwner.Dispose();
        return count;
    }
}
