using Searchify.Metrics;

namespace Searchify.Tests.Metrics;

public class DistanceMetricTests
{
    [Fact]
    public void Levenshtein_EqualStrings_ReturnsZeroDistance()
    {
        string input = "hello";
        string target = "hello";

        int distance = DistanceMetric.Levenshtein.Calculate(input, target);

        Assert.Equal(0, distance);
    }

    [Fact]
    public void Levenshtein_DifferentStrings_ReturnsCorrectDistance()
    {
        string input = "kitten";
        string target = "sitting";

        int distance = DistanceMetric.Levenshtein.Calculate(input, target);

        Assert.Equal(3, distance);
    }

    [Fact]
    public void LevenshteinIgnoreCase_EqualStrings_ReturnsZeroDistance()
    {
        string input = "hello";
        string target = "HELLo";

        int distance = DistanceMetric.LevenshteinIgnoreCase.Calculate(input, target);

        Assert.Equal(0, distance);
    }

    [Fact]
    public void LevenshteinIgnoreCase_DifferentStrings_ReturnsCorrectDistance()
    {
        string input = "kitten";
        string target = "SITTING";

        int distance = DistanceMetric.LevenshteinIgnoreCase.Calculate(input, target);

        Assert.Equal(3, distance);
    }
}
