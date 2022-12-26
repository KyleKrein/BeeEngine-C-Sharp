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
    }

    static Application()
    {
        PlatformOS = Environment.OSVersion.Platform == PlatformID.Unix ? OS.Mac : OS.Windows;
    }

    private Window InitWindow(WindowProps initSettings)
    {
        return PlatformOS switch
        {
            OS.Windows => new CrossPlatformWindow(initSettings),
            OS.Linux => new CrossPlatformWindow(initSettings),
            OS.Mac => new CrossPlatformWindow(initSettings),
            _ => throw new PlatformNotSupportedException()
        };
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
        _window.UpdateLayers();
        Render();
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