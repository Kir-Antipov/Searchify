namespace Searchify.Collections.Generic;

/// <summary>
/// Represents a match in a Burkhard-Keller Tree, consisting of a value and its corresponding distance.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
/// <typeparam name="TDistance">The type of the distance.</typeparam>
/// <param name="Value">The match value.</param>
/// <param name="Distance">The match distance.</param>
public readonly record struct BKTreeMatch<TValue, TDistance>(TValue Value, TDistance Distance)
{
    /// <summary>
    /// Converts a <see cref="BKTreeMatch{TValue, TDistance}"/> to its value.
    /// </summary>
    /// <param name="match">The match to convert.</param>
    /// <returns>The value of the match.</returns>
    public static implicit operator TValue(BKTreeMatch<TValue, TDistance> match) => match.Value;
}
