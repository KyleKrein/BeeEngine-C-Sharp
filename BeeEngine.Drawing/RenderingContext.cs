namespace BeeEngine.Drawing;

public static class RenderingContext
{
    internal static Renderer _renderer;
    public static bool Initialized { get; private set; } = false;
    static RenderingContext()
    {
        _renderer = new Renderer();
    }

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
}