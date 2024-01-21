using System.Collections;

namespace Searchify.Comparison;

/// <summary>
/// Represents a base comparer for characters.
/// </summary>
public interface ICharComparer : IComparer<char>, IEqualityComparer<char>, IComparer, IEqualityComparer
{
    /// <summary>
    /// Converts the character comparer to a string comparer.
    /// </summary>
    /// <returns>An instance of <see cref="StringComparer"/> based on this character comparer.</returns>
    StringComparer AsStringComparer();
}
