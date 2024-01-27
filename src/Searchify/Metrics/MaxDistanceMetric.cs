using System.Numerics;
using System.Runtime.CompilerServices;

namespace Searchify.Metrics;

/// <summary>
/// Provides methods for creating instances of the <see cref="MaxDistanceMetric{TValue, TDistance}"/> class.
/// </summary>
public static class MaxDistanceMetric
{
    /// <summary>
    /// Creates a new max distance metric using the specified metric function.
    /// </summary>
    /// <typeparam name="TValue">The type of values to calculate the maximum distance from.</typeparam>
    /// <typeparam name="TDistance">The type representing the calculated maximum distance.</typeparam>
    /// <param name="metric">The metric function used to calculate the max distance.</param>
    /// <returns>The new max distance metric.</returns>
    public static MaxDistanceMetric<TValue, TDistance> Create<TValue, TDistance>(Func<TValue, TDistance> metric)
    {
        ArgumentNullException.ThrowIfNull(metric);

        return new DelegateMaxDistanceMetric<TValue, TDistance>(metric);
    }
}

/// <summary>
/// Provides functionality for calculating the maximum distance
/// between a source value and the set of all possible values.
/// </summary>
/// <typeparam name="TValue">The type of values to calculate the maximum distance from.</typeparam>
/// <typeparam name="TDistance">The type representing the calculated maximum distance.</typeparam>
public abstract class MaxDistanceMetric<TValue, TDistance> : IMaxDistanceMetric<TValue, TDistance>
{
    /// <inheritdoc/>
    public abstract TDistance Calculate(TValue source);
}

internal sealed class DelegateMaxDistanceMetric<TValue, TDistance> : MaxDistanceMetric<TValue, TDistance>
{
    private readonly Func<TValue, TDistance> _delegate;

    public DelegateMaxDistanceMetric(Func<TValue, TDistance> delegateMetric)
    {
        _delegate = delegateMetric;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override TDistance Calculate(TValue source) => _delegate(source);

    public override bool Equals(object? obj)
        => obj is DelegateMaxDistanceMetric<TValue, TDistance> other && _delegate == other._delegate;

    public override int GetHashCode()
        => _delegate.GetHashCode();
}

internal readonly struct MaxStringDistanceRatioMetric<TDistance> : IMaxDistanceMetric<string, TDistance>
    where TDistance : INumberBase<TDistance>
{
    private readonly float _maxRatio;

    public MaxStringDistanceRatioMetric(float maxRatio) => _maxRatio = maxRatio;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TDistance Calculate(string source)
        => source is null ? TDistance.Zero : TDistance.CreateChecked(source.Length * _maxRatio);
}
