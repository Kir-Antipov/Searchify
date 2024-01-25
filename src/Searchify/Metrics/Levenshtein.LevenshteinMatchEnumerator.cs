using System.Numerics;
using Spanned;

namespace Searchify.Metrics;

public static partial class Levenshtein
{
    /// <summary>
    /// Represents an enumerator containing the set of successful matches found by
    /// comparing the input sequence against the pattern sequence.
    /// </summary>
    public ref struct LevenshteinMatchEnumerator<TDistance> where TDistance : unmanaged, INumber<TDistance>
    {
        private readonly SpanOwner<LevenshteinTransform> _transforms;

        private readonly TDistance _deletionCost;

        private readonly TDistance _insertionCost;

        private readonly TDistance _substitutionCost;

        private int _hasMaxDistanceAndInputLength;

        private TDistance _maxDistance;

        private int _index;

        internal LevenshteinMatchEnumerator(
            SpanOwner<LevenshteinTransform> transforms,
            int inputLength,
            TDistance deletionCost,
            TDistance insertionCost,
            TDistance substitutionCost)
        {
            _transforms = transforms;
            _deletionCost = deletionCost;
            _insertionCost = insertionCost;
            _substitutionCost = substitutionCost;
            _hasMaxDistanceAndInputLength = inputLength | int.MinValue;
            _index = -1;
        }

        internal LevenshteinMatchEnumerator(
            SpanOwner<LevenshteinTransform> transforms,
            int inputLength,
            TDistance maxDistance,
            TDistance deletionCost,
            TDistance insertionCost,
            TDistance substitutionCost)
        {
            _transforms = transforms;
            _deletionCost = deletionCost;
            _insertionCost = insertionCost;
            _substitutionCost = substitutionCost;
            _hasMaxDistanceAndInputLength = inputLength;
            _maxDistance = maxDistance;
            _index = -1;
        }

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
                Span<LevenshteinTransform> transforms = _transforms.Span;
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
        /// Provides an enumerator that iterates through the matches.
        /// </summary>
        /// <returns>
        /// A copy of this enumerator.
        /// </returns>
        public readonly LevenshteinMatchEnumerator<TDistance> GetEnumerator() => this;

        /// <summary>
        /// Advances the enumerator to the next match.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the enumerator was successfully advanced to the next element;
        /// <c>false</c> if the enumerator cannot find additional matches.
        /// </returns>
        public bool MoveNext()
        {
            Span<LevenshteinTransform> transforms = _transforms.Span;
            int nextIndex;

            if (_hasMaxDistanceAndInputLength >= 0)
            {
                nextIndex = IndexOfNextTransform(transforms, _index, direction: 1, _hasMaxDistanceAndInputLength, _maxDistance, _deletionCost, _insertionCost, _substitutionCost, out _);
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
                nextIndex = IndexOfNextTransform(transforms, _index, direction: 1, inputLength, _deletionCost, _insertionCost, _substitutionCost, out _);
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

        /// <summary>
        /// Frees unmanaged resources used by this iterator.
        /// </summary>
        public readonly void Dispose() => _transforms.Dispose();
    }
}
