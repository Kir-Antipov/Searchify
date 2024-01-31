using System.Collections.Frozen;
using Searchify.Comparison;
using Searchify.Metrics;
using Searchify.Processing;

namespace Searchify;

/// <summary>
/// Provides methods for creating instances of the <see cref="SearchProvider{TQuery, TItem}"/> class.
/// </summary>
public static class SearchProvider
{
    /// <summary>
    /// Combines multiple search providers into a single search provider.
    /// </summary>
    /// <typeparam name="TQuery">The type of the search query.</typeparam>
    /// <typeparam name="TItem">The type of the items being searched.</typeparam>
    /// <param name="searchProviders">The search providers to combine.</param>
    /// <returns>The combined search provider.</returns>
    public static SearchProvider<TQuery, TItem> Combine<TQuery, TItem>(IEnumerable<SearchProvider<TQuery, TItem>> searchProviders)
    {
        ArgumentNullException.ThrowIfNull(searchProviders);

        return new CombinedSearchProvider<TQuery, TItem>(searchProviders.ToArray());
    }

    /// <inheritdoc cref="Combine{TQuery, TItem}(IEnumerable{SearchProvider{TQuery, TItem}})"/>
    public static SearchProvider<TQuery, TItem> Combine<TQuery, TItem>(IEnumerable<ISearchProvider<TQuery, TItem>> searchProviders)
    {
        ArgumentNullException.ThrowIfNull(searchProviders);

        IEnumerable<SearchProvider<TQuery, TItem>> castedSearchProviders = searchProviders
            .Select(x => x is SearchProvider<TQuery, TItem> castedSearchProvider
                ? castedSearchProvider : new SearchProviderWrapper<TQuery, TItem>(x));

        return new CombinedSearchProvider<TQuery, TItem>(castedSearchProviders.ToArray());
    }

    /// <summary>
    /// Creates a new search provider using the specified search function.
    /// </summary>
    /// <typeparam name="TQuery">The type of the search query.</typeparam>
    /// <typeparam name="TItem">The type of the items being searched.</typeparam>
    /// <param name="searchProvider">The search function used to execute queries.</param>
    /// <returns>The new search provider.</returns>
    public static SearchProvider<TQuery, TItem> Create<TQuery, TItem>(Func<TQuery, SearchOptions, SearchResult<TItem>> searchProvider)
    {
        ArgumentNullException.ThrowIfNull(searchProvider);

        return new DelegateSearchProvider<TQuery, TItem>(searchProvider);
    }

    /// <summary>
    /// Creates a search provider based on multiple <see href="https://en.wikipedia.org/wiki/Inverted_index">inverted indices</see>,
    /// which are created from tokens extracted from the names of the specified <paramref name="items"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the items being indexed.</typeparam>
    /// <param name="items">The collection of items to be indexed.</param>
    /// <param name="nameSelectors">The collection of functions that select a name from an item.</param>
    /// <param name="distanceMetric">
    /// The distance metric used in spell checking.
    /// <para/>
    /// By default, <see cref="DistanceMetric.Levenshtein"/> is used.
    /// </param>
    /// <param name="tokenizer">
    /// The tokenizer used to extract words from the names.
    /// <para/>
    /// By default, <see cref="Tokenizer.Words"/> is used.
    /// </param>
    /// <returns>A combined search provider consisting of search providers created for each name selector.</returns>
    public static SearchProvider<string, TItem> Create<TItem>(
        IEnumerable<TItem> items,
        IEnumerable<Func<TItem, string>> nameSelectors,
        IDistanceMetric<string, int>? distanceMetric = null,
        ITokenizer<string, string>? tokenizer = null
    )
        where TItem : notnull
    {
        ArgumentNullException.ThrowIfNull(nameSelectors);

        distanceMetric ??= DistanceMetric.Levenshtein;
        tokenizer ??= Tokenizer.Words;
        ISpellChecker<string> CreateSpellChecker(IReadOnlyCollection<string> words) => SpellChecker.Create(words, distanceMetric);
        static IComparer<string> CreateNameComparer(string name) => new RelativeStringLengthComparer(name);

        IEnumerable<SearchProvider<string, TItem>> searchProviders = nameSelectors
            .Select(nameSelector => Create(items, nameSelector, tokenizer, CreateSpellChecker, distanceMetric, distanceMetric, CreateNameComparer));

        return new CombinedSearchProvider<string, TItem>(searchProviders.ToArray());
    }

