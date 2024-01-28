using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Searchify.Helpers;
using Searchify.Metrics;
using Spanned.Collections.Generic;

namespace Searchify.Collections.Generic;

/// <summary>
/// Represents a node in a Burkhard-Keller Tree.
/// </summary>
/// <typeparam name="TValue">The type of values stored in the tree.</typeparam>
/// <typeparam name="TDistance">The type of distance metric used to measure the similarity between values.</typeparam>
[DebuggerDisplay("Value = {Value}, Count = {Count}")]
[DebuggerTypeProxy(typeof(CollectionDebugView<,>))]
public sealed class BKTreeNode<TValue, TDistance> : ICollection<TValue>, IReadOnlyCollection<TValue>, ICollection
    where TDistance : notnull, INumber<TDistance>, IMinMaxValue<TDistance>
{
    private readonly BKTree<TValue, TDistance> _tree;

    private readonly TValue _value;

    private readonly Dictionary<TDistance, BKTreeNode<TValue, TDistance>> _children;

    /// <summary>
    /// Initializes a new instance of the <see cref="BKTreeNode{TValue, TDistance}"/> class.
    /// </summary>
    /// <param name="tree">The BKTree that the node belongs to.</param>
    /// <param name="value">The value associated with the node.</param>
    internal BKTreeNode(BKTree<TValue, TDistance> tree, TValue value)
    {
        Debug.Assert(tree is not null);

        _tree = tree;
        _value = value;
        _children = new();
    }

    /// <summary>
    /// Gets the tree associated with this node.
    /// </summary>
    public BKTree<TValue, TDistance> Tree => _tree;

    /// <summary>
    /// Gets the value associated with this node.
    /// </summary>
    public TValue Value => _value;

    /// <summary>
    /// Gets a collection containing the child nodes of this node.
    /// </summary>
    public IReadOnlyCollection<BKTreeNode<TValue, TDistance>> Children => _children.Values;

    /// <summary>
    /// Returns the number of elements accessible from the current node.
    /// </summary>
    public int Count
    {
        get
        {
            int count = 1;

            foreach (BKTreeNode<TValue, TDistance> child in _children.Values)
                count += child.Count;

            return count;
        }
    }

    bool ICollection<TValue>.IsReadOnly => false;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => this;

    void ICollection<TValue>.Add(TValue item) => Add(item);

    /// <summary>
    /// Adds a value to the current sub-tree.
    /// </summary>
    /// <param name="value">The value to add.</param>
    /// <returns><c>true</c> if the value was added to the sub-tree; otherwise, <c>false</c>.</returns>
    internal bool Add(TValue value)
    {
        IDistanceMetric<TValue, TDistance> metric = _tree.Metric;
        BKTreeNode<TValue, TDistance> currentNode = this;

        while (true)
        {
            TDistance distance = metric.Calculate(currentNode._value, value);
            if (TDistance.IsZero(distance))
                return false;

            if (!currentNode._children.TryGetValue(distance, out BKTreeNode<TValue, TDistance>? childNode))
            {
                currentNode._children.Add(distance, new(_tree, value));
                return true;
            }

            currentNode = childNode;
        }
    }

    bool ICollection<TValue>.Remove(TValue item) => Remove(item);

    /// <summary>
    /// Removes a value from the sub-tree.
    /// </summary>
    /// <remarks>
    /// It is not possible to remove the value associated with the current node.
    /// </remarks>
    /// <param name="value">The value to remove.</param>
    /// <returns><c>true</c> if the value was removed from the sub-tree; otherwise, <c>false</c>.</returns>
    internal bool Remove(TValue value)
    {
        foreach (BKTreeNode<TValue, TDistance> child in _children.Values)
        {
            if (child.Remove(value, this))
                return true;
        }

        return false;
    }

    private bool Remove(TValue value, BKTreeNode<TValue, TDistance> parentNode)
    {
        IDistanceMetric<TValue, TDistance> metric = _tree.Metric;
        BKTreeNode<TValue, TDistance> currentNode = this;

        while (true)
        {
            TDistance distance = metric.Calculate(currentNode._value, value);
            if (TDistance.IsZero(distance))
            {
                parentNode._children.Remove(parentNode._children.First(x => x.Value == currentNode).Key);
                IEnumerable<TValue> nestedValues = currentNode._children.Values.SelectMany(static x => x);
                foreach (TValue nestedValue in nestedValues)
                    parentNode.Add(nestedValue);

                return true;
            }

            if (!currentNode._children.TryGetValue(distance, out BKTreeNode<TValue, TDistance>? childNode))
                return false;

            parentNode = currentNode;
            currentNode = childNode;
        }
    }

    void ICollection<TValue>.Clear() => Clear();

    /// <summary>
    /// Detaches all child nodes from the current node.
    /// </summary>
    internal void Clear() => _children.Clear();

    /// <summary>
    /// Determines whether the current sub-tree contains a specific value.
    /// </summary>
    /// <param name="value">The value to locate in the sub-tree.</param>
    /// <returns><c>true</c> if the sub-tree contains the specified value; otherwise, <c>false</c>.</returns>
    public bool Contains(TValue value) => Find(value, TDistance.Zero) is not null;

    /// <summary>
    /// Searches the sub-tree for a given value and returns the equal value it finds, if any.
    /// </summary>
    /// <param name="equalValue">The value to search for.</param>
    /// <param name="actualValue">
    /// The value from the sub-tree that the search found, or the default value of <typeparamref name="TValue"/>
    /// when the search yielded no match.
    /// </param>
    /// <returns>A value indicating whether the search was successful.</returns>
    public bool TryGetValue(TValue equalValue, [MaybeNullWhen(false)] out TValue actualValue)
    {
        BKTreeMatch<TValue, TDistance>? match = Find(equalValue, TDistance.Zero);
        if (match is null)
        {
            actualValue = default;
            return false;
        }
        else
        {
            actualValue = match.Value.Value;
            return true;
        }
    }

    /// <summary>
    /// Finds the best match in the sub-tree for the specified value.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>The best match found in the sub-tree, or <c>null</c> if the search yielded no match.</returns>
    public BKTreeMatch<TValue, TDistance>? Find(TValue value)
        => Find(value, TDistance.MaxValue);

    /// <summary>
    /// Finds the closest match to the specified value within the given maximum distance.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <param name="maxDistance">The maximum distance allowed for a match.</param>
    /// <returns>The closest match to the specified value within the given maximum distance, or <c>null</c> if no match is found.</returns>
    public BKTreeMatch<TValue, TDistance>? Find(TValue value, TDistance maxDistance)
    {
        IDistanceMetric<TValue, TDistance> metric = _tree.Metric;
        BKTreeNode<TValue, TDistance>? bestNode = null;
        TDistance bestDistance = maxDistance;

        BKTreeNodeBuffer<TValue, TDistance> buffer = new();
        ValueStack<BKTreeNode<TValue, TDistance>> nodes = new(buffer);
        nodes.Push(this);

        while (nodes.TryPop(out BKTreeNode<TValue, TDistance>? currentNode))
        {
            TDistance currentDistance = metric.Calculate(value, currentNode._value);
            if (currentDistance <= bestDistance)
                (bestNode, bestDistance) = (currentNode, currentDistance);

            foreach ((TDistance childDistance, BKTreeNode<TValue, TDistance> childNode) in currentNode._children)
            {
                if (TDistance.Abs(childDistance - currentDistance) <= bestDistance)
                    nodes.Push(childNode);
            }
        }

        nodes.Dispose();
        return bestNode is null ? null : new(bestNode._value, bestDistance);
    }

    /// <summary>
    /// Finds all matches in the sub-tree that are within the specified maximum distance from the given value.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <param name="maxDistance">The maximum distance allowed for a match.</param>
    /// <param name="limit">The maximum number of matches to return. Use <c>-1</c> to return all matches.</param>
    /// <returns>A collection of matches within the specified maximum distance sorted by the match distance.</returns>
    public LinkedList<BKTreeMatch<TValue, TDistance>> FindAll(TValue value, TDistance maxDistance, int limit = -1)
    {
        if (limit == 0)
            return new();
        else if (limit < 0)
            limit = int.MaxValue;

        IDistanceMetric<TValue, TDistance> metric = _tree.Metric;
        LinkedList<BKTreeMatch<TValue, TDistance>> matches = new();

        BKTreeNodeBuffer<TValue, TDistance> buffer = new();
        ValueStack<BKTreeNode<TValue, TDistance>> nodes = new(buffer);
        nodes.Push(this);

        while (nodes.TryPop(out BKTreeNode<TValue, TDistance>? currentNode))
        {
            TDistance currentDistance = metric.Calculate(value, currentNode._value);
            if (currentDistance <= maxDistance)
            {
                if (matches.Count == limit)
                    matches.RemoveLast();

                BKTreeMatch<TValue, TDistance> match = new(currentNode._value, currentDistance);
                LinkedListNode<BKTreeMatch<TValue, TDistance>>? lastNode = matches.First is null || currentDistance <= matches.First.ValueRef.Distance ? null : matches.First;
                while (lastNode is { Next: not null } && currentDistance > lastNode.Next.ValueRef.Distance)
                    lastNode = lastNode.Next;

                if (lastNode is null)
                    matches.AddFirst(match);
                else
                    matches.AddAfter(lastNode, match);

                if (matches.Count == limit)
                    maxDistance = matches.Last!.ValueRef.Distance;
            }

            foreach ((TDistance childDistance, BKTreeNode<TValue, TDistance> childNode) in currentNode._children)
            {
                if (TDistance.Abs(childDistance - currentDistance) <= maxDistance)
                    nodes.Push(childNode);
            }
        }

        return matches;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{nameof(BKTreeNode<TValue, TDistance>)} {{ Value = {_value} }}";

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the values in the current sub-tree.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the sub-tree.</returns>
    public IEnumerator<TValue> GetEnumerator()
    {
        yield return _value;

        foreach (BKTreeNode<TValue, TDistance> child in _children.Values)
        {
            foreach (TValue childValue in child)
                yield return childValue;
        }
    }

    void ICollection.CopyTo(Array array, int index)
    {
        for (IEnumerator<TValue> e = GetEnumerator(); e.MoveNext(); index++)
        {
            if ((uint)index >= (uint)array.Length)
                ThrowHelper.ThrowArgumentException_InsufficientDestinationCapacity();

            array.SetValue(e.Current, index);
        }
    }

    /// <summary>
    /// Copies the elements of the sub-tree to an array, starting at the specified index.
    /// </summary>
    /// <param name="array">
    /// The one-dimensional array that is the destination of the elements copied from the sub-tree.
    /// The array must have zero-based indexing.
    /// </param>
    /// <param name="index">The zero-based index in the array at which copying begins.</param>
    public void CopyTo(TValue[] array, int index)
    {
        for (IEnumerator<TValue> e = GetEnumerator(); e.MoveNext(); index++)
        {
            if ((uint)index >= (uint)array.Length)
                ThrowHelper.ThrowArgumentException_InsufficientDestinationCapacity();

            array[index] = e.Current;
        }
    }
}
