namespace Searchify.Processing;

/// <summary>
/// Defines the ability to tokenize input into a sequence of tokens.
/// </summary>
/// <typeparam name="TInput">The type of input to be tokenized.</typeparam>
/// <typeparam name="TToken">The type of tokens generated during the tokenization process.</typeparam>
public interface ITokenizer<TInput, TToken>
{
    /// <summary>
    /// Tokenizes the provided input and returns a collection of tokens it comprises.
    /// </summary>
    /// <param name="input">The input to be tokenized.</param>
    /// <returns>The tokens extracted from the input.</returns>
    IEnumerable<TToken> Tokenize(TInput input);
}
