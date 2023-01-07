using BeeEngine.Profiling;
using BeeEngine;
using BeeEngine.Core;
using BeeEngine.Events;
using BeeEngine.OpenTK.Profiling;
using BeeEngine.Platform.OpenGL;

namespace BeeEngine;

public abstract class Application: IDisposable
{
    private readonly Window _window;
    public static readonly OS PlatformOS;
    private LayerStack _layerStack;

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
        Instrumentor.BeginSession("Startup", "startup.json");
        _isGame = initSettings.IsGame;
        
        Instance = this;
        DebugTimer.Start("InitWindow");
        _window = InitWindow(initSettings);
        DebugTimer.End("InitWindow");
        InitGui();
        DebugTimer.Start("Renderer.Init");
        BeeEngine.Renderer.Init();
        DebugTimer.End("Renderer.Init");
#if DEBUG
        PushOverlay(new DebugLayer());
#endif
    }
    [ProfileMethod]
    private void InitGui()
    {
        switch (PlatformOS)
        {
            case OS.Windows:
                _layerStack.SetGUILayer(new ImGuiLayerOpenGL());
                break;
            case OS.Linux:
                _layerStack.SetGUILayer(new ImGuiLayerOpenGL());
                break;
            case OS.Mac:
                _layerStack.SetGUILayer(new ImGuiLayerOpenGL());
                break;
            default:
                throw new PlatformNotSupportedException();
        }
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
                _layerStack = new LayerStack();
                _eventQueue = new EventQueue(_layerStack);
                return new CrossPlatformWindow(initSettings, _eventQueue);
            case OS.Linux:
                RendererAPI.API = API.OpenGL;
                _layerStack = new LayerStack();
                _eventQueue = new EventQueue(_layerStack);
                return new CrossPlatformWindow(initSettings, _eventQueue);
            case OS.Mac:
                RendererAPI.API = API.OpenGL;
                _layerStack = new LayerStack();
                _eventQueue = new EventQueue(_layerStack);
                return new CrossPlatformWindow(initSettings, _eventQueue);
            default:
                throw new PlatformNotSupportedException();
        }
    }
    
    public void Run()
    {
        DebugTimer.Start("Window.Init()");
        _window.Init();
        DebugTimer.End("Window.Init()");
        DebugTimer.Start("LoadContent");
        LoadContent();
        DebugTimer.End("LoadContent");
        DebugTimer.Start("Initialize");
        Initialize();
        DebugTimer.End("Initialize");
        Instrumentor.EndSession();
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
    private EventQueue _eventQueue;

    private void RenderLoop()
    {
        Render();
        
    }
    [ProfileMethod]
    private void UpdateLoop()
    {
        //DebugTimer.Start();
        _eventQueue.Dispatch();
        InitializeGameObjects();
        DebugTimer.Start("Update");
        Update();
        DebugTimer.End("Update");
        _layerStack.Update();
        UpdateGameObjects();
        LateUpdateGameObjects();
        //DebugTimer.End();
    }
    protected abstract void OnEvent(ref EventDispatcher e);
    [ProfileMethod]
    private void UpdateGameObjects()
    {
        
    }
    [ProfileMethod]
    private void LateUpdateGameObjects()
    {
        
    }
    [ProfileMethod]
    private void InitializeGameObjects()
    {
        
    }

    protected void PushLayer(Layer layer)
    {
        _layerStack.PushLayer(layer);
    }

    protected void PushOverlay(Layer overlay)
    {
        _layerStack.PushOverlay(overlay);
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
    
    
    //Work with shader library
    protected Shader LoadShader(string filepath)
    {
        return BeeEngine.Renderer.Shaders.Load(filepath);
    }
    protected Shader LoadShader(string name, string filepath)
    {
        return BeeEngine.Renderer.Shaders.Load(filepath);
    }

    protected void AddShader(string name, Shader shader)
    {
        BeeEngine.Renderer.Shaders.Add(name, shader);
    }
    protected void AddShader(Shader shader)
    {
        BeeEngine.Renderer.Shaders.Add(shader);
    }

    protected Shader GetShader(string name)
    {
        return BeeEngine.Renderer.Shaders.Get(name);
    }

    internal void Dispatch(ref EventDispatcher e)
    {
        e.Dispatch<WindowResizedEvent>(OnWindowResize);
        OnEvent(ref e);
    }

    private bool OnWindowResize(WindowResizedEvent e)
    {
        if (Width == 0 || Height == 0)
        {
            IsMinimized = true;
            return false;
        }

        IsMinimized = false;
        BeeEngine.Renderer.OnWindowResized(e.Width, e.Height);
        return false;
    }

    public bool IsMinimized { get; private set; } = false;
}

public enum OS
{
    Windows,
    Linux,
    Mac,
    IOS,
    Android
}