namespace Searchify.Benchmarks.TestUtilities;

internal static class Content
{
    public static string Format(string pattern)
    {
        if (pattern.StartsWith("##"))
            return pattern.Substring(1);

        if (pattern.StartsWith('#'))
            return ReadAllText(pattern.Substring(1));

        return pattern;
    }

    public static string ReadAllText(string fileName) => File.ReadAllText(ResolveFileName(fileName));

    public static byte[] ReadAllBytes(string fileName) => File.ReadAllBytes(ResolveFileName(fileName));

    private static string ResolveFileName(string fileName)
    {
        string basePath = Path.GetDirectoryName(GetCurrentFilePath())!;
        string targetPath = Path.Combine(basePath, "../../Content/", fileName);
        return targetPath;

        static string GetCurrentFilePath([CallerFilePath] string? currentPath = null) => currentPath!;
    }
}