    /// <summary>
    /// Creates a search provider based on the <see href="https://en.wikipedia.org/wiki/Inverted_index">inverted index</see>,
    /// which is created from tokens extracted from the names of the specified <paramref name="items"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the items being indexed.</typeparam>
    /// <param name="items">The collection of items to be indexed.</param>
    /// <param name="nameSelector">The function that selects a name from an item.</param>
    /// <param name="distanceMetric">
    /// The distance metric used in spell checking.
    /// <para/>
    /// By default, <see cref="DistanceMetric.Levenshtein"/> is used.
    /// </param>
    /// <param name="tokenizer">
    /// The tokenizer used to extract words from the names.
    /// <para/>
    /// By default, <see cref="Tokenizer.Words"/> is used.
    /// </param>
    /// <returns>The new search provider.</returns>
    public static SearchProvider<string, TItem> Create<TItem>(
        IEnumerable<TItem> items,
        Func<TItem, string> nameSelector,
        IDistanceMetric<string, int>? distanceMetric = null,
        ITokenizer<string, string>? tokenizer = null
    )
        where TItem : notnull
    {
        distanceMetric ??= DistanceMetric.Levenshtein;
        tokenizer ??= Tokenizer.Words;
        ISpellChecker<string> CreateSpellChecker(IReadOnlyCollection<string> words) => SpellChecker.Create(words, distanceMetric);
        static IComparer<string> CreateNameComparer(string name) => new RelativeStringLengthComparer(name);

        return Create(items, nameSelector, tokenizer, CreateSpellChecker, distanceMetric, distanceMetric, CreateNameComparer);
    }

    /// <summary>
    /// Creates a search provider based on the <see href="https://en.wikipedia.org/wiki/Inverted_index">inverted index</see>,
    /// which is created from tokens extracted from the names of the specified <paramref name="items"/>.
    /// </summary>
    /// <typeparam name="TName">The type of the name used for indexing.</typeparam>
    /// <typeparam name="TItem">The type of items being indexed.</typeparam>
    /// <param name="items">The collection of items to be indexed.</param>
    /// <param name="nameSelector">The function that selects a name from an item.</param>
    /// <param name="tokenizer">
    /// The tokenizer used to extract words from the names.
    /// <para/>
    /// By default, the whole item name is considered as a single token.
    /// </param>
    /// <param name="spellCheckerFactory">
    /// The factory function that creates a spell checker for the extracted words.
    /// </param>
    /// <param name="nameComparer">The equality comparer for comparing names.</param>
    /// <param name="nameComparisonFactory">
    /// The factory function that creates a comparer for comparing names relative to the name being searched.
    /// </param>
    /// <returns>The new search provider.</returns>
    public static SearchProvider<TName, TItem> Create<TName, TItem>(
        IEnumerable<TItem> items,
        Func<TItem, TName> nameSelector,
        ITokenizer<TName, TName>? tokenizer = null,
        Func<IReadOnlyCollection<TName>, ISpellChecker<TName>>? spellCheckerFactory = null,
        IEqualityComparer<TName>? nameComparer = null,
        Func<TName, Comparison<TName>>? nameComparisonFactory = null
    )
        where TName : notnull
        where TItem : notnull
    {
        tokenizer ??= Tokenizer.Create<TName, TName>(static x => [x]);
        Func<TName, IComparer<TName>>? nameComparerFactory = nameComparisonFactory is null ? null : name => Comparer<TName>.Create(nameComparisonFactory(name));

        return Create(items, nameSelector, tokenizer, spellCheckerFactory, nameComparer, nameComparer, nameComparerFactory);
    }

