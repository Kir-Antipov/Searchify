using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Searchify.Helpers;

/// <summary>
/// Provides helper methods to throw specific exceptions.
/// </summary>
internal static class ThrowHelper
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    [DoesNotReturn]
    public static void ThrowArgumentNullException(string? paramName)
        => throw new ArgumentNullException(paramName);

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> indicating that the capacity of the destination array is insufficient.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    [DoesNotReturn]
    public static void ThrowArgumentException_InsufficientDestinationCapacity()
        => throw new ArgumentException("Destination array was not long enough.");

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> indicating that the parameter is not compatible with the generic comparer.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    [DoesNotReturn]
    public static void ThrowArgumentException_InvalidArgumentForComparison()
        => throw new ArgumentException("Type of argument is not compatible with the generic comparer.");

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> indicating that the parameter is not a character.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    [DoesNotReturn]
    public static void ThrowArgumentException_MustBeOfTypeChar(string? paramName)
        => throw new ArgumentException("Object must be of type Char.", paramName);

    /// <summary>
    /// Verifies if the provided object is of type <see cref="char"/> and throws an exception if not.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the object is not of type Char.</exception>
    public static void ThrowArgumentException_IfNotChar([NotNull] object? obj, [CallerArgumentExpression(nameof(obj))] string? paramName = null)
    {
        if (obj is not char)
            ThrowArgumentException_MustBeOfTypeChar(paramName);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [DoesNotReturn]
    public static void ThrowArgumentOutOfRangeException()
        => throw new ArgumentOutOfRangeException();

    /// <summary>
    /// Throws a <see cref="NotSupportedException"/> indicating that the collection is read-only.
    /// </summary>
    /// <exception cref="NotSupportedException"/>
    [DoesNotReturn]
    public static void ThrowNotSupportedException_ReadOnlyCollection()
        => throw new NotSupportedException("Collection is read-only.");
}
