namespace Searchify;

/// <summary>
/// Represents a suggestion in a search operation with a corresponding rank.
/// </summary>
/// <typeparam name="T">The type of the suggested value.</typeparam>
/// <param name="Value">The suggested value.</param>
/// <param name="Rank">
/// The rank associated with the suggestion.
/// <para/>
/// The rank is a normalized value between 0 (close match) and 1 (complete mismatch).
/// </param>
public readonly record struct SearchSuggestion<T>(T Value, float Rank)
{
    /// <summary>
    /// Converts a <see cref="SearchSuggestion{T}"/> to the suggested value.
    /// </summary>
    /// <param name="suggestion">The search suggestion.</param>
    /// <returns>The suggested value.</returns>
    public static implicit operator T(SearchSuggestion<T> suggestion) => suggestion.Value;
}
