using System.Diagnostics;

namespace Searchify.Processing;

/// <summary>
/// Represents the result of a spell check operation.
/// </summary>
/// <typeparam name="TWord">The type of word being spell-checked.</typeparam>
[DebuggerDisplay("IsCorrect = {IsCorrect}")]
public readonly struct SpellCheckResult<TWord> : ISpellCheckResult<TWord>
{
    private readonly bool _isCorrect;

    private readonly IEnumerable<TWord>? _suggestions;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpellCheckResult{TWord}"/> struct.
    /// </summary>
    /// <param name="isCorrect">A value indicating whether the word is spelled correctly.</param>
    public SpellCheckResult(bool isCorrect)
    {
        _isCorrect = isCorrect;
        _suggestions = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpellCheckResult{TWord}"/> struct.
    /// </summary>
    /// <param name="isCorrect">A value indicating whether the word is spelled correctly.</param>
    /// <param name="suggestions">A collection of suggested corrections for the word.</param>
    public SpellCheckResult(bool isCorrect, IEnumerable<TWord>? suggestions)
    {
        _isCorrect = isCorrect;
        _suggestions = suggestions;
    }

    /// <inheritdoc/>
    public bool IsCorrect => _isCorrect;

    /// <inheritdoc/>
    public IEnumerable<TWord> Suggestions => _suggestions ?? Enumerable.Empty<TWord>();
}
