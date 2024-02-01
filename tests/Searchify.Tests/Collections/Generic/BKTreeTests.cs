using System.Diagnostics.CodeAnalysis;
using Searchify.Collections.Generic;
using Searchify.Metrics;

namespace Searchify.Tests.Collections.Generic;

file sealed class IntDistance : DistanceMetric<int, int>
{
    public override int Calculate(int source, int target) => Math.Abs(source - target);

    public override bool Equals(int x, int y) => x == y;

    public override int GetHashCode([DisallowNull] int obj) => obj;
}

public class BKTreeTests
{
    [Fact]
    public void Add_AddsValueToTree()
    {
        BKTree<int, int> tree = new(new IntDistance());
        int value = 5;

        bool result = tree.Add(value);

        Assert.True(result);
        Assert.Single(tree);
        Assert.Contains(value, tree);
    }

    [Fact]
    public void AddRange_AddsValuesToTree()
    {
        BKTree<int, int> tree = new(new IntDistance());
        int[] values = [1, 2, 3, 4, 5];

        tree.AddRange(values);

        Assert.Equal(values.Length, tree.Count);
        foreach (int value in values)
            Assert.Contains(value, tree);
    }

    [Fact]
    public void AddRange_ProducesExpectedTreeStructure()
    {
        // https://en.wikipedia.org/wiki/BK-tree#Example
        BKTree<string, int> tree = new(DistanceMetric.Levenshtein);
        tree.AddRange(["book", "books", "cake", "boo", "boon", "cook", "cake", "cape", "cart"]);

        Assert.False(tree.IsEmpty);
        Assert.Equal("book", tree.Root.Value);
        Assert.Equal(["books", "cake"], tree.Root.Children.Select(x => x.Value));
        Assert.Equal(["boo"], tree.Root.Children.First(x => x.Value == "books").Children.Select(x => x.Value));
        Assert.Equal(["boon", "cook"], tree.Root.Children.First(x => x.Value == "books").Children.First().Children.Select(x => x.Value));
        Assert.Equal(["cape", "cart"], tree.Root.Children.First(x => x.Value == "cake").Children.Select(x => x.Value));
    }

    [Fact]
    public void Remove_ExistingValue_RemovesValueFromTree()
    {
        BKTree<int, int> tree = new(new IntDistance());
        int[] values = [1, 2, 3, 4, 5];
        tree.AddRange(values);
        int value = 5;
        tree.Add(value);

        bool result = tree.Remove(value);

        Assert.True(result);
        Assert.Equal(values.Length - 1, tree.Count);
        Assert.DoesNotContain(value, tree);
        Assert.Equal(values.Where(x => x != value), tree.OrderBy(x => x));
    }

    [Fact]
    public void Remove_NonExistingValue_DoesNotModifyTree()
    {
        BKTree<int, int> tree = new(new IntDistance());
        int value = 5;

        bool result = tree.Remove(value);

        Assert.False(result);
        Assert.Empty(tree);
        Assert.DoesNotContain(value, tree);
    }

    [Fact]
    public void Clear_RemovesAllValuesFromTree()
    {
        BKTree<int, int> tree = new(new IntDistance());
        tree.AddRange([1, 2, 3, 4, 5]);

        tree.Clear();

        Assert.Empty(tree);
    }

    [Fact]
    public void Contains_ExistingValue_ReturnsTrue()
    {
        BKTree<int, int> tree = new(new IntDistance());
        int value = 5;
        tree.Add(value);

        bool result = tree.Contains(value);

        Assert.True(result);
    }

    [Fact]
    public void Contains_NonExistingValue_ReturnsFalse()
    {
        BKTree<int, int> tree = new(new IntDistance());
        tree.AddRange([1, 2, 3, 4, 6]);
        int value = 5;

        bool result = tree.Contains(value);

        Assert.False(result);
    }

    [Fact]
    public void TryGetValue_ExistingValue_ReturnsTrueAndActualValue()
    {
        BKTree<int, int> tree = new(new IntDistance());
        int value = 5;
        tree.Add(value);

        bool result = tree.TryGetValue(value, out int actualValue);

        Assert.True(result);
        Assert.Equal(value, actualValue);
    }

    [Fact]
    public void TryGetValue_NonExistingValue_ReturnsFalse()
    {
        BKTree<int, int> tree = new(new IntDistance());
        int value = 5;

        bool result = tree.TryGetValue(value, out _);

        Assert.False(result);
    }

    [Fact]
    public void Find_NonEmptyTree_ReturnsBestMatch()
    {
        BKTree<string, int> tree = new(DistanceMetric.Levenshtein);
        tree.AddRange(["apple", "banana", "cherry"]);
        string value = "ape";

        BKTreeMatch<string, int>? match = tree.Find(value);

        Assert.Equal("apple", match?.Value);
        Assert.Equal(2, match?.Distance);
    }

    [Fact]
    public void Find_EmptyTree_ReturnsNull()
    {
        BKTree<int, int> tree = new(new IntDistance());
        int value = 5;

        BKTreeMatch<int, int>? match = tree.Find(value);

        Assert.Null(match);
    }

    [Fact]
    public void FindAll_NonEmptyTree_ReturnsMatchesWithinMaxDistance()
    {
        BKTree<string, int> tree = new(DistanceMetric.Levenshtein);
        tree.AddRange(["apple", "banana", "cherry", "grape", "grapes", "orange", "tape"]);
        string value = "ape";
        int maxDistance = 3;

        LinkedList<BKTreeMatch<string, int>> matches = tree.FindAll(value, maxDistance);

        Assert.Equal(4, matches.Count);
        Assert.Equal(new BKTreeMatch<string, int>("tape", 1), matches.First?.Value);
        Assert.Contains(new BKTreeMatch<string, int>("apple", 2), matches);
        Assert.Contains(new BKTreeMatch<string, int>("grape", 2), matches);
        Assert.Equal(new BKTreeMatch<string, int>("grapes", 3), matches.Last?.Value);
    }

    [Fact]
    public void FindAll_EmptyTree_ReturnsEmptyCollection()
    {
        BKTree<int, int> tree = new(new IntDistance());
        int value = 5;
        int maxDistance = 2;

        LinkedList<BKTreeMatch<int, int>> matches = tree.FindAll(value, maxDistance);

        Assert.Empty(matches);
    }

    [Fact]
    public void GetEnumerator_EnumeratesAllValuesInTree()
    {
        BKTree<int, int> tree = new(new IntDistance());
        int[] values = [1, 2, 3, 4, 5];
        tree.AddRange(values);

        IEnumerator<int> enumerator = tree.GetEnumerator();

        List<int> enumeratedValues = new();
        while (enumerator.MoveNext())
        {
            enumeratedValues.Add(enumerator.Current);
        }
        enumeratedValues.Sort();

        Assert.Equal(enumeratedValues, values);
    }

    [Fact]
    public void CopyTo_CopiesValuesToArrayStartingAtIndex()
    {
        BKTree<int, int> tree = new(new IntDistance());
        int[] values = [1, 2, 3, 4, 5];
        tree.AddRange(values);
        int[] array = [-1, 0, 0, 0, 0, 0];

        tree.CopyTo(array, 1);
        array.AsSpan().Sort();

        Assert.Equal(array, [-1, .. values]);
    }
}
