using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Searchify.Comparison;
using Searchify.Helpers;

namespace Searchify.Metrics;

/// <summary>
/// Provides methods for creating instances of the <see cref="DistanceMetric{TValue, TDistance}"/> class.
/// </summary>
public static class DistanceMetric
{
    /// <summary>
    /// Represents the Levenshtein distance metric for comparing strings.
    /// </summary>
    public static DistanceMetric<string, int> Levenshtein { get; } = new LevenshteinDistanceMetric<OrdinalCaseSensitiveCharComparer>();

    /// <summary>
    /// Represents the case-insensitive Levenshtein distance metric for comparing strings.
    /// </summary>
    public static DistanceMetric<string, int> LevenshteinIgnoreCase { get; } = new LevenshteinDistanceMetric<CurrentCultureIgnoreCaseCharComparer>();

    /// <summary>
    /// Creates a new distance metric using the specified metric function and equality comparer.
    /// </summary>
    /// <typeparam name="TValue">The type of values to calculate the distance between.</typeparam>
    /// <typeparam name="TDistance">The type representing the calculated distance.</typeparam>
    /// <param name="metric">The metric function that calculates the distance between two values.</param>
    /// <param name="comparer">The equality comparer used to compare values.</param>
    /// <returns>The new distance metric.</returns>
    public static DistanceMetric<TValue, TDistance> Create<TValue, TDistance>(
        Func<TValue, TValue, TDistance> metric,
        IEqualityComparer<TValue> comparer)
    {
        ArgumentNullException.ThrowIfNull(metric);
        ArgumentNullException.ThrowIfNull(comparer);

        return new DelegateDistanceMetric<TValue, TDistance>(metric, comparer);
    }

    /// <summary>
    /// Creates a new distance metric using the specified metric function.
    /// </summary>
    /// <typeparam name="TValue">The type of values to calculate the distance between.</typeparam>
    /// <typeparam name="TDistance">The type representing the calculated distance.</typeparam>
    /// <param name="metric">The metric function that calculates the distance between two values.</param>
    /// <returns>The new distance metric.</returns>
    public static DistanceMetric<TValue, TDistance> Create<TValue, TDistance>(
        Func<TValue, TValue, TDistance> metric
    )
        where TDistance : struct
    {
        ArgumentNullException.ThrowIfNull(metric);

        IEqualityComparer<TValue> comparer = ConvertMetricToEqualityComparer(metric);

        return new DelegateDistanceMetric<TValue, TDistance>(metric, comparer);
    }

    /// <summary>
    /// Creates a distance metric based on the Levenshtein distance algorithm.
    /// </summary>
    /// <typeparam name="TComparer">The type of the comparer used to compare characters.</typeparam>
    /// <param name="comparer">The comparer used to compare characters.</param>
    /// <returns>The new distance metric based on the Levenshtein distance algorithm.</returns>
    public static DistanceMetric<string, int> CreateLevenshtein<TComparer>(TComparer comparer)
        where TComparer : IEqualityComparer<char>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        int Calculate(string input, string target)
            => Metrics.Levenshtein.Distance<char, int, TComparer>(input, target, comparer);

