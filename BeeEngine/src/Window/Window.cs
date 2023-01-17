using BeeEngine;
using OpenTK.Windowing.Common;

namespace BeeEngine;

internal abstract class Window: IDisposable
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    public virtual string Title { get; set; }
    
    public virtual VSync VSync { get; set; }

    public Context Context { get; init; }

    public abstract void Init();
    public abstract void Run(Action updateLoop, Action renderLoop);
    public abstract void RunMultiThreaded(Action updateLoop, Action renderLoop);
    public event EventHandler<MouseButtonEventArgs> MouseClick;

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

    public WindowProps()
    {
        Title = "BeeEngine Window";
        Width = 1280;
        Height = 720;
        VSync = VSync.On;
        IsGame = false;
    }
    public WindowProps(string title = "BeeEngine Window", int width = 1280, int height = 720, VSync vSync = VSync.On, bool isGame = false)
    {
        Title = title;
        Width = width;
        Height = height;
        VSync = vSync;
        IsGame = isGame;
    }
}