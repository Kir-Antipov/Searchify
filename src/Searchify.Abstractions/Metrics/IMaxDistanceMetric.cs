namespace Searchify.Metrics;

/// <summary>
/// Provides functionality for calculating the maximum distance
/// between a source value and the set of all possible values.
/// </summary>
/// <typeparam name="TValue">The type of values to calculate the maximum distance from.</typeparam>
/// <typeparam name="TDistance">The type representing the calculated maximum distance.</typeparam>
public interface IMaxDistanceMetric<TValue, TDistance>
{
    /// <summary>
    /// Calculates the maximum distance between the source value and the set of all possible values.
    /// </summary>
    /// <param name="source">The source value.</param>
    /// <returns>The calculated maximum distance from the source value to the set of all possible values.</returns>
    TDistance Calculate(TValue source);
}
