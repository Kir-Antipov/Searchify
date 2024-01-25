using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using Spanned;

namespace Searchify.Metrics;

/// <summary>
/// Provides methods for calculating the Levenshtein distance between two sequences.
/// </summary>
public static partial class Levenshtein
{
    /// <summary>
    /// Returns the required buffer size for performing calculations
    /// in the context of the <c>EnumerateMatches</c> method,
    /// based on the specified length of a pattern sequence.
    /// </summary>
    /// <param name="patternLength">The length of the pattern sequence.</param>
    /// <returns>The required buffer size for performing calculations.</returns>
    public static int GetBufferSize(int patternLength) => (patternLength + 1) * 6;

    private static RDistance NormalizeDistance<TDistance, RDistance>(TDistance distance, int length1, int length2)
        where TDistance : INumber<TDistance>
        where RDistance : INumber<RDistance>
    {
        TDistance maxDistance = TDistance.Max(TDistance.CreateChecked(length1), TDistance.CreateChecked(length2));
        if (TDistance.IsZero(maxDistance))
            return TDistance.IsZero(distance) ? RDistance.Zero : RDistance.One;

        return RDistance.Clamp(RDistance.CreateChecked(distance) / RDistance.CreateChecked(maxDistance), RDistance.Zero, RDistance.One);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TDistance Distance<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> a,
        scoped ReadOnlySpan<TChar> b,
        scoped Span<TDistance> row0,
        scoped Span<TDistance> row1,
        TComparer comparer,
        [ConstantExpected] bool ignoreLocation,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        Debug.Assert(comparer is not null);
        Debug.Assert(row0.Length == b.Length + 1);
        Debug.Assert(row1.Length == b.Length + 1);

        if (a == b)
            return TDistance.Zero;

        if (a.IsEmpty)
            return TDistance.CreateChecked(b.Length) * insertionCost;

        if (b.IsEmpty)
            return TDistance.CreateChecked(a.Length) * deletionCost;

        if (ignoreLocation)
        {
            row0.Clear();
        }
        else
        {
            row0.FillSequential(offset: TDistance.Zero, step: insertionCost);
        }

        for (int i = 0; i < a.Length; i++)
        {
            row1[0] = TDistance.CreateChecked(i + 1) * deletionCost;

            for (int j = 0; j < b.Length; j++)
            {
                TDistance deletion = row0[j + 1] + deletionCost;
                TDistance insertion = row1[j] + insertionCost;
                TDistance substitution = row0[j] + (comparer.Equals(a[i], b[j]) ? TDistance.Zero : substitutionCost);

                row1[j + 1] = TDistance.Min(deletion, TDistance.Min(insertion, substitution));
            }

            Span<TDistance> tmp = row0;
            row0 = row1;
            row1 = tmp;
        }

        return ignoreLocation ? row0.Min() : row0[row0.Length - 1];
    }

    private static Span<LevenshteinTransform> Transforms<TChar, TDistance, TComparer>(
        scoped ReadOnlySpan<TChar> a,
        scoped ReadOnlySpan<TChar> b,
        Span<LevenshteinTransform> row0,
        Span<LevenshteinTransform> row1,
        TComparer comparer,
        [ConstantExpected] bool ignoreLocation,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost
    )
        where TDistance : unmanaged, INumber<TDistance>
        where TComparer : IEqualityComparer<TChar>
    {
        Debug.Assert(comparer is not null);
        Debug.Assert(row0.Length == b.Length + 1);
        Debug.Assert(row1.Length == b.Length + 1);

        if (ignoreLocation)
        {
            row0.Clear();
        }
        else
        {
            for (int i = 0; i < row0.Length; i++)
                row0[i] = new(insertions: i);
        }

        for (int i = 0; i < a.Length; i++)
        {
            row1[0] = new(deletions: i + 1);

            for (int j = 0; j < b.Length; j++)
            {
                LevenshteinTransform deletion = row0[j + 1].Increment(deletions: 1);
                LevenshteinTransform insertion = row1[j].Increment(insertions: 1);
                LevenshteinTransform substitution = comparer.Equals(a[i], b[j]) ? row0[j] : row0[j].Increment(substitutions: 1);

                row1[j + 1] = LevenshteinTransform.Min(deletion, insertion, substitution, deletionCost, insertionCost, substitutionCost);
            }

            Span<LevenshteinTransform> tmp = row0;
            row0 = row1;
            row1 = tmp;
        }

        return row0;
    }

