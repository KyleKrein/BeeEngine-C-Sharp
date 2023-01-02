namespace BeeEngine.OpenTK.Events;

public class ResourceManager
{
    public static string ProcessFilePath(string filepath)
    {
        var result = filepath.Replace('\\', '/');
        return result;
    }
}