using System.Text.RegularExpressions;

namespace Searchify.Processing;

/// <summary>
/// Provides methods for creating instances of the <see cref="Tokenizer{TInput, TToken}"/> class.
/// </summary>
public static partial class Tokenizer
{
    [GeneratedRegex(@"\W+")]
    private static partial Regex GetWordsRegex();

    /// <summary>
    /// Gets the tokenizer for splitting input strings into tokens based on non-word characters.
    /// </summary>
    public static Tokenizer<string, string> Words { get; } = new RegexTokenizer(GetWordsRegex());

    /// <summary>
    /// Creates a new tokenizer using the specified tokenizing function.
    /// </summary>
    /// <typeparam name="TInput">The type of input to be tokenized.</typeparam>
    /// <typeparam name="TToken">The type of tokens generated during the tokenization process.</typeparam>
    /// <param name="tokenizer">The tokenizing function used to break the input into tokens.</param>
    /// <returns>The new tokenizer.</returns>
    public static Tokenizer<TInput, TToken> Create<TInput, TToken>(Func<TInput, IEnumerable<TToken>> tokenizer)
    {
        ArgumentNullException.ThrowIfNull(tokenizer);

        return new DelegateTokenizer<TInput, TToken>(tokenizer);
    }

    /// <summary>
    /// Creates a new tokenizer using the specified regular expression.
    /// </summary>
    /// <param name="regex">The regular expression used to split the input string into tokens.</param>
    /// <returns>The new tokenizer.</returns>
    public static Tokenizer<string, string> Create(Regex regex)
    {
        ArgumentNullException.ThrowIfNull(regex);

        return new RegexTokenizer(regex);
    }
}

/// <summary>
/// Defines the ability to tokenize input into a sequence of tokens.
/// </summary>
/// <typeparam name="TInput">The type of input to be tokenized.</typeparam>
/// <typeparam name="TToken">The type of tokens generated during the tokenization process.</typeparam>
public abstract class Tokenizer<TInput, TToken> : ITokenizer<TInput, TToken>
{
    /// <inheritdoc/>
    public abstract IEnumerable<TToken> Tokenize(TInput input);
}

internal sealed class DelegateTokenizer<TInput, TToken> : Tokenizer<TInput, TToken>
{
    private readonly Func<TInput, IEnumerable<TToken>> _delegate;

    public DelegateTokenizer(Func<TInput, IEnumerable<TToken>> delegateTokenizer)
    {
        _delegate = delegateTokenizer;
    }

    public override IEnumerable<TToken> Tokenize(TInput input)
        => _delegate(input);

    public override bool Equals(object? obj)
        => obj is DelegateTokenizer<TInput, TToken> other && _delegate == other._delegate;

    public override int GetHashCode()
        => _delegate.GetHashCode();
}

internal sealed class RegexTokenizer : Tokenizer<string, string>
{
    private readonly Regex _regex;

    public RegexTokenizer(Regex regex)
    {
        _regex = regex;
    }

    public override IEnumerable<string> Tokenize(string input)
        => _regex.Split(input).Where(static x => x.Length != 0);

    public override bool Equals(object? obj)
        => obj is RegexTokenizer other && _regex == other._regex;

    public override int GetHashCode()
        => _regex.GetHashCode();
}
