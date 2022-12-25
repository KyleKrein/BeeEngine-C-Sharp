using OpenTK.Windowing.Common;

namespace BeeEngine.OpenTK;

internal abstract class Window: IDisposable
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    public virtual string Title { get; set; }
    
    public virtual bool IsVSync { get; }

    public void SetVSync(bool enabled)
    {
        SetVSyncInternal(enabled);
    }

    public Window(WindowProps initSettings = new WindowProps())
    {
        Width = initSettings.Width;
        Height = initSettings.Height;
        Title = initSettings.Title;
        SetVSyncInternal(initSettings.EnableVSync);
    }

    internal abstract void SetVSyncInternal(bool enabled);

    public event EventHandler<MouseButtonEventArgs> MouseClick;

    protected abstract void Initialize();
    protected abstract void LoadContent();
    protected abstract void UnloadContent();
    protected abstract void Update();
    protected abstract void FixedUpdate();
    protected abstract void Render();

    internal abstract void ReleaseUnmanagedResources();

    internal virtual void Dispose(bool disposing)
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
    public readonly bool EnableVSync;
    public WindowProps(string title = "BeeEngine Window", int width = 1280, int height = 720, bool enableVSync = true)
    {
        Title = title;
        Width = width;
        Height = height;
        EnableVSync = enableVSync;
    }
}