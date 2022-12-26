using OpenTK.Windowing.Common;

namespace BeeEngine.OpenTK;

public abstract class Window: IDisposable
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    public virtual string Title { get; set; }
    
    public virtual VSync VSync { get; set; }

    public Window(WindowProps initSettings)
    {
        Width = initSettings.Width;
        Height = initSettings.Height;
        Title = initSettings.Title;
    }

    public abstract void Init();
    public abstract void Run(Action updateLoop, Action renderLoop);
    public abstract void RunMultiThreaded(Action updateLoop, Action renderLoop);

    public event EventHandler<MouseButtonEventArgs> MouseClick;
    public abstract void DispatchEvents();
    public abstract void UpdateLayers();
    public abstract void PushLayer(Layer layer);
    public abstract void PushOverlay(Layer overlay);
    public abstract void PopLayer(Layer layer);
    public abstract void PopOverlay(Layer overlay);

    public abstract void ReleaseUnmanagedResources();

    protected virtual void Dispose(bool disposing)
    {
        
    }

    public void Dispose()
    {
        Dispose(true);
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Window()
    {
        Dispose(false);
        ReleaseUnmanagedResources();
    }
}

public readonly struct WindowProps
{
    public readonly string Title;
    public readonly int Width;
    public readonly int Height;
    public readonly VSync VSync;
    public readonly bool IsGame;
    public WindowProps(string title = "BeeEngine Window", int width = 1280, int height = 720, VSync vSync = VSync.On, bool isGame = false)
    {
        Title = title;
        Width = width;
        Height = height;
        VSync = vSync;
        IsGame = isGame;
    }
}