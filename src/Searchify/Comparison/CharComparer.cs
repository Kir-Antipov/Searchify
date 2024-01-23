using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Searchify.Helpers;

namespace Searchify.Comparison;

/// <summary>
/// Provides different character comparison strategies.
/// </summary>
public static class CharComparer
{
    /// <summary>
    /// An <see cref="ICharComparer"/> that performs a case-sensitive ordinal comparison.
    /// </summary>
    public static OrdinalCaseSensitiveCharComparer Ordinal => default;

    /// <summary>
    /// An <see cref="ICharComparer"/> that performs case-insensitive character comparison using the invariant culture.
    /// </summary>
    public static InvariantCultureIgnoreCaseCharComparer InvariantCultureIgnoreCase => default;

    /// <summary>
    /// An <see cref="ICharComparer"/> that performs case-insensitive character comparison using the current culture.
    /// </summary>
    public static CurrentCultureIgnoreCaseCharComparer CurrentCultureIgnoreCase => default;
}

/// <summary>
/// Represents a character comparer that performs case-sensitive ordinal comparison.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public readonly struct OrdinalCaseSensitiveCharComparer : ICharComparer
{
    /// <inheritdoc/>
    public StringComparer AsStringComparer() => StringComparer.Ordinal;

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(char x, char y) => x - y;

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(char x, char y) => x == y;

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(char obj) => obj;

    int IComparer.Compare(object? x, object? y)
    {
        ThrowHelper.ThrowArgumentException_IfNotChar(x);
        ThrowHelper.ThrowArgumentException_IfNotChar(y);

        return (char)x - (char)y;
    }

    bool IEqualityComparer.Equals(object? x, object? y)
    {
        ThrowHelper.ThrowArgumentException_IfNotChar(x);
        ThrowHelper.ThrowArgumentException_IfNotChar(y);

        return (char)x == (char)y;
    }

    int IEqualityComparer.GetHashCode([DisallowNull] object obj)
    {
        ThrowHelper.ThrowArgumentException_IfNotChar(obj);

        return (char)obj;
    }
}

/// <summary>
/// Represents a character comparer that performs case-insensitive character comparison using the invariant culture.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public readonly struct InvariantCultureIgnoreCaseCharComparer : ICharComparer
{
    /// <inheritdoc/>
    public StringComparer AsStringComparer() => StringComparer.InvariantCultureIgnoreCase;

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(char x, char y) => char.ToLowerInvariant(x) - char.ToLowerInvariant(y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(char x, char y) => char.ToLowerInvariant(x) == char.ToLowerInvariant(y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(char obj) => char.ToLowerInvariant(obj);

    int IComparer.Compare(object? x, object? y)
    {
        ThrowHelper.ThrowArgumentException_IfNotChar(x);
        ThrowHelper.ThrowArgumentException_IfNotChar(y);

        return char.ToLowerInvariant((char)x) - char.ToLowerInvariant((char)y);
    }

    bool IEqualityComparer.Equals(object? x, object? y)
    {
        ThrowHelper.ThrowArgumentException_IfNotChar(x);
        ThrowHelper.ThrowArgumentException_IfNotChar(y);

        return char.ToLowerInvariant((char)x) == char.ToLowerInvariant((char)y);
    }

    int IEqualityComparer.GetHashCode([DisallowNull] object obj)
    {
        ThrowHelper.ThrowArgumentException_IfNotChar(obj);

        return char.ToLowerInvariant((char)obj);
    }
}

/// <summary>
/// Represents a character comparer that performs case-insensitive character comparison using the current culture.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public readonly struct CurrentCultureIgnoreCaseCharComparer : ICharComparer
{
    /// <inheritdoc/>
    public StringComparer AsStringComparer() => StringComparer.CurrentCultureIgnoreCase;

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(char x, char y) => char.ToLower(x) - char.ToLower(y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(char x, char y) => char.ToLower(x) == char.ToLower(y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(char obj) => char.ToLower(obj);

    int IComparer.Compare(object? x, object? y)
    {
        ThrowHelper.ThrowArgumentException_IfNotChar(x);
        ThrowHelper.ThrowArgumentException_IfNotChar(y);

        return char.ToLower((char)x) - char.ToLower((char)y);
    }

    bool IEqualityComparer.Equals(object? x, object? y)
    {
        ThrowHelper.ThrowArgumentException_IfNotChar(x);
        ThrowHelper.ThrowArgumentException_IfNotChar(y);

        return char.ToLower((char)x) == char.ToLower((char)y);
    }

    int IEqualityComparer.GetHashCode([DisallowNull] object obj)
    {
        ThrowHelper.ThrowArgumentException_IfNotChar(obj);

        return char.ToLower((char)obj);
    }
}