    internal static int IndexOfNextTransform<TDistance>(
        scoped Span<LevenshteinTransform> transforms,
        int currentIndex,
        int direction,
        int inputLength,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost,
        out (int Index, int Length, TDistance Distance) data
    )
        where TDistance : unmanaged, INumber<TDistance>
    {
        int bestIndex = -1;
        TDistance? distance = null;
        (int Index, int Length) location;

        while (true)
        {
            currentIndex = IndexOfNextTransform(transforms, currentIndex, direction, inputLength, out location);
            if ((uint)currentIndex >= (uint)transforms.Length)
                break;

            if (location.Length == 0 && inputLength != 0)
                continue;

            TDistance currentDistance = transforms[currentIndex].ToDistance(deletionCost, insertionCost, substitutionCost);
            if (distance <= currentDistance)
                continue;

            bestIndex = currentIndex;
            distance = currentDistance;
            if (TDistance.IsZero(currentDistance))
                break;
        }

        data = (location.Index, location.Length, distance ?? default);
        return bestIndex;
    }

    internal static int IndexOfNextTransform<TDistance>(
        scoped Span<LevenshteinTransform> transforms,
        int currentIndex,
        int direction,
        int inputLength,
        TDistance maxDistance,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost,
        out (int Index, int Length, TDistance Distance) data
    )
        where TDistance : unmanaged, INumber<TDistance>
    {
        while (true)
        {
            currentIndex = IndexOfNextTransform(transforms, currentIndex, direction, inputLength, out (int Index, int Length) location);
            if ((uint)currentIndex >= (uint)transforms.Length)
                break;

            if (location.Length == 0 && inputLength != 0)
                continue;

            TDistance distance = transforms[currentIndex].ToDistance(deletionCost, insertionCost, substitutionCost);
            if (distance > maxDistance)
                continue;

            data = (location.Index, location.Length, distance);
            return currentIndex;
        }

        data = default;
        return -1;
    }

    private static int IndexOfNextTransform(
        scoped Span<LevenshteinTransform> transforms,
        int currentIndex,
        int direction,
        int inputLength,
        out (int Index, int Length) location)
    {
        int nextIndex = currentIndex + direction;
        if ((uint)nextIndex >= transforms.Length)
        {
            location = default;
            return -1;
        }

        int currentLocation;
        if ((uint)currentIndex < transforms.Length)
        {
            ref LevenshteinTransform currentTransform = ref transforms[currentIndex];
            int currentLength = inputLength - currentTransform.Deletions + currentTransform.Insertions;
            currentLocation = currentIndex - currentLength;
        }
        else
        {
            currentLocation = -1;
        }

        int nextLength;
        int nextLocation;
        int nextEdits;
        while (true)
        {
            ref LevenshteinTransform nextTransform = ref transforms[nextIndex];
            nextLength = inputLength - nextTransform.Deletions + nextTransform.Insertions;
            nextLocation = nextIndex - nextLength;
            nextEdits = nextTransform.TotalEdits;
            if (nextLocation != currentLocation)
                break;

            nextIndex += direction;
            if ((uint)nextIndex >= transforms.Length)
            {
                location = default;
                return -1;
            }
        }

        for (int i = nextIndex + direction; (uint)i < transforms.Length; i += direction)
        {
            ref LevenshteinTransform testTransform = ref transforms[i];
            int testLength = inputLength - testTransform.Deletions + testTransform.Insertions;
            int testLocation = i - testLength;
            int testEdits = testTransform.TotalEdits;
            if (testLocation != nextLocation || testEdits > nextEdits)
                break;

            (nextIndex, nextLength, nextLocation, nextEdits) = (i, testLength, testLocation, testEdits);
        }

        location = (nextLocation, nextLength);
        return nextIndex;
    }
}