    /// <summary>
    /// Creates a search provider based on the <see href="https://en.wikipedia.org/wiki/Inverted_index">inverted index</see>,
    /// which is created from tokens extracted from the names of the specified <paramref name="items"/>.
    /// </summary>
    /// <typeparam name="TName">The type of the name used for indexing.</typeparam>
    /// <typeparam name="TItem">The type of items being indexed.</typeparam>
    /// <typeparam name="TWord">The type of words extracted from the names.</typeparam>
    /// <param name="items">The collection of items to be indexed.</param>
    /// <param name="nameSelector">The function that selects a name from an item.</param>
    /// <param name="tokenizer">The tokenizer used to extract words from the names.</param>
    /// <param name="spellChecker">The spell checker for the extracted words.</param>
    /// <param name="nameComparer">The equality comparer for comparing names.</param>
    /// <param name="wordComparer">The equality comparer for comparing words.</param>
    /// <param name="boundNameComparer">
    /// The comparer for comparing names relative to the name being searched.
    /// </param>
    /// <returns>The new search provider.</returns>
    public static SearchProvider<TName, TItem> Create<TName, TItem, TWord>(
        IEnumerable<TItem> items,
        Func<TItem, TName> nameSelector,
        ITokenizer<TName, TWord> tokenizer,
        ISpellChecker<TWord>? spellChecker = null,
        IEqualityComparer<TName>? nameComparer = null,
        IEqualityComparer<TWord>? wordComparer = null,
        IComparer<TName>? boundNameComparer = null
    )
        where TItem : notnull
        where TWord : notnull
    {
        Func<IReadOnlyCollection<TWord>, ISpellChecker<TWord>>? spellCheckerFactory = spellChecker is null ? null : _ => spellChecker;
        Func<TName, IComparer<TName>>? nameComparerFactory = boundNameComparer is null ? null : _ => boundNameComparer;

        return Create(items, nameSelector, tokenizer, spellCheckerFactory, nameComparer, wordComparer, nameComparerFactory);
    }

    /// <summary>
    /// Creates a search provider based on the <see href="https://en.wikipedia.org/wiki/Inverted_index">inverted index</see>,
    /// which is created from tokens extracted from the names of the specified <paramref name="items"/>.
    /// </summary>
    /// <typeparam name="TName">The type of the name used for indexing.</typeparam>
    /// <typeparam name="TItem">The type of items being indexed.</typeparam>
    /// <typeparam name="TWord">The type of words extracted from the names.</typeparam>
    /// <param name="items">The collection of items to be indexed.</param>
    /// <param name="nameSelector">A function that selects a name from an item.</param>
    /// <param name="tokenizer">The tokenizer used to extract words from the names.</param>
    /// <param name="spellCheckerFactory">
    /// A factory function that creates a spell checker for the extracted words.
    /// </param>
    /// <param name="nameComparer">An equality comparer for comparing names.</param>
    /// <param name="wordComparer">An equality comparer for comparing words.</param>
    /// <param name="nameComparerFactory">
    /// A factory function that creates a comparer for comparing names relative to the name being searched.
    /// </param>
    /// <returns>The new search provider.</returns>
    public static SearchProvider<TName, TItem> Create<TName, TItem, TWord>(
        IEnumerable<TItem> items,
        Func<TItem, TName> nameSelector,
        ITokenizer<TName, TWord> tokenizer,
        Func<IReadOnlyCollection<TWord>, ISpellChecker<TWord>>? spellCheckerFactory = null,
        IEqualityComparer<TName>? nameComparer = null,
        IEqualityComparer<TWord>? wordComparer = null,
        Func<TName, IComparer<TName>>? nameComparerFactory = null
    )
        where TItem : notnull
        where TWord : notnull
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(nameSelector);
        ArgumentNullException.ThrowIfNull(tokenizer);
        nameComparer ??= EqualityComparer<TName>.Default;
        nameComparerFactory ??= static _ => Comparer<TName>.Default;

        Dictionary<TWord, HashSet<TItem>> invIndexBuilder = new(wordComparer);
        foreach (TItem item in items)
        {
            TName name = nameSelector(item);
            IEnumerable<TWord> words = tokenizer.Tokenize(name);

            foreach (TWord word in words)
            {
                HashSet<TItem> itemsByWord = invIndexBuilder.TryGetValue(word, out HashSet<TItem>? x) ? x : (invIndexBuilder[word] = new());
                itemsByWord.Add(item);
            }
        }

        FrozenDictionary<TWord, TItem[]> invIndex = invIndexBuilder.ToFrozenDictionary(static x => x.Key, static x => x.Value.ToArray(), wordComparer);
        ISpellChecker<TWord> spellChecker = spellCheckerFactory?.Invoke(invIndex.Keys) ?? SpellChecker<TWord>.Null;

