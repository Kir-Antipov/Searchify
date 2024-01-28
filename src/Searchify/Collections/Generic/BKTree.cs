using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Searchify.Metrics;

namespace Searchify.Collections.Generic;

/// <summary>
/// Represents a Burkhard-Keller Tree.
/// </summary>
/// <typeparam name="TValue">The type of values stored in the tree.</typeparam>
/// <typeparam name="TDistance">The type of distance metric used to measure the similarity between values.</typeparam>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(CollectionDebugView<,>))]
public class BKTree<TValue, TDistance> : ICollection<TValue>, IReadOnlyCollection<TValue>, ICollection
    where TDistance : notnull, INumber<TDistance>, IMinMaxValue<TDistance>
{
    private readonly IDistanceMetric<TValue, TDistance> _metric;

    private BKTreeNode<TValue, TDistance>? _root;

    /// <summary>
    /// Initializes a new instance of the <see cref="BKTree{TValue, TDistance}"/> class with the specified distance metric.
    /// </summary>
    /// <param name="metric">The distance metric used to calculate the distance between values.</param>
    public BKTree(IDistanceMetric<TValue, TDistance> metric)
    {
        ArgumentNullException.ThrowIfNull(metric);

        _metric = metric;
    }

    /// <summary>
    /// Gets the distance metric used to calculate the distance between values.
    /// </summary>
    public IDistanceMetric<TValue, TDistance> Metric => _metric;

    /// <summary>
    /// Gets the root node of the tree.
    /// </summary>
    /// <remarks>
    /// The root node is <c>null</c> when the tree is empty.
    /// </remarks>
    public BKTreeNode<TValue, TDistance>? Root => _root;

    /// <summary>
    /// Returns a value that indicates whether the current <see cref="BKTree{TValue, TDistance}"/> is empty.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Root))]
    public bool IsEmpty => _root is null;

    /// <summary>
    /// Gets the total number of nodes in the tree.
    /// </summary>
    public int Count => _root?.Count ?? 0;

    bool ICollection<TValue>.IsReadOnly => false;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => this;

    void ICollection<TValue>.Add(TValue value) => Add(value);

    /// <summary>
    /// Adds a value to the tree.
    /// </summary>
    /// <param name="value">The value to add.</param>
    /// <returns><c>true</c> if the value was added to the tree; otherwise, <c>false</c>.</returns>
    public bool Add(TValue value)
    {
        if (_root is null)
        {
            _root = new(this, value);
            return true;
        }

        return _root.Add(value);
    }

    /// <summary>
    /// Adds a range of values to the tree.
    /// </summary>
    /// <param name="values">The values to add.</param>
    public void AddRange(IEnumerable<TValue> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        foreach (TValue value in values)
            Add(value);
    }

    /// <summary>
    /// Removes a value from the tree.
    /// </summary>
    /// <param name="value">The value to remove.</param>
    /// <returns><c>true</c> if the value was removed from the tree; otherwise, <c>false</c>.</returns>
    public bool Remove(TValue value)
    {
        if (_root is null)
            return false;

        if (!_metric.Equals(_root.Value, value))
            return _root.Remove(value);

        IEnumerable<TValue> nestedValues = _root.Children.SelectMany(static x => x);
        _root = null;

        foreach (TValue nodeValue in nestedValues)
            Add(nodeValue);

        return true;
    }

    /// <summary>
    /// Detaches the root node from the tree.
    /// </summary>
    public void Clear() => _root = null;

    /// <summary>
    /// Determines whether the tree contains a specific value.
    /// </summary>
    /// <param name="value">The value to locate in the tree.</param>
    /// <returns><c>true</c> if the tree contains the specified value; otherwise, <c>false</c>.</returns>
    public bool Contains(TValue value) => _root is not null && _root.Contains(value);

    /// <summary>
    /// Searches the tree for a given value and returns the equal value it finds, if any.
    /// </summary>
    /// <param name="equalValue">The value to search for.</param>
    /// <param name="actualValue">
    /// The value from the tree that the search found, or the default value of <typeparamref name="TValue"/>
    /// when the search yielded no match.
    /// </param>
    /// <returns>A value indicating whether the search was successful.</returns>
    public bool TryGetValue(TValue equalValue, [MaybeNullWhen(false)] out TValue actualValue)
    {
        if (_root is null)
        {
            actualValue = default;
            return false;
        }

        return _root.TryGetValue(equalValue, out actualValue);
    }

    /// <summary>
    /// Finds the best match in the tree for the specified value.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>The best match found in the tree, or <c>null</c> if the search yielded no match.</returns>
    public BKTreeMatch<TValue, TDistance>? Find(TValue value)
        => Find(value, TDistance.MaxValue);

    /// <summary>
    /// Finds the closest match to the specified value within the given maximum distance.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <param name="maxDistance">The maximum distance allowed for a match.</param>
    /// <returns>The closest match to the specified value within the given maximum distance, or <c>null</c> if no match is found.</returns>
    public BKTreeMatch<TValue, TDistance>? Find(TValue value, TDistance maxDistance)
        => _root?.Find(value, maxDistance);

    /// <summary>
    /// Finds all matches in the tree that are within the specified maximum distance from the given value.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <param name="maxDistance">The maximum distance allowed for a match.</param>
    /// <param name="limit">The maximum number of matches to return. Use <c>-1</c> to return all matches.</param>
    /// <returns>A collection of matches within the specified maximum distance sorted by the match distance.</returns>
    public LinkedList<BKTreeMatch<TValue, TDistance>> FindAll(TValue value, TDistance maxDistance, int limit = -1)
        => _root is null ? new() : _root.FindAll(value, maxDistance, limit);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the values in the current tree.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the tree.</returns>
    public IEnumerator<TValue> GetEnumerator()
        => _root is null ? Enumerable.Empty<TValue>().GetEnumerator() : _root.GetEnumerator();

    void ICollection.CopyTo(Array array, int index)
    {
        if (_root is null)
            return;

        ((ICollection)_root).CopyTo(array, index);
    }

    /// <summary>
    /// Copies the elements of the tree to an array, starting at the specified index.
    /// </summary>
    /// <param name="array">
    /// The one-dimensional array that is the destination of the elements copied from the tree.
    /// The array must have zero-based indexing.
    /// </param>
    /// <param name="index">The zero-based index in the array at which copying begins.</param>
    public void CopyTo(TValue[] array, int index)
    {
        if (_root is null)
            return;

        _root.CopyTo(array, index);
    }
}
