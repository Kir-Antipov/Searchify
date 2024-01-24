using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Searchify.Metrics;

[DebuggerDisplay($"{{{nameof(TotalEdits)},nq}}")]
internal readonly struct LevenshteinTransform
{
    public readonly int Deletions;

    public readonly int Insertions;

    public readonly int Substitutions;

    internal int TotalEdits => Deletions + Insertions + Substitutions;

    internal LevenshteinTransform(int deletions = 0, int insertions = 0, int substitutions = 0)
    {
        Deletions = deletions;
        Insertions = insertions;
        Substitutions = substitutions;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal LevenshteinTransform Increment(int deletions = 0, int insertions = 0, int substitutions = 0) => new
    (
        deletions: Deletions + deletions,
        insertions: Insertions + insertions,
        substitutions: Substitutions + substitutions
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal T ToDistance<T>(T deletionCost, T insertionCost, T substitutionCost) where T : INumberBase<T>
        => T.CreateChecked(Deletions) * deletionCost
            + T.CreateChecked(Insertions) * insertionCost
            + T.CreateChecked(Substitutions) * substitutionCost;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static LevenshteinTransform Min<T>(
        LevenshteinTransform a,
        LevenshteinTransform b,
        LevenshteinTransform c,
        T deletionCost,
        T insertionCost,
        T substitutionCost
    )
        where T : INumber<T>
    {
        T aDistance = a.ToDistance(deletionCost, insertionCost, substitutionCost);
        T bDistance = b.ToDistance(deletionCost, insertionCost, substitutionCost);
        T cDistance = c.ToDistance(deletionCost, insertionCost, substitutionCost);

        if (aDistance <= bDistance && aDistance <= cDistance)
        {
            return a;
        }
        else if (bDistance <= cDistance && bDistance <= aDistance)
        {
            return b;
        }
        else
        {
            return c;
        }
    }
}