        return new InvertedIndexSearchProvider<TItem, TName, TWord>(invIndex, nameSelector, nameComparer, nameComparerFactory, tokenizer, spellChecker);
    }
}

/// <summary>
/// Represents a generic search provider that allows searching for items based on a specified query.
/// </summary>
/// <typeparam name="TQuery">The type of the search query.</typeparam>
/// <typeparam name="TItem">The type of the items being searched.</typeparam>
public abstract class SearchProvider<TQuery, TItem> : ISearchProvider<TQuery, TItem>
{
    ISearchResult<TItem> ISearchProvider<TQuery, TItem>.Search(TQuery query, SearchOptions options)
        => Search(query, options);

    /// <summary>
    /// Searches for an item based on the specified query.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="options">The search options.</param>
    /// <returns>An instance of <see cref="SearchResult{TItem}"/> containing the search result and suggestions, if any.</returns>
    public abstract SearchResult<TItem> Search(TQuery query, SearchOptions options = default);

    ISearchResult<TItem> ISearchProvider<TQuery, TItem>.SearchLast(TQuery query, SearchOptions options)
        => SearchLast(query, options);

    /// <summary>
    /// Searches for the last occurrence of an item based on the specified query.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="options">The search options.</param>
    /// <returns>An instance of <see cref="SearchResult{TItem}"/> containing the search result and suggestions, if any.</returns>
    public virtual SearchResult<TItem> SearchLast(TQuery query, SearchOptions options = default)
        => Search(query, options);

    async ValueTask<ISearchResult<TItem>> ISearchProvider<TQuery, TItem>.SearchAsync(TQuery query, SearchOptions options, CancellationToken cancellationToken)
    {
        SearchResult<TItem> result = await SearchAsync(query, options, cancellationToken).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Asynchronously searches for an item based on the specified query.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="options">The search options.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An instance of <see cref="SearchResult{TItem}"/> containing the search result and suggestions, if any.</returns>
    public virtual ValueTask<SearchResult<TItem>> SearchAsync(TQuery query, SearchOptions options = default, CancellationToken cancellationToken = default)
        => new(Search(query, options));

    async ValueTask<ISearchResult<TItem>> ISearchProvider<TQuery, TItem>.SearchLastAsync(TQuery query, SearchOptions options, CancellationToken cancellationToken)
    {
        SearchResult<TItem> result = await SearchLastAsync(query, options, cancellationToken).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Asynchronously searches for the last occurrence of an item based on the specified query.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="options">The search options.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An instance of <see cref="SearchResult{TItem}"/> containing the search result and suggestions, if any.</returns>
    public virtual ValueTask<SearchResult<TItem>> SearchLastAsync(TQuery query, SearchOptions options = default, CancellationToken cancellationToken = default)
        => SearchAsync(query, options, cancellationToken);
}

internal sealed class DelegateSearchProvider<TQuery, TItem> : SearchProvider<TQuery, TItem>
{
    private readonly Func<TQuery, SearchOptions, SearchResult<TItem>> _delegate;

    internal DelegateSearchProvider(Func<TQuery, SearchOptions, SearchResult<TItem>> delegateSearchProvider)
    {
        _delegate = delegateSearchProvider;
    }

    public override SearchResult<TItem> Search(TQuery query, SearchOptions options = default)
        => _delegate(query, options);

    public override bool Equals(object? obj)
        => obj is DelegateSearchProvider<TQuery, TItem> other && _delegate == other._delegate;

    public override int GetHashCode()
        => _delegate.GetHashCode();
}

internal sealed class CombinedSearchProvider<TQuery, TItem> : SearchProvider<TQuery, TItem>
{
    private readonly SearchProvider<TQuery, TItem>[] _searchProviders;

    internal CombinedSearchProvider(SearchProvider<TQuery, TItem>[] searchProviders)
    {
        _searchProviders = searchProviders;
    }

    public override SearchResult<TItem> Search(TQuery query, SearchOptions options = default)
    {
        SearchProvider<TQuery, TItem>[] searchProviders = _searchProviders;
        IEnumerable<SearchSuggestion<TItem>> suggestions = Enumerable.Empty<SearchSuggestion<TItem>>();
        int maxSuggestions = options.MaxSuggestions < 0 ? int.MaxValue : options.MaxSuggestions;

        for (int i = 0; i < searchProviders.Length; i++)
        {
            SearchResult<TItem> result = searchProviders[i].Search(query, options);
            suggestions = suggestions.Concat(result.Suggestions);

            if (result.Success)
                return new(true, result.Value, suggestions.OrderBy(static x => x.Rank).Take(maxSuggestions));
        }

        return new(false, default, suggestions.OrderBy(static x => x.Rank).Take(maxSuggestions));
    }

    public override SearchResult<TItem> SearchLast(TQuery query, SearchOptions options = default)
    {
        SearchProvider<TQuery, TItem>[] searchProviders = _searchProviders;
        IEnumerable<SearchSuggestion<TItem>> suggestions = Enumerable.Empty<SearchSuggestion<TItem>>();
        int maxSuggestions = options.MaxSuggestions < 0 ? int.MaxValue : options.MaxSuggestions;

        for (int i = 0; i < searchProviders.Length; i++)
        {
            SearchResult<TItem> result = searchProviders[i].SearchLast(query, options);
            suggestions = suggestions.Concat(result.Suggestions);

            if (result.Success)
                return new(true, result.Value, suggestions.OrderBy(static x => x.Rank).Take(maxSuggestions));
        }

        return new(false, default, suggestions.OrderBy(static x => x.Rank).Take(maxSuggestions));
    }

    public override async ValueTask<SearchResult<TItem>> SearchAsync(TQuery query, SearchOptions options = default, CancellationToken cancellationToken = default)
    {
        SearchProvider<TQuery, TItem>[] searchProviders = _searchProviders;
        IEnumerable<SearchSuggestion<TItem>> suggestions = Enumerable.Empty<SearchSuggestion<TItem>>();
        int maxSuggestions = options.MaxSuggestions < 0 ? int.MaxValue : options.MaxSuggestions;

        for (int i = 0; i < searchProviders.Length; i++)
        {
            SearchResult<TItem> result = await searchProviders[i].SearchAsync(query, options, cancellationToken).ConfigureAwait(false);
            suggestions = suggestions.Concat(result.Suggestions);

            if (result.Success)
                return new(true, result.Value, suggestions.OrderBy(static x => x.Rank).Take(maxSuggestions));
        }

        return new(false, default, suggestions.OrderBy(static x => x.Rank).Take(maxSuggestions));
    }

    public override async ValueTask<SearchResult<TItem>> SearchLastAsync(TQuery query, SearchOptions options = default, CancellationToken cancellationToken = default)
    {
        SearchProvider<TQuery, TItem>[] searchProviders = _searchProviders;
        int maxSuggestions = options.MaxSuggestions < 0 ? int.MaxValue : options.MaxSuggestions;

        bool success = false;
        TItem? value = default;
        IEnumerable<SearchSuggestion<TItem>> suggestions = Enumerable.Empty<SearchSuggestion<TItem>>();

        for (int i = 0; i < searchProviders.Length; i++)
        {
            SearchResult<TItem> result = await searchProviders[i].SearchLastAsync(query, options, cancellationToken).ConfigureAwait(false);
            suggestions = suggestions.Concat(result.Suggestions);
            if (!result.Success)
                continue;

            success = true;
            value = result.Value;
            break;
        }

        return new(success, value, suggestions.OrderBy(static x => x.Rank).Take(maxSuggestions));
    }
}

internal sealed class SearchProviderWrapper<TQuery, TItem> : SearchProvider<TQuery, TItem>
{
    private readonly ISearchProvider<TQuery, TItem> _searchProvider;

    internal SearchProviderWrapper(ISearchProvider<TQuery, TItem> searchProvider)
    {
        _searchProvider = searchProvider;
    }

    public override SearchResult<TItem> Search(TQuery query, SearchOptions options = default)
    {
        ISearchResult<TItem> result = _searchProvider.Search(query, options);
        return new(result.Success, result.Value, result.Suggestions);
    }

    public override SearchResult<TItem> SearchLast(TQuery query, SearchOptions options = default)
    {
        ISearchResult<TItem> result = _searchProvider.SearchLast(query, options);
        return new(result.Success, result.Value, result.Suggestions);
    }

    public override async ValueTask<SearchResult<TItem>> SearchAsync(TQuery query, SearchOptions options = default, CancellationToken cancellationToken = default)
    {
        ISearchResult<TItem> result = await _searchProvider.SearchAsync(query, options, cancellationToken).ConfigureAwait(false);
        return new(result.Success, result.Value, result.Suggestions);
    }

    public override async ValueTask<SearchResult<TItem>> SearchLastAsync(TQuery query, SearchOptions options = default, CancellationToken cancellationToken = default)
    {
        ISearchResult<TItem> result = await _searchProvider.SearchLastAsync(query, options, cancellationToken).ConfigureAwait(false);
        return new(result.Success, result.Value, result.Suggestions);
    }

    public override bool Equals(object? obj) => _searchProvider.Equals(obj);

    public override int GetHashCode() => _searchProvider.GetHashCode();

    public override string? ToString() => _searchProvider.ToString();
}

internal sealed class InvertedIndexSearchProvider<TItem, TName, TWord> : SearchProvider<TName, TItem>
    where TItem : notnull
    where TWord : notnull
{
    private readonly FrozenDictionary<TWord, TItem[]> _items;

    private readonly Func<TItem, TName> _nameSelector;

    private readonly IEqualityComparer<TName> _nameComparer;

    private readonly Func<TName, IComparer<TName>> _nameComparerFactory;

    private readonly ITokenizer<TName, TWord> _tokenizer;

    private readonly ISpellChecker<TWord> _spellChecker;

    internal InvertedIndexSearchProvider(
        FrozenDictionary<TWord, TItem[]> items,
        Func<TItem, TName> nameSelector,
        IEqualityComparer<TName> nameComparer,
        Func<TName, IComparer<TName>> nameComparerFactory,
        ITokenizer<TName, TWord> tokenizer,
        ISpellChecker<TWord> spellChecker)
    {
        _items = items;
        _nameSelector = nameSelector;
        _nameComparer = nameComparer;
        _tokenizer = tokenizer;
        _spellChecker = spellChecker;
        _nameComparerFactory = nameComparerFactory;
    }

    public override SearchResult<TItem> Search(TName query, SearchOptions options = default)
    {
        int maxSuggestions = options.MaxSuggestions < 0 ? int.MaxValue : options.MaxSuggestions;
        int maxCandidates = (int)Math.Min(maxSuggestions + 1L, int.MaxValue);
        FrozenDictionary<TWord, TItem[]> items = _items;
        Func<TItem, TName> nameSelector = _nameSelector;
        IComparer<TName> boundNameComparer = _nameComparerFactory(query);
        Dictionary<TItem, int> candidates = new();
        IEnumerable<TWord> words = _tokenizer.Tokenize(query).Select(x => _spellChecker.TryFixSpelling(x, out TWord? y) ? y : x);
        int wordCount = 0;

        foreach (TWord word in words)
        {
            wordCount++;

            if (!items.TryGetValue(word, out TItem[]? itemBucket))
                continue;

            foreach (TItem item in itemBucket)
            {
                int encountered = candidates.TryGetValue(item, out int previouslyEncountered) ? previouslyEncountered : 0;
                candidates[item] = encountered + 1;
            }
        }

        bool hasResult = false;
        TItem? result = default;
        int suggestionCount = Math.Min(candidates.Count, maxSuggestions);
        List<SearchSuggestion<TItem>>? suggestions = suggestionCount <= 0 ? null : new(suggestionCount);
        IEnumerable<SearchSuggestion<TItem>> results = candidates
            .Select(x => new SearchSuggestion<TItem>(x.Key, 1f - (float)x.Value / wordCount))
            .OrderBy(static x => x.Rank)
            .ThenBy(x => nameSelector(x.Value), boundNameComparer);

        foreach (SearchSuggestion<TItem> suggestion in results)
        {
            if (!hasResult && suggestion.Rank == 0f && _nameComparer.Equals(nameSelector(suggestion.Value), query))
            {
                hasResult = true;
                result = suggestion.Value;
            }
            else if (suggestionCount > 0)
            {
                suggestions!.Add(suggestion);
                suggestionCount--;
            }
            else
            {
                break;
            }
        }

        return new SearchResult<TItem>(hasResult, result, suggestions);
    }
}
