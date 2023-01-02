using BeeEngine.Mathematics;
using BeeEngine.OpenTK.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK;

public abstract class Application: IDisposable
{
    private readonly Window _window;
    public static readonly OS PlatformOS;

    public int Width
    {
        get => _window.Width;
        set => _window.Width = value;
    }
    public int Height
    {
        get => _window.Height;
        set => _window.Height = value;
    }

    public VSync VSync
    {
        get => _window.VSync;
        set => _window.VSync = value;
    }
    public static Application? Instance { get; private set; }
    public Application(WindowProps initSettings = default)
    {
        if (Instance!=null)
        {
            throw new Exception("Can't start two applications at once");
        }
        _isGame = initSettings.IsGame;
        
        Instance = this;
        _window = InitWindow(initSettings);

        Renderer.Renderer.Init();
    }

    static Application()
    {
        PlatformOS = Environment.OSVersion.Platform == PlatformID.Unix ? OS.Mac : OS.Windows;
    }

    private Window InitWindow(WindowProps initSettings)
    {
        switch (PlatformOS)
        {
            case OS.Windows:
                RendererAPI.API = API.OpenGL;
                return new CrossPlatformWindow(initSettings);
            case OS.Linux:
                RendererAPI.API = API.OpenGL;
                return new CrossPlatformWindow(initSettings);
            case OS.Mac:
                RendererAPI.API = API.OpenGL;
                return new CrossPlatformWindow(initSettings);
            default:
                throw new PlatformNotSupportedException();
        }
    }
    
    public void Run()
    {
        _window.Init();
        LoadContent();
        Initialize();
        if (_isGame)
        {
            _window.RunMultiThreaded(UpdateLoop, RenderLoop);
        }
        else
        {
            _window.Run(UpdateLoop, RenderLoop);
        }
        UnloadContent();
        _window.Dispose();
    }

    private readonly bool _isGame;
    private void RenderLoop()
    {
        Render();
        _window.UpdateLayers();
    }

    private void UpdateLoop()
    {
        InitializeGameObjects();
        Update();
        UpdateGameObjects();
        LateUpdateGameObjects();
    }
    private void UpdateGameObjects()
    {
        
    }

    private void LateUpdateGameObjects()
    {
        
    }

    private void InitializeGameObjects()
    {
        
    }

    protected void PushLayer(Layer layer)
    {
        _window.PushLayer(layer);
    }

    protected void PushOverlay(Layer overlay)
    {
        _window.PushOverlay(overlay);
    }
    protected abstract void Initialize();
    protected abstract void LoadContent();
    protected abstract void UnloadContent();
    protected abstract void Update();
    protected abstract void FixedUpdate();
    protected abstract void Render();

    private void ReleaseUnmanagedResources()
    {
        // TODO release unmanaged resources here
    }

    private void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
            _window.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Application()
    {
        Dispose(false);
    }
    
    
    
}

public enum OS
{
    Windows,
    Linux,
    Mac
}