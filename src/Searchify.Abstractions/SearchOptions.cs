namespace Searchify;

/// <summary>
/// Represents configuration options for a search operation.
/// </summary>
/// <param name="MaxSuggestions">
/// The maximum number of suggestions to be returned by the search.
/// <para/>
/// A value of <c>0</c> (default) disables the suggestion feature.
/// <para/>
/// A value of <c>-1</c> signifies that there is no limit to the number of suggestions.
/// </param>
public readonly record struct SearchOptions(int MaxSuggestions);
