using System.Collections;

namespace Searchify.Metrics;

/// <summary>
/// Provides functionality for calculating the distance between two values.
/// </summary>
/// <typeparam name="TValue">The type of values to calculate the distance between.</typeparam>
/// <typeparam name="TDistance">The type representing the calculated distance.</typeparam>
public interface IDistanceMetric<TValue, TDistance> : IEqualityComparer<TValue>, IEqualityComparer
{
    /// <summary>
    /// Calculates the distance between the source and target values.
    /// </summary>
    /// <param name="source">The source value.</param>
    /// <param name="target">The target value.</param>
    /// <returns>The calculated distance between the source and target values.</returns>
    TDistance Calculate(TValue source, TValue target);
}
