# Searchify

[![GitHub Build Status](https://img.shields.io/github/actions/workflow/status/Kir-Antipov/Searchify/build.yml?style=flat&logo=github&cacheSeconds=3600)](https://github.com/Kir-Antipov/Searchify/actions/workflows/build.yml)
[![Version](https://img.shields.io/github/v/release/Kir-Antipov/Searchify?sort=date&style=flat&label=version&cacheSeconds=3600)](https://github.com/Kir-Antipov/Searchify/releases/latest)
[![License](https://img.shields.io/github/license/Kir-Antipov/Searchify?style=flat&cacheSeconds=36000)](https://github.com/Kir-Antipov/Searchify/blob/HEAD/LICENSE.md)

<img alt="Searchify Icon" src="https://raw.githubusercontent.com/Kir-Antipov/Searchify/HEAD/media/icon.png" width="128">

`Searchify` is a high-performance .NET library designed to enhance search operations. It brings together efficient fuzzy search, spell checking, and data indexing capabilities - providing a practical solution for quick and precise data retrieval. With easy adaptability to large data sets and an innate ability to manage misspelled queries and flexible search parameters, it ensures optimized search performance. Ideal for data-heavy .NET projects, `Searchify` delivers solid performance without overcomplicating the process.

----

## NuGet Packages

| **Package** | **Latest Version** |
|:------------|:-------------------|
| Searchify   | [![NuGet](https://img.shields.io/nuget/v/Searchify?style=flat&logo=nuget&label=nuget&cacheSeconds=3600)](https://nuget.org/packages/Searchify/ "Download Searchify from NuGet.org") |
| Searchify.Abstractions | [![NuGet](https://img.shields.io/nuget/v/Searchify.Abstractions?style=flat&logo=nuget&label=nuget&cacheSeconds=3600)](https://nuget.org/packages/Searchify.Abstractions/ "Download Searchify.Abstractions from NuGet.org") |

----

## Getting Started

### Installation

To get started, first add the [Searchify](https://nuget.org/packages/Searchify) package to your project. You can do this by running the following command:

```sh
dotnet add package Searchify
```

Alternatively, you can install it via the Package Manager Console with this command:

```sh
Install-Package Searchify
```

### Usage

#### SearchProvider

`SearchProvider` is the magnum opus of this library that combines all its other features to deliver straightforward yet powerful search functionality.

`SearchProvider.Create` provides a range of useful overloads, empowering you to index any collection via any property of any type. However, since string search queries are used far more often than anything else in the real world scenarios, we will focus on them. If you have a collection of articles and wish to index them by their titles to later perform searches on them, it's as easy as:

```csharp
var search = SearchProvider.Create(articles, x => x.Title);
var result = search.Search("Some Title");
```

The new search provider indexes all supplied titles, utilizing the comprising words *(you can provide a custom tokenizer)*, and provides fuzzy search based on the Levenshtein distance *(a custom distance metric or an entire spell checker can also be used)*.

To delve deeper, let's consider the use case that inspired me to develop this library. I was working on a Steam-related application that necessitated a user search feature across games and their corresponding achievements. Considering users are unlikely to remember the exact wording of an achievement or its full description, fuzzy search support was crucial. Hence, I adopted approximate string matching via the Levenshtein distance and implemented a basic loop that compared a query against every entry in a given collection. This approach functioned reasonably well until I discovered that PayDay 2 has over 1300 achievements and is not even the holder of the most significant achievement count on Steam! As you can guess, the rudimentary for loop ain't gonna cut it for such scenarios. A single search would take between 100 to 150 ms on my machine, which was not *that* disastrous, but still quite and quite noticeable. Instead of settling for displaying a spinning preloader of some sort and only showing query results when a user finishes typing, I craved real-time results and, therefore, created this library. Now, it is absolutely effortless for anybody to index anything based on any amount of any properties and carry out instantaneous fuzzy searches with built-in spell checking:

```csharp
var search = SearchProvider.Create(
    achievements,
    [
        x => x.Name,
        x => x.Description,
    ],
    DistanceMetric.LevenshteinIgnoreCase
);

// ...

var query = "compete da heist stealthy with minigun and r-laucnher in shadow raid";
var result = search.Search(query, new() { MaxSuggestions = 3 });

// ...
```

In the example above, we indexed a collection of achievement objects by their names and descriptions using a case-insensitive version of the Levenshtein distance. The outcome of the executed query is as follows:

```
1. In the Shadow Raid job, complete the heist in stealth while having the Vulcan Minigun and HRL-7 Rocket Launcher equipped.
2. Complete any day of a heist in stealth with a Locomotive 12G shotgun modified with the "Silent Killer Suppressor" equipped. Unlocks the "Suppressed Barrel" for the Street Sweeper shotgun, "Rutger" mask, "Banana Peel" material and "Banana" pattern.
3. In the Art Gallery job, complete the heist in stealth within 4 minutes with each crew member wearing the Improved Combined Tactical Vest and no Armor Bag deployable equipped. Unlocks the "Classic Stock" for the AK weapon family, "2 Piece Stock" for the AK and CAR weapon families, "Pachy" mask, "Fossil" material and "Prehistoric Predator" pattern.

Elapsed: 2,3115 ms
```

Processing a natural language query and locating the best matches in the indexed collection took only 2 milliseconds on my laptop *(compared with 100-150 ms using the initial approach)*. By integrating a synonym-normalization layer with built-in spell checkers, you could even create a fully-featured in-memory search engine - and, admit it, that's pretty darn neat!

#### SpellChecker

`SpellChecker` does exactly what you think it does - it checks the spelling of the input.

There are several built-in methods to create a spell checker, one of which is through a provided vocabulary and a distance metric used to determine the similarity between words:

```csharp
var spellChecker = SpellChecker.Create(
    ["book", "books", "cake", "boo", "boon", "cook", "cake", "cape", "cart"],
    DistanceMetric.Levenshtein
);

// IsCorrect = false, Suggestions = ["cook"]
var result = spellChecker.CheckSpelling("cool");

// true, "cook"
var isWordCorrected = spellChecker.TryFixSpelling("cool", out var correctedWord);
```

However, you also have the freedom to create your own `SpellChecker` according to your project requirements.

#### Levenshtein

`Levenshtein` offers a bunch of methods useful for approximate string matching based on the [Levenshtein distance algorithm](https://en.wikipedia.org/wiki/Levenshtein_distance).

The API is designed to be familiar to those accustomed to using `Regex`:

- `IsMatch` - Determines if there is a subsequence within the pattern that, given a maximum specified distance for edits, matches the input sequence.
- `Match` - Searches for the first subsequence within the pattern sequence that matches the input sequence when edited to within a specified maximum distance.
- `LastMatch` - Searches for the last subsequence within the pattern sequence that matches the input sequence when edited to within a specified maximum distance.
- `Matches` - Searches for all subsequences within the pattern sequence that match the input sequence when edited to within a specified maximum distance.
- `EnumerateMatches` - Searches for all subsequences within the pattern sequence that match the input sequence when edited to within a specified maximum distance.
- `Count` - Searches for all subsequences within the pattern sequence that match the input sequence when edited to within a specified maximum distance and returns the number of matches.

For its key feature, `Levenshtein` provides methods that calculate the distance/similarity ratio between two sequences:

- `Distance` - Calculates the Levenshtein distance between two sequences.
- `SubsequenceDistance` - Calculates the Levenshtein distance between the input sequence and its most comparable subsequence in the pattern sequence.
- `Ratio` - Calculates a normalized similarity ratio between two sequences.
- `SubsequenceRatio` - Calculates a normalized subsequence similarity ratio between two sequences.

```csharp
Levenshtein.Distance("word", "World");
Levenshtein.Distance("word", "World", CharComparer.CurrentCultureIgnoreCase);
Levenshtein.Distance("word", "World", CharComparer.CurrentCultureIgnoreCase, substitutionCost: 0);
```

It's safe to say that this is one of the most advanced implementations of the Levenshtein distance algorithm available in .NET. It stands out for being not only one of the fastest and the most memory-efficient, but also the most flexible one. This flexibility extends beyond merely allowing you to provide a custom character comparer and configure weights for different edit operations *(i.e., deletions, insertions, and substitutions)* - it also permits the calculation of the Levenshtein distance between **any** sequence, not limited to strings or arrays of characters.

Here are a few benchmarks against the most popular existing implementation I've discovered on NuGet that continues to be actively maintained:

| Method                  | Parameters               | Mean        | Ratio | Allocated | Alloc Ratio |
|------------------------ |------------------------- |------------:|------:|----------:|------------:|
| Distance_FuzzySearchNet |            (Word, World) | 2,104.08 ns |  1.00 |    3056 B |        1.00 |
| Distance_Searchify      |            (Word, World) |    97.25 ns |  0.05 |         - |        0.00 |
|                         |                          |             |       |           |             |
| IsMatch_FuzzySearchNet  | (Thou(...)End]) [471203] |  6821.44 ms |  1.00 |   20872 B |       1.000 |
| IsMatch_Searchify       | (Thou(...)End]) [471203] |   115.71 ms |  0.02 |     184 B |       0.009 |

Imagine allocating *(especially in 2024)* an entire DOOM level, only to discover that the edit distance between the words "Word" and "World" is 1, while also being 20 times slower than a solution that doesn't allocate at all in such trivial scenarios. While you might argue that the time difference doesn't matter when the operation takes nanoseconds, the scalability issue becomes evident when the optimized solution takes milliseconds, and the non-optimized one takes **seconds**. Therefore, refrain from using naive implementations of popular algorithms; opt for `Searchify`, which offers a greater level of flexibility, requires significantly fewer allocations, and operates more efficiently all at the same time ;)

#### DistanceMetric

`DistanceMetric` serves as a facade for distance functions *(e.g., Levenshtein distance)*, essential for defining a [metric space](https://en.wikipedia.org/wiki/Metric_space).

Currently, there are two built-in implementations of `DistanceMetric` in this library: `DistanceMetric.Levenshtein` and `DistanceMetric.LevenshteinIgnoreCase`.

```csharp
// 2 (replace 'l' with 'L', insert missing 'h')
int distance = DistanceMetric.Levenshtein.Calculate("ligt", "Light");

// 1 (insert missing 'h')
int distanceIgnoreCase = DistanceMetric.LevenshteinIgnoreCase.Calculate("ligt", "Light");
```

Nonetheless, you can always wrap any distance function using the `DistanceMetric.Create` method.

#### BKTree

`BKTree` is an `ICollection<T>` implementation of the [Burkhard-Keller Tree](https://en.wikipedia.org/wiki/BK-tree).

```csharp
var tree = new BKTree<string, int>(DistanceMetric.Levenshtein)
{
    "book",
    "books",
    "cake",
    "boo",
    "boon",
    "cook",
    "cake",
    "cape",
    "cart",
};

var theCakeIsReal = tree.Contains("cake");  // true
var theCakeIsRemoved = tree.Remove("cake"); // true
var theCakeIsALie = !tree.Contains("cake"); // true

// Value = cook, Distance = 1
var result = tree.Find("cool");

// Value = cook, Distance = 1
// Value = boon, Distance = 2
// Value = boo,  Distance = 2
// Value = book, Distance = 2
var results = tree.FindAll("cool", maxDistance: 2);
```

#### Tokenizer

`Tokenizer` may seem like an intimidating term to some folks, but its concept is really quite simple - it takes a specific input and breaks it into smaller components.

Currently, the only built-in `Tokenizer` in this library is `Tokenizer.Words`. Given that the library is primarily targeted towards spell checking and fuzzy string matching, operations become simpler when conducted at the level of individual words:

```csharp
// ["Hello", "world", "This", "is", "a", "test"]
var tokens = Tokenizer.Words.Tokenize("Hello, world! This is a test...");
```

Nevertheless, you can always create your very own `Tokenizer` suited to any data type of your preference.

----

## License

Licensed under the terms of the [MIT License](https://github.com/Kir-Antipov/Searchify/blob/master/LICENSE.md).
