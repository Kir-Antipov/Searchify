namespace Searchify;

/// <summary>
/// Represents a generic search provider that allows searching for items based on a specified query.
/// </summary>
/// <typeparam name="TQuery">The type of the search query.</typeparam>
/// <typeparam name="TItem">The type of the items being searched.</typeparam>
public interface ISearchProvider<TQuery, TItem>
{
    /// <summary>
    /// Searches for an item based on the specified query.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="options">The search options.</param>
    /// <returns>An instance of <see cref="ISearchResult{TItem}"/> containing the search result and suggestions, if any.</returns>
    ISearchResult<TItem> Search(TQuery query, SearchOptions options = default);

    /// <summary>
    /// Searches for the last occurrence of an item based on the specified query.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="options">The search options.</param>
    /// <returns>An instance of <see cref="ISearchResult{TItem}"/> containing the search result and suggestions, if any.</returns>
    ISearchResult<TItem> SearchLast(TQuery query, SearchOptions options = default);

    /// <summary>
    /// Asynchronously searches for an item based on the specified query.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="options">The search options.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An instance of <see cref="ISearchResult{TItem}"/> containing the search result and suggestions, if any.</returns>
    ValueTask<ISearchResult<TItem>> SearchAsync(TQuery query, SearchOptions options = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously searches for the last occurrence of an item based on the specified query.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="options">The search options.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An instance of <see cref="ISearchResult{TItem}"/> containing the search result and suggestions, if any.</returns>
    ValueTask<ISearchResult<TItem>> SearchLastAsync(TQuery query, SearchOptions options = default, CancellationToken cancellationToken = default);
}
