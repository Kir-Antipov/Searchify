using Searchify.Processing;

namespace Searchify.Tests.Processing;

public class TokenizerTest
{
    [Fact]
    public void Words_ReturnsCorrectTokens()
    {
        string input = "Hello, world! This is a test...";

        IEnumerable<string> tokens = Tokenizer.Words.Tokenize(input);

        Assert.Equal(["Hello", "world", "This", "is", "a", "test"], tokens);
    }
}
