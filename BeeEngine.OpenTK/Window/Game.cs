using BeeEngine.Drawing;
using BeeEngine.OpenTK.Events;
using BeeEngine.OpenTK.Gui;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using MouseButton = BeeEngine.OpenTK.Events.MouseButton;

namespace BeeEngine.OpenTK;

public abstract class Game: IDisposable
{
    public static Game Instance { get; private set; }
    public string Title
    {
        get => _nativeWindowSettings.Title;
        set => _nativeWindowSettings.Title = value;
    }

    public int Width
    {
        get => _nativeWindowSettings.Size.X;
        set => _nativeWindowSettings.Size = new Vector2i(value, _nativeWindowSettings.Size.Y);
    }

    public int Height
    {
        get => _nativeWindowSettings.Size.Y;
        set => _nativeWindowSettings.Size = new Vector2i(_nativeWindowSettings.Size.X, value);
    }
    
    public Point Location { get; set; }
    public Size Size 
    {
        get => new Size(_nativeWindowSettings.Size.X, _nativeWindowSettings.Size.Y);
        set => _nativeWindowSettings.Size = new Vector2i(value.Width, value.Height);
    }

    private GameWindow _window;
    
    private GameWindowSettings _gameWindowSettings = GameWindowSettings.Default;
    private NativeWindowSettings _nativeWindowSettings = NativeWindowSettings.Default;
    public Game(string title, int width, int height)
    {
        if (Instance != null)
        {
            throw new InvalidOperationException("Can't have two instances of the game");
        }

        Instance = this;
        _layerStack = new LayerStack();
        _events = new EventQueue(_layerStack);
        _nativeWindowSettings.Flags = ContextFlags.ForwardCompatible;
        _nativeWindowSettings.Profile = ContextProfile.Core;
        _nativeWindowSettings.API = ContextAPI.OpenGL;
        _nativeWindowSettings.AutoLoadBindings = true;
        _nativeWindowSettings.Title = title;
        _nativeWindowSettings.Size = new Vector2i(width, height);
        _window = new GameWindow(_gameWindowSettings, _nativeWindowSettings);
        //Time gameTime = new Time();

        //_window.Load += () => { _controller = new ImGuiController(_window.ClientSize.X, _window.ClientSize.Y); };
        
        
        SetupEventsQueue();

        SubscribeToGameloopEvents();
        _window.Run();
    }

    private void SetupEventsQueue()
    {
        _window.MouseMove += (e) => { _events.AddEvent(new MouseMovedEvent(e.X, e.Y, e.DeltaX, e.DeltaY)); };

        _window.Resize += (e) =>
        {
            //GL.Viewport(0, 0, e.Width, e.Height);

            _events.AddEvent(new WindowResizedEvent(e.Width, e.Height));
            
            //GL.Viewport(0,0,e.Width, e.Height);
        };
        _window.MouseDown += (e) => _events.AddEvent(new MouseDownEvent((MouseButton) (int) e.Button,
            _window.MouseState.Position.X, _window.MouseState.Position.Y));
        _window.MouseUp += (e) => _events.AddEvent(new MouseUpEvent((MouseButton) (int) e.Button,
            _window.MouseState.Position.X, _window.MouseState.Position.Y));
        _window.MouseWheel += (e) =>
        {
            _events.AddEvent(new MouseScrolledEvent(e.Offset.Y, e.Offset.X));
        };
        _window.KeyDown += (e) => _events.AddEvent(new KeyDownEvent((Key) (int) e.Key));
        _window.KeyUp += (e) => _events.AddEvent(new KeyUpEvent((Key) (int) e.Key));
        _window.TextInput += (e) => _events.AddEvent(new KeyTypedEvent(e.Unicode));
        //_window.Move
        /*unsafe
        {
            GLFW.SetScrollCallback(_window.WindowPtr, (window, x, y) =>
            {
                _events.AddEvent(new MouseScrolledEvent((float) x, (float) y));
            });
        }*/
    }

    private void SubscribeToGameloopEvents()
    {
        _window.Load += LoadContent;
        _window.Load += () =>
        {
            GL.ClearColor(Color4.CornflowerBlue);
        };
        _window.Unload += UnloadResources;
        _window.Load += Initialize;
        _window.UpdateFrame += e =>
        {
            //gameTime.TotalTime += TimeSpan.FromMilliseconds(e.Time);
            //gameTime.ElapsedTime = TimeSpan.FromMilliseconds(e.Time);
            Time.Update();
            InitializeGameObjects();
            _events.Dispatch();
            Update();
            UpdateGameObjects();
            LateUpdateGameObjects();
        };
        _window.RenderFrame += e =>
        {
            //_controller.Update(_window, (float)e.Time);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _layerStack.Update();
            Render();
            //_controller.Render();
            //ImGuiController.CheckGLError("End of frame");
            _window.SwapBuffers();
        };
    }


    public void Run()
    {
        
    }

    protected abstract void UnloadResources();

    private void UpdateGameObjects()
    {
        
    }

    private void LateUpdateGameObjects()
    {
        
    }

    private void InitializeGameObjects()
    {
        
    }

    public void PushLayer(Layer layer)
    {
        _layerStack.PushLayer(layer);
    }

    public void PushOverlay(Layer overlay)
    {
        _layerStack.PushOverlay(overlay);
    }
    

    private readonly LayerStack _layerStack;
    private readonly EventQueue _events;
    
    protected abstract void Initialize();
    protected abstract void LoadContent();
    protected abstract void Update();
    protected abstract void Render();

    public void Dispose()
    {
        //_controller.Dispose();
        _layerStack.Dispose();
        _window.Dispose();
    }

    internal GameWindow GetWindow()
    {
        return _window;
    }
}