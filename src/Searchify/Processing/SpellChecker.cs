using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Searchify.Collections.Generic;
using Searchify.Metrics;

namespace Searchify.Processing;

/// <summary>
/// Provides methods for creating instances of the <see cref="SpellChecker{TWord}"/> class.
/// </summary>
public static class SpellChecker
{
    /// <summary>
    /// Creates a new spell checker using the specified spell checking function.
    /// </summary>
    /// <typeparam name="TWord">The type of words to be checked and corrected.</typeparam>
    /// <param name="spellChecker">The function that checks the spelling of the provided word.</param>
    /// <returns>The new spell checker.</returns>
    public static SpellChecker<TWord> Create<TWord>(Func<TWord, SpellCheckResult<TWord>> spellChecker)
    {
        ArgumentNullException.ThrowIfNull(spellChecker);

        return new DelegateSpellChecker<TWord>(spellChecker);
    }

    /// <summary>
    /// Creates a new spell checker using the specified vocabulary.
    /// </summary>
    /// <typeparam name="TWord">The type of words to be checked and corrected.</typeparam>
    /// <typeparam name="TDistance">The type representing the calculated distance.</typeparam>
    /// <param name="vocabulary">The collection of words to build the spell checker from.</param>
    /// <param name="distanceMetric">The distance metric used to calculate the similarity between words.</param>
    /// <param name="maxDistanceMetric">The maximum distance metric used to determine the maximum allowed distance between words.</param>
    /// <param name="maxSuggestions">The maximum number of suggestions to return. Use <c>-1</c> to return all suggestions.</param>
    /// <returns>The new spell checker.</returns>
    public static SpellChecker<TWord> Create<TWord, TDistance>(
        IEnumerable<TWord> vocabulary,
        IDistanceMetric<TWord, TDistance> distanceMetric,
        IMaxDistanceMetric<TWord, TDistance> maxDistanceMetric,
        int maxSuggestions = -1
    )
        where TDistance : notnull, INumber<TDistance>, IMinMaxValue<TDistance>
    {
        ArgumentNullException.ThrowIfNull(vocabulary);
        ArgumentNullException.ThrowIfNull(distanceMetric);
        ArgumentNullException.ThrowIfNull(maxDistanceMetric);

        return new BKTreeSpellChecker<TWord, TDistance, IMaxDistanceMetric<TWord, TDistance>>(vocabulary, distanceMetric, maxDistanceMetric, maxSuggestions);
    }

    /// <inheritdoc cref="Create{TWord, TDistance}(IEnumerable{TWord}, IDistanceMetric{TWord, TDistance}, IMaxDistanceMetric{TWord, TDistance}, int)"/>
    public static SpellChecker<TWord> Create<TWord, TDistance>(
        IEnumerable<TWord> vocabulary,
        IDistanceMetric<TWord, TDistance> distanceMetric,
        Func<TWord, TDistance> maxDistanceMetric,
        int maxSuggestions = -1
    ) where TDistance : notnull, INumber<TDistance>, IMinMaxValue<TDistance>
        => Create(vocabulary, distanceMetric, MaxDistanceMetric.Create(maxDistanceMetric), maxSuggestions);

    /// <summary>
    /// Creates a new spell checker using the specified vocabulary.
    /// </summary>
    /// <typeparam name="TDistance">The type representing the calculated distance.</typeparam>
    /// <param name="vocabulary">The collection of words to build the spell checker from.</param>
    /// <param name="distanceMetric">The distance metric used to calculate the similarity between words.</param>
    /// <param name="maxDistanceRatio">
    /// The maximum allowed ratio of the calculated distance between two words to be considered a match.
    /// <para/>
    /// A value between <c>0</c> and <c>1</c>, where
    /// <c>0</c> means that only exact matches are considered, and
    /// <c>1</c> means that all words are considered as matches.
    /// </param>
    /// <param name="maxSuggestions">The maximum number of suggestions to return. Use <c>-1</c> to return all suggestions.</param>
    /// <returns>The new spell checker.</returns>
    public static SpellChecker<string> Create<TDistance>(
        IEnumerable<string> vocabulary,
        IDistanceMetric<string, TDistance> distanceMetric,
        float maxDistanceRatio = 0.25f,
        int maxSuggestions = -1
    )
        where TDistance : notnull, INumber<TDistance>, IMinMaxValue<TDistance>
    {
        ArgumentNullException.ThrowIfNull(vocabulary);
        ArgumentNullException.ThrowIfNull(distanceMetric);

        return new BKTreeSpellChecker<string, TDistance, MaxStringDistanceRatioMetric<TDistance>>(vocabulary, distanceMetric, new(maxDistanceRatio), maxSuggestions);
    }
}

/// <summary>
/// Provides functionality to check and potentially fix the spelling of words.
/// </summary>
/// <typeparam name="TWord">The type of words to be checked and corrected.</typeparam>
public abstract class SpellChecker<TWord> : ISpellChecker<TWord>
{
    /// <summary>
    /// Gets a spell checker instance that always considers words as correctly spelled.
    /// </summary>
    public static SpellChecker<TWord> Null { get; } = new DelegateSpellChecker<TWord>(static _ => new(isCorrect: true));

    ISpellCheckResult<TWord> ISpellChecker<TWord>.CheckSpelling(TWord word) => CheckSpelling(word);

    /// <summary>
    /// Checks the spelling of the provided word and returns a spell check result.
    /// </summary>
    /// <param name="word">The word to be spell-checked.</param>
    /// <returns>An instance of <see cref="SpellCheckResult{TWord}"/> containing the result of the spell check.</returns>
    public abstract SpellCheckResult<TWord> CheckSpelling(TWord word);

    /// <inheritdoc/>
    public virtual bool TryFixSpelling(TWord word, [MaybeNullWhen(false)] out TWord spellCheckedWord)
    {
        SpellCheckResult<TWord> result = CheckSpelling(word);
        if (result.IsCorrect)
        {
            spellCheckedWord = word;
            return true;
        }

        foreach (TWord suggestion in result.Suggestions)
        {
            spellCheckedWord = suggestion;
            return true;
        }

        spellCheckedWord = default;
        return false;
    }
}

internal sealed class DelegateSpellChecker<TWord> : SpellChecker<TWord>
{
    private readonly Func<TWord, SpellCheckResult<TWord>> _delegate;

    public DelegateSpellChecker(Func<TWord, SpellCheckResult<TWord>> delegateSpellChecker)
    {
        _delegate = delegateSpellChecker;
    }

    public override SpellCheckResult<TWord> CheckSpelling(TWord word)
        => _delegate(word);

    public override bool Equals(object? obj)
        => obj is DelegateSpellChecker<TWord> other && _delegate == other._delegate;

    public override int GetHashCode()
        => _delegate.GetHashCode();
}

internal sealed class BKTreeSpellChecker<TWord, TDistance, TMaxDistance> : SpellChecker<TWord>
    where TDistance : notnull, INumber<TDistance>, IMinMaxValue<TDistance>
    where TMaxDistance : IMaxDistanceMetric<TWord, TDistance>
{
    private readonly FrozenSet<TWord> _vocabulary;

    private readonly BKTree<TWord, TDistance> _tree;

    private readonly TMaxDistance _maxDistanceMetric;

    private readonly int _maxSuggestions;

    public BKTreeSpellChecker(
        IEnumerable<TWord> vocabulary,
        IDistanceMetric<TWord, TDistance> distanceMetric,
        TMaxDistance maxDistanceMetric,
        int maxSuggestions)
    {
        _vocabulary = vocabulary.ToFrozenSet(distanceMetric);
        _tree = new(distanceMetric);
        _tree.AddRange(_vocabulary);
        _maxDistanceMetric = maxDistanceMetric;
        _maxSuggestions = maxSuggestions < 0 ? int.MaxValue : maxSuggestions;
    }

    public ISet<TWord> Vocabulary => _vocabulary;

    public override SpellCheckResult<TWord> CheckSpelling(TWord word)
    {
        if (_vocabulary.Contains(word))
            return new(isCorrect: true);

        if (_maxSuggestions == 0)
            return new(isCorrect: false);

        TDistance maxDistance = _maxDistanceMetric.Calculate(word);
        IEnumerable<TWord>? suggestions = _tree.FindAll(word, maxDistance, _maxSuggestions).Select(static x => x.Value);
        return new(isCorrect: false, suggestions);
    }

    public override bool TryFixSpelling(TWord word, [MaybeNullWhen(false)] out TWord spellCheckedWord)
    {
        if (_vocabulary.Contains(word))
        {
            spellCheckedWord = word;
            return true;
        }

        TDistance maxDistance = _maxDistanceMetric.Calculate(word);
        BKTreeMatch<TWord, TDistance>? suggestion = _tree.Find(word, maxDistance);
        if (suggestion is not null)
        {
            spellCheckedWord = suggestion.Value.Value;
            return true;
        }

        spellCheckedWord = default;
        return false;
    }
}