        return new DelegateDistanceMetric<string, int>(Calculate, ConvertCharComparerToStringComparer(comparer, Calculate));
    }

    /// <summary>
    /// Creates a distance metric based on the Levenshtein distance algorithm.
    /// </summary>
    /// <typeparam name="TComparer">The type of the comparer used to compare characters.</typeparam>
    /// <param name="comparer">The comparer used to compare characters.</param>
    /// <param name="deletionCost">The cost of deleting a character.</param>
    /// <param name="insertionCost">The cost of inserting a character.</param>
    /// <param name="substitutionCost">The cost of substituting a character.</param>
    /// <returns>The new distance metric based on the Levenshtein distance algorithm.</returns>
    public static DistanceMetric<string, int> CreateLevenshtein<TComparer>(
        TComparer comparer,
        int deletionCost = 1,
        int insertionCost = 1,
        int substitutionCost = 1
    )
        where TComparer : IEqualityComparer<char>
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        int Calculate(string input, string target)
            => Metrics.Levenshtein.Distance<char, int, TComparer>(input, target, comparer, deletionCost, insertionCost, substitutionCost);

        return new DelegateDistanceMetric<string, int>(Calculate, ConvertCharComparerToStringComparer(comparer, Calculate));
    }

    private static IEqualityComparer<string> ConvertCharComparerToStringComparer(IEqualityComparer<char> comparer, Func<string, string, int> metric)
    {
        if (comparer is ICharComparer charComparer)
            return charComparer.AsStringComparer();

        if (comparer is IEqualityComparer<string> stringComparer)
            return stringComparer;

        return EqualityComparer<string>.Create(
            (a, b) => ReferenceEquals(a, b) || a is not null && b is not null && metric(a, b) == 0,
            static x => x.GetHashCode()
        );
    }

    private static IEqualityComparer<TValue> ConvertMetricToEqualityComparer<TValue, TDistance>(
        Func<TValue, TValue, TDistance> metric
    )
        where TDistance : struct
    {
        return EqualityComparer<TValue>.Create(Equals, GetHashCode);

        bool Equals(TValue? x, TValue? y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return EqualityComparer<TDistance>.Default.Equals(metric(x, y), default);
        }

        static int GetHashCode(TValue obj) => obj?.GetHashCode() ?? 0;
    }
}

/// <summary>
/// Provides functionality for calculating the distance between two values.
/// </summary>
/// <typeparam name="TValue">The type of values to calculate the distance between.</typeparam>
/// <typeparam name="TDistance">The type representing the calculated distance.</typeparam>
public abstract class DistanceMetric<TValue, TDistance> : IDistanceMetric<TValue, TDistance>
{
    /// <inheritdoc/>
    public abstract TDistance Calculate(TValue source, TValue target);

    /// <inheritdoc/>
    public abstract bool Equals(TValue? x, TValue? y);

    /// <inheritdoc/>
    public abstract int GetHashCode([DisallowNull] TValue obj);

    bool IEqualityComparer.Equals(object? x, object? y)
    {
        if (x == y)
            return true;

        if (x is null || y is null)
            return false;

        if (x is TValue xValue && y is TValue yValue)
            return Equals(xValue, yValue);

        ThrowHelper.ThrowArgumentException_InvalidArgumentForComparison();
        return false;
    }

    int IEqualityComparer.GetHashCode(object? obj)
    {
        if (obj is null)
            return 0;

        if (obj is TValue objValue)
            return GetHashCode(objValue);

        ThrowHelper.ThrowArgumentException_InvalidArgumentForComparison();
        return 0;
    }
}

internal sealed class DelegateDistanceMetric<TValue, TDistance> : DistanceMetric<TValue, TDistance>
{
    private readonly Func<TValue, TValue, TDistance> _delegate;

    private readonly IEqualityComparer<TValue> _comparer;

    public DelegateDistanceMetric(Func<TValue, TValue, TDistance> delegateMetric, IEqualityComparer<TValue> comparer)
    {
        _delegate = delegateMetric;
        _comparer = comparer;
    }

    public override TDistance Calculate(TValue source, TValue target) => _delegate(source, target);

    public override bool Equals(TValue? x, TValue? y) => _comparer.Equals(x, y);

    public override int GetHashCode([DisallowNull] TValue obj) => _comparer.GetHashCode(obj);

    public override bool Equals(object? obj)
        => obj is DelegateDistanceMetric<TValue, TDistance> other &&
        _delegate == other._delegate &&
        _comparer == other._comparer;

    public override int GetHashCode()
        => HashCode.Combine(_delegate.GetHashCode(), _comparer.GetHashCode());
}

internal sealed class LevenshteinDistanceMetric<TComparer> : DistanceMetric<string, int>
    where TComparer : struct, ICharComparer
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int Calculate(string source, string target)
        => Levenshtein.Distance<char, int, TComparer>(source, target, default);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(string? x, string? y)
        => default(TComparer).AsStringComparer().Equals(x, y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode([DisallowNull] string obj)
        => default(TComparer).AsStringComparer().GetHashCode(obj);

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj?.GetType() == typeof(LevenshteinDistanceMetric<TComparer>);

    public override int GetHashCode()
        => typeof(LevenshteinDistanceMetric<TComparer>).GetHashCode();
}
