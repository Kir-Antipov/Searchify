using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Numerics;
using Searchify.Collections.Generic;
using Searchify.Helpers;

namespace Searchify.Metrics;

/// <summary>
/// Represents the set of successful matches found by comparing
/// the input sequence against the pattern sequence.
/// </summary>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(CollectionDebugView<>))]
public readonly struct LevenshteinMatchCollection<TDistance>
    : IList<LevenshteinMatch<TDistance>>, IReadOnlyList<LevenshteinMatch<TDistance>>, IList, IDisposable
    where TDistance : unmanaged, INumber<TDistance>
{
    private readonly LevenshteinTransform[] _transforms;

    private readonly int _transformsLength;

    private readonly TDistance _deletionCost;

    private readonly TDistance _insertionCost;

    private readonly TDistance _substitutionCost;

    private readonly int _hasMaxDistanceAndInputLength;

    private readonly TDistance _maxDistance;

    internal LevenshteinMatchCollection(
        LevenshteinTransform[] transforms,
        int transformsLength,
        int inputLength,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost)
    {
        _transforms = transforms;
        _transformsLength = transformsLength;
        _deletionCost = deletionCost;
        _insertionCost = insertionCost;
        _substitutionCost = substitutionCost;
        _hasMaxDistanceAndInputLength = inputLength | int.MinValue;
    }

    internal LevenshteinMatchCollection(
        LevenshteinTransform[] transforms,
        int transformsLength,
        int inputLength,
        TDistance maxDistance,
        TDistance deletionCost,
        TDistance insertionCost,
        TDistance substitutionCost)
    {
        _transforms = transforms;
        _transformsLength = transformsLength;
        _deletionCost = deletionCost;
        _insertionCost = insertionCost;
        _substitutionCost = substitutionCost;
        _hasMaxDistanceAndInputLength = inputLength;
        _maxDistance = maxDistance;
    }

    object? IList.this[int index]
    {
        get => this[index];
        set => throw new NotSupportedException();
    }

    LevenshteinMatch<TDistance> IList<LevenshteinMatch<TDistance>>.this[int index]
    {
        get => this[index];
        set => throw new NotSupportedException();
    }

    /// <summary>
    /// Gets an individual match.
    /// </summary>
    /// <param name="index">
    /// The index into the <see cref="LevenshteinMatch{TDistance}"/> collection.
    /// </param>
    /// <returns>
    /// The match at the given <paramref name="index"/> in the collection.
    /// </returns>
    public LevenshteinMatch<TDistance> this[int index]
    {
        get
        {
            Enumerator e = GetEnumerator();
            for (int i = 0; e.MoveNext(); i++)
            {
                if (i == index)
                    return e.Current;
            }

            ThrowHelper.ThrowArgumentOutOfRangeException();
            return default;
        }
    }

    /// <summary>
    /// Gets the number of matches.
    /// </summary>
    public int Count
    {
        get
        {
            int count = 0;

            Enumerator e = GetEnumerator();
            while (e.MoveNext())
                count++;

            return count;
        }
    }

    bool IList.IsReadOnly => true;

    bool ICollection<LevenshteinMatch<TDistance>>.IsReadOnly => true;

    bool ICollection.IsSynchronized => true;

    bool IList.IsFixedSize => true;

    object ICollection.SyncRoot => throw new NotSupportedException();

    int IList.IndexOf(object? value) => value is LevenshteinMatch<TDistance> other ? IndexOf(other) : -1;

    /// <summary>
    /// Searches for the specified object and returns the zero-based index of the first
    /// occurrence within the entire <see cref="LevenshteinMatchCollection{TDistance}"/>.
    /// </summary>
    /// <param name="item">
    /// The object to locate in the <see cref="LevenshteinMatchCollection{TDistance}"/>.
    /// </param>
    /// <returns>
    /// The zero-based index of the first occurrence of item within the entire <see cref="LevenshteinMatchCollection{TDistance}"/>,
    /// if found; otherwise, -1.
    /// </returns>
    public int IndexOf(LevenshteinMatch<TDistance> item)
    {
        for ((int i, Enumerator e) = (0, GetEnumerator()); e.MoveNext(); i++)
        {
            if (e.Current == item)
                return i;
        }

        return -1;
    }

    void ICollection.CopyTo(Array array, int index)
    {
        for (Enumerator e = GetEnumerator(); e.MoveNext(); index++)
        {
            if ((uint)index >= (uint)array.Length)
                ThrowHelper.ThrowArgumentException_InsufficientDestinationCapacity();

            array.SetValue(e.Current, index);
        }
    }

    /// <summary>
    /// Copies the elements of the collection to an array, starting at a particular array index.
    /// </summary>
    /// <param name="array">
    /// The one-dimensional array that is the destination of the elements copied
    /// from the collection. The array must have zero-based indexing.
    /// </param>
    /// <param name="index">
    /// The zero-based index in <paramref name="array"/> at which copying begins.
    /// </param>
    public void CopyTo(LevenshteinMatch<TDistance>[] array, int index)
    {
        for (Enumerator e = GetEnumerator(); e.MoveNext(); index++)
        {
            if ((uint)index >= (uint)array.Length)
                ThrowHelper.ThrowArgumentException_InsufficientDestinationCapacity();

            array[index] = e.Current;
        }
    }

    bool IList.Contains(object? value) => value is LevenshteinMatch<TDistance> other && Contains(other);

    /// <summary>
    /// Determines whether an element is in the <see cref="LevenshteinMatchCollection{TDistance}"/>.
    /// </summary>
    /// <param name="item">
    /// The object to locate in the <see cref="LevenshteinMatchCollection{TDistance}"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if item is found in the <see cref="LevenshteinMatchCollection{TDistance}"/>;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(LevenshteinMatch<TDistance> item) => IndexOf(item) >= 0;

    int IList.Add(object? value)
    {
        ThrowHelper.ThrowNotSupportedException_ReadOnlyCollection();
        return 0;
    }

    void ICollection<LevenshteinMatch<TDistance>>.Add(LevenshteinMatch<TDistance> item) => ThrowHelper.ThrowNotSupportedException_ReadOnlyCollection();

    void IList.Insert(int index, object? value) => ThrowHelper.ThrowNotSupportedException_ReadOnlyCollection();

    void IList<LevenshteinMatch<TDistance>>.Insert(int index, LevenshteinMatch<TDistance> item) => ThrowHelper.ThrowNotSupportedException_ReadOnlyCollection();

    void IList.Remove(object? value) => ThrowHelper.ThrowNotSupportedException_ReadOnlyCollection();

    bool ICollection<LevenshteinMatch<TDistance>>.Remove(LevenshteinMatch<TDistance> item)
    {
        ThrowHelper.ThrowNotSupportedException_ReadOnlyCollection();
        return false;
    }

    void IList.RemoveAt(int index) => ThrowHelper.ThrowNotSupportedException_ReadOnlyCollection();

    void IList<LevenshteinMatch<TDistance>>.RemoveAt(int index) => ThrowHelper.ThrowNotSupportedException_ReadOnlyCollection();

    void IList.Clear() => ThrowHelper.ThrowNotSupportedException_ReadOnlyCollection();

    void ICollection<LevenshteinMatch<TDistance>>.Clear() => ThrowHelper.ThrowNotSupportedException_ReadOnlyCollection();

    /// <summary>
    /// Frees unmanaged resources used by the collection.
    /// </summary>
    public void Dispose() => ArrayPool<LevenshteinTransform>.Shared.Return(_transforms);

    IEnumerator<LevenshteinMatch<TDistance>> IEnumerable<LevenshteinMatch<TDistance>>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Provides an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// An enumerator that iterates through the collection.
    /// </returns>
    public Enumerator GetEnumerator() => new(_transforms, _transformsLength, _hasMaxDistanceAndInputLength, _maxDistance, _deletionCost, _insertionCost, _substitutionCost);

    /// <summary>
    /// Represents an enumerator containing the set of successful matches found by
    /// comparing the input sequence against the pattern sequence.
    /// </summary>
    public struct Enumerator : IEnumerator<LevenshteinMatch<TDistance>>
    {
        private readonly LevenshteinTransform[] _transforms;

        private readonly int _transformsLength;

        private readonly TDistance _deletionCost;

        private readonly TDistance _insertionCost;

        private readonly TDistance _substitutionCost;

        private int _hasMaxDistanceAndInputLength;

        private TDistance _maxDistance;

        private int _index;

        internal Enumerator(
            LevenshteinTransform[] transforms,
            int transformsLength,
            int hasMaxDistanceAndInputLength,
            TDistance maxDistance,
            TDistance deletionCost,
            TDistance insertionCost,
            TDistance substitutionCost)
        {
            _transforms = transforms;
            _transformsLength = transformsLength;
            _deletionCost = deletionCost;
            _insertionCost = insertionCost;
            _substitutionCost = substitutionCost;
            _hasMaxDistanceAndInputLength = hasMaxDistanceAndInputLength;
            _maxDistance = maxDistance;
            _index = -1;
        }

        readonly object IEnumerator.Current => Current;

        /// <summary>
        /// Gets the <see cref="LevenshteinMatch{TDistance}"/> element at the current position
        /// of the enumerator.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Enumeration has either not started or has already finished.
        /// </exception>
        public readonly LevenshteinMatch<TDistance> Current
        {
            get
            {
                Span<LevenshteinTransform> transforms = _transforms.AsSpan(0, _transformsLength);
                int index = _index;
                if ((uint)index >= transforms.Length)
                    return default;

                ref LevenshteinTransform transform = ref transforms[index];
                int matchLength = _hasMaxDistanceAndInputLength - transform.Deletions + transform.Insertions;
                int matchLocation = index - matchLength;
                TDistance distance = transform.ToDistance(_deletionCost, _insertionCost, _substitutionCost);
                return new(matchLocation, matchLength, transform.Deletions, transform.Insertions, transform.Substitutions, distance);
            }
        }

        /// <summary>
        /// Advances the enumerator to the next match.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the enumerator was successfully advanced to the next element;
        /// <c>false</c> if the enumerator cannot find additional matches.
        /// </returns>
        public bool MoveNext()
        {
            Span<LevenshteinTransform> transforms = _transforms.AsSpan(0, _transformsLength);
            int nextIndex;

            if (_hasMaxDistanceAndInputLength >= 0)
            {
                nextIndex = Levenshtein.IndexOfNextTransform(transforms, _index, direction: 1, _hasMaxDistanceAndInputLength, _maxDistance, _deletionCost, _insertionCost, _substitutionCost, out _);
                if (nextIndex > 0)
                {
                    _index = nextIndex;
                    return true;
                }

                return false;
            }
            else
            {
                int inputLength = _hasMaxDistanceAndInputLength & int.MaxValue;
                nextIndex = Levenshtein.IndexOfNextTransform(transforms, _index, direction: 1, inputLength, _deletionCost, _insertionCost, _substitutionCost, out _);
                if ((uint)nextIndex < (uint)transforms.Length)
                {
                    _index = nextIndex;
                    _hasMaxDistanceAndInputLength = inputLength;
                    _maxDistance = transforms[nextIndex].ToDistance(_deletionCost, _insertionCost, _substitutionCost);
                    return true;
                }

                return false;
            }
        }

        void IEnumerator.Reset() => _index = -1;

        readonly void IDisposable.Dispose() { }
    }
}
