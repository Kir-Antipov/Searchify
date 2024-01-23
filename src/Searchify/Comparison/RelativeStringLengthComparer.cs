namespace Searchify.Comparison;

/// <summary>
/// Compares the relative string lengths of two strings based on an anchor string.
/// </summary>
internal sealed class RelativeStringLengthComparer : IComparer<string>
{
    private readonly string? _anchor;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelativeStringLengthComparer"/> class with the specified anchor string.
    /// </summary>
    /// <param name="anchor">The anchor string to compare against.</param>
    public RelativeStringLengthComparer(string? anchor)
    {
        _anchor = anchor;
    }

    /// <summary>
    /// Compares two strings based on their relative lengths to the anchor string.
    /// </summary>
    /// <param name="x">The first string to compare.</param>
    /// <param name="y">The second string to compare.</param>
    /// <returns>
    /// A signed integer that indicates the relative order of the strings being compared.
    /// </returns>
    public int Compare(string? x, string? y)
    {
        if (x is null)
            return y is null ? 0 : -1;

        if (y is null || _anchor is null)
            return 1;

        return Math.Abs(x.Length - _anchor.Length) - Math.Abs(y.Length - _anchor.Length);
    }
}
