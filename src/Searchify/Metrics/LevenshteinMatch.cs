namespace Searchify.Metrics;

/// <summary>
/// Represents the results from a single Levenshtein distance match.
/// </summary>
/// <typeparam name="TDistance">The numeric type used to represent the costs of deletion, insertion, and substitution.</typeparam>
public readonly struct LevenshteinMatch<TDistance> : IEquatable<LevenshteinMatch<TDistance>>
{
    private readonly int _index;

    private readonly int _length;

    private readonly int _deletions;

    private readonly int _insertions;

    // Currently, this structure consists of six 32-bit fields, with _distance expected to be an int or float in most scenarios.
    // This means it occupies 24 bytes, which might seem a bit chonky for a structure, but actually, it isn't that bad, since
    // it also aligns well with 64-bit platforms. Nonetheless, increasing it further to 28 or even 32 bytes is far from ideal,
    // as the cost of copying such a large structure could potentially leave performance in shambles. Therefore, we cannot
    // afford to add another field, even a single-byte one, to represent the success status. We can, however, fit the success
    // status into an already existing field. Note that _index and _length cannot be negative since they relate to a certain
    // position within a string. Similarly, _deletions, _insertions, and _substitutions cannot go negative as they denote the
    // quantity of operations performed on said string, i.e., their minimum limit would be zero. Thus, each field effectively
    // uses only 31 bits of their formal 32 bits, leaving us with whole five spare bits! Considering the success status can only
    // be `true` or `false`, a single bit is all we need. As _index and _length are expected to be used all the time, and
    // _deletions and _insertions being interchangeable, dependent on whether we want to invert the result of a match (inserting
    // a character in the input sequence is the same operation as deleting one from the pattern, and vice versa),
    // the _substitutions field seems to be the least likely to be used frequently. So, we can store the success status in its
    // most significant bit, which also indicates the sign of the signed 32-bit integer.
    //
    // Consequently, verifying the success status is as simple as checking if the resultant value is less than zero (x < 0).
    // To set the most significant bit, we can use `x | int.MinValue` (the min value has only the most significant bit set), and
    // to obtain the original number of substitutions we can utilize `x & int.MaxValue` (the max value has all other bits set,
    // excluding the most significant one).
    private readonly int _successAndSubstitutions;

    private readonly TDistance _distance;

    internal LevenshteinMatch(int index, int length, int deletions, int insertions, int substitutions, TDistance distance)
    {
        _index = index;
        _length = length;
        _deletions = deletions;
        _insertions = insertions;
        _successAndSubstitutions = substitutions | int.MinValue;
        _distance = distance;
    }

    /// <summary>
    /// Gets an empty (failed) match.
    /// </summary>
    public static LevenshteinMatch<TDistance> Empty => default;

    /// <summary>
    /// The position in the pattern sequence where the match is found.
    /// </summary>
    public int Index => _index;

    /// <summary>
    /// The length of the matching subsequence found in the pattern sequence.
    /// </summary>
    public int Length => _length;

    /// <summary>
    /// The number of deletions from the input sequence.
    /// </summary>
    public int Deletions => _deletions;

    /// <summary>
    /// The number of insertions in the input sequence.
    /// </summary>
    public int Insertions => _insertions;

    /// <summary>
    /// The number of substitutions in the input sequence.
    /// </summary>
    public int Substitutions => _successAndSubstitutions & int.MaxValue;

    /// <summary>
    /// The distance between the input sequence and the matching subsequence found in the pattern sequence.
    /// </summary>
    public TDistance Distance => _distance;

    /// <summary>
    /// Indicates whether the match is successful.
    /// </summary>
    public bool Success => _successAndSubstitutions < 0;

    /// <summary>
    /// Returns the string representation of the current <see cref="LevenshteinMatch{TDistance}"/> object.
    /// </summary>
    /// <returns>
    /// The string representation of the match.
    /// </returns>
    public override string ToString() => Success ? $"{_index}..{_index + _length}" : string.Empty;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(_index, _length);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is LevenshteinMatch<TDistance> match && Equals(match);

    /// <inheritdoc/>
    public bool Equals(LevenshteinMatch<TDistance> other)
        => (_index, _length) == (other._index, other._length)
        && EqualityComparer<TDistance>.Default.Equals(_distance, other._distance)
        && (_deletions, _insertions, _successAndSubstitutions) == (other._deletions, other._insertions, other._successAndSubstitutions);

    /// <summary>
    /// Compares two values to determine equality.
    /// </summary>
    /// <param name="left">The value to compare with <paramref name="right"/>.</param>
    /// <param name="right">The value to compare with <paramref name="left"/>.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator ==(LevenshteinMatch<TDistance> left, LevenshteinMatch<TDistance> right) => left.Equals(right);

    /// <summary>
    /// Compares two values to determine inequality.
    /// </summary>
    /// <param name="left">The value to compare with <paramref name="right"/>.</param>
    /// <param name="right">The value to compare with <paramref name="left"/>.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator !=(LevenshteinMatch<TDistance> left, LevenshteinMatch<TDistance> right) => !left.Equals(right);
}
