namespace Searchify.Processing;

/// <summary>
/// Represents the result of a spell check operation.
/// </summary>
/// <typeparam name="TWord">The type of word being spell-checked.</typeparam>
public interface ISpellCheckResult<TWord>
{
    /// <summary>
    /// Indicates whether the word is correct.
    /// </summary>
    bool IsCorrect { get; }

    /// <summary>
    /// A collection of suggested corrections for the word.
    /// </summary>
    IEnumerable<TWord> Suggestions { get; }
}
