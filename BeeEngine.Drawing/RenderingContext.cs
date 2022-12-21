/*namespace BeeEngine.Drawing;

public static class RenderingContext
{
    public static bool Initialized { get; private set; } = false;

    public static void Create(params WindowFlags[] flags)
    {
        _renderer.InitWithoutWindow(flags);
        Initialized = true;
    }

    internal static Renderer GetRenderer()
    {
        if (!Initialized)
        {
            Create();
        }
        return _renderer;
    }
}*/