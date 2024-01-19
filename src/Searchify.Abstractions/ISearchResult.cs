using System.Diagnostics.CodeAnalysis;

namespace Searchify;

/// <summary>
/// Represents the result of a search operation.
/// </summary>
/// <typeparam name="T">The type of elements in the search result.</typeparam>
public interface ISearchResult<T> : IEnumerable<T>
{
    /// <summary>
    /// Indicates whether the search operation was successful.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    bool Success { get; }

    /// <summary>
    /// The value representing the result of the search operation, if successful.
    /// </summary>
    T? Value { get; }

    /// <summary>
    /// A collection of search suggestions.
    /// </summary>
    IEnumerable<SearchSuggestion<T>> Suggestions { get; }
}
