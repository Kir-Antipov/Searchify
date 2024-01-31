using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Searchify.Helpers;

namespace Searchify;

/// <summary>
/// Represents the result of a search operation.
/// </summary>
/// <typeparam name="T">The type of elements in the search result.</typeparam>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(),nq}}")]
public readonly struct SearchResult<T> : ISearchResult<T>
{
    /// <summary>
    /// Returns an empty instance of the <see cref="SearchResult{T}"/>.
    /// </summary>
    public static SearchResult<T> Empty => default;

    private readonly bool _success;

    private readonly T? _value;

    private readonly IEnumerable<SearchSuggestion<T>>? _suggestions;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchResult{T}"/> struct.
    /// </summary>
    /// <param name="suggestions">The suggestions related to the search result.</param>
    public SearchResult(IEnumerable<SearchSuggestion<T>>? suggestions)
    {
        _success = false;
        _value = default;
        _suggestions = suggestions;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchResult{T}"/> struct.
    /// </summary>
    /// <param name="value">The successful search result value.</param>
    /// <param name="suggestions">The suggestions related to the search result.</param>
    public SearchResult(T value, IEnumerable<SearchSuggestion<T>>? suggestions = null)
    {
        if (value is null)
            ThrowHelper.ThrowArgumentNullException(nameof(value));

        _success = true;
        _value = value;
        _suggestions = suggestions;
    }

    internal SearchResult(bool success, T? value, IEnumerable<SearchSuggestion<T>>? suggestions)
    {
        _success = success;
        _value = value;
        _suggestions = suggestions;
    }

    /// <inheritdoc/>
    [MemberNotNullWhen(true, nameof(Value))]
    public bool Success => _success;

    /// <inheritdoc/>
    public T? Value => _value;

    /// <inheritdoc/>
    public IEnumerable<SearchSuggestion<T>> Suggestions => _suggestions ?? Enumerable.Empty<SearchSuggestion<T>>();

    private string DebuggerDisplay() => Success ? $"Success = true, Value = {Value}" : "Success = false";

    /// <summary>
    /// Returns an enumerator that iterates through the search result and suggestions.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the search result and suggestions.</returns>
    public Enumerator GetEnumerator() => new(_success, _value, _suggestions);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Represents an enumerator for iterating through the search result and suggestions.
    /// </summary>
    public struct Enumerator : IEnumerator<T>
    {
        /// <summary>
        /// The current state of the enumerator.
        ///
        /// <para/>
        ///
        /// Possible values:
        /// <list type="bullet">
        ///     <item><term>0</term><description>Enumeration not started; search was successful; no current value.</description></item>
        ///     <item><term>1</term><description>Enumeration not started; search was unsuccessful; no current value.</description></item>
        ///     <item><term>2</term><description>Enumeration started; search was successful; current value represents the search result.</description></item>
        ///     <item><term>3</term><description>Enumeration started, search was unsuccessful; current value represents the current suggestion.</description></item>
        ///     <item><term>4</term><description>Enumeration started, search was successful; current value represents the current suggestion.</description></item>
        /// </list>
        /// </summary>
        private byte _state;

        private readonly T? _value;

        private readonly IEnumerator<SearchSuggestion<T>> _suggestions;

        internal Enumerator(bool success, T? value, IEnumerable<SearchSuggestion<T>>? suggestions)
        {
            _state = success ? (byte)0 : (byte)1;
            _value = value;
            _suggestions = (suggestions ?? Enumerable.Empty<SearchSuggestion<T>>()).GetEnumerator();
        }

        readonly object? IEnumerator.Current => Current;

        /// <inheritdoc/>
        public readonly T Current => _state <= 2 ? _value! : _suggestions.Current;

        /// <inheritdoc/>
        public bool MoveNext()
        {
            switch (_state)
            {
                case 0:
                    _state = 2;
                    return true;

                case 1:
                case 2:
                    _state += 2;
                    return _suggestions.MoveNext();

                default:
                    return _suggestions.MoveNext();
            }
        }

        /// <inheritdoc/>
        public readonly void Dispose() => _suggestions.Dispose();

        void IEnumerator.Reset()
        {
            _state = (byte)(_state % 2);
            _suggestions.Reset();
        }
    }
}
