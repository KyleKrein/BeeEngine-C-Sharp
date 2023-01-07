namespace BeeEngine;

public class ResourceManager
{
    public static string ProcessFilePath(string filepath)
    {
        var result = filepath.Replace('\\', '/');
        return result;
    }

    public static string GetNameFromPath(string filepath)
    {
        var lastDot = filepath.LastIndexOf('.');
        var lastSlash = filepath.LastIndexOf('/');
        var lastBackSlash = filepath.LastIndexOf('\\');
        lastSlash = Math.Max(lastSlash, lastBackSlash) + 1;
        var count = lastDot == -1 ? filepath.Length - lastSlash : lastDot - lastSlash;
        string name = filepath.Substring(lastSlash, count);
        return name;
    }
    public static ReadOnlySpan<char> GetNameFromPath(ReadOnlySpan<char> filepath)
    {
        var lastDot = filepath.LastIndexOf('.');
        var lastSlash = filepath.LastIndexOf('/');
        var lastBackSlash = filepath.LastIndexOf('\\');
        lastSlash = Math.Max(lastSlash, lastBackSlash) + 1;
        var count = lastDot == -1 ? filepath.Length - lastSlash : lastDot - lastSlash;
        return filepath.Slice(lastSlash, count);
    }
}