using System.Diagnostics.CodeAnalysis;

namespace Searchify.Processing;

/// <summary>
/// Provides functionality to check and potentially fix the spelling of words.
/// </summary>
/// <typeparam name="TWord">The type of words to be checked and corrected.</typeparam>
public interface ISpellChecker<TWord>
{
    /// <summary>
    /// Checks the spelling of the provided word and returns a spell check result.
    /// </summary>
    /// <param name="word">The word to be spell-checked.</param>
    /// <returns>An instance of <see cref="ISpellCheckResult{TWord}"/> containing the result of the spell check.</returns>
    ISpellCheckResult<TWord> CheckSpelling(TWord word);

    /// <summary>
    /// Tries to fix the spelling of the provided word.
    /// </summary>
    /// <param name="word">The word to be spell-checked.</param>
    /// <param name="spellCheckedWord">When successful, contains the corrected version of the word.</param>
    /// <returns>
    /// <c>true</c> if the spelling was successfully corrected (or the word was initially correct);
    /// otherwise, <c>false</c>.
    /// </returns>
    bool TryFixSpelling(TWord word, [MaybeNullWhen(false)] out TWord spellCheckedWord);
}
