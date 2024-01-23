using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Searchify.Comparison;

/// <summary>
/// Represents a default comparer for the type specified by the generic argument.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
[EditorBrowsable(EditorBrowsableState.Never)]
public readonly struct DefaultComparer<T> : IComparer<T>, IEqualityComparer<T>, IComparer, IEqualityComparer
{
    /// <inheritdoc/>
    public int Compare(T? x, T? y) => Comparer<T>.Default.Compare(x, y);

    /// <inheritdoc/>
    public bool Equals(T? x, T? y) => EqualityComparer<T>.Default.Equals(x, y);

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] T obj) => EqualityComparer<T>.Default.GetHashCode(obj);

    int IComparer.Compare(object? x, object? y) => ((IComparer)Comparer<T>.Default).Compare(x, y);

    bool IEqualityComparer.Equals(object? x, object? y) => ((IEqualityComparer)EqualityComparer<T>.Default).Equals(x, y);

    int IEqualityComparer.GetHashCode([DisallowNull] object obj) => obj.GetHashCode();
}
