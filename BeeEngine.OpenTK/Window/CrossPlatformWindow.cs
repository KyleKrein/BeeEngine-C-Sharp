using System.Diagnostics;
using BeeEngine.Drawing;
using BeeEngine.Events;
using BeeEngine.Platform.OpenGL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using InvalidOperationException = System.InvalidOperationException;

namespace BeeEngine;

internal class CrossPlatformWindow:Window, IDisposable
{
    public static CrossPlatformWindow Instance { get; private set; }
    public override string Title
    {
        get => _nativeWindowSettings.Title;
        set => _nativeWindowSettings.Title = value;
    }

    public override int Width
    {
        get => _nativeWindowSettings.Size.X;
        set => _nativeWindowSettings.Size = new Vector2i(value, _nativeWindowSettings.Size.Y);
    }

    public override int Height
    {
        get => _nativeWindowSettings.Size.Y;
        set => _nativeWindowSettings.Size = new Vector2i(_nativeWindowSettings.Size.X, value);
    }

    private VSync _vSync = VSync.On;
    public override VSync VSync
    {
        get => _vSync;
        set
        {
            GLFW.SwapInterval(value == VSync.Off? 0: 1);
            _vSync = value;
        }
    }

    private double _renderFrequency;
    public double RenderFrequency
    {
        get => this._renderFrequency;
        set
        {
            if (value <= 1.0)
                this._renderFrequency = 0.0;
            else if (value <= 500.0)
                this._renderFrequency = value;
            else
                this._renderFrequency = 500.0;
        }
    }

    /// <summary>
    /// Gets a double representing the time spent in the RenderFrame function, in seconds.
    /// </summary>
    public double RenderTime { get; protected set; } = default;

    /// <summary>
    /// Gets a double representing the time spent in the UpdateFrame function, in seconds.
    /// </summary>
    public double UpdateTime { get; protected set; } = default;
    /// <summary>
    /// Gets or sets a double representing the update frequency, in hertz.
    /// </summary>
    /// <remarks>
    ///  <para>
    /// A value of 0.0 indicates that UpdateFrame events are generated at the maximum possible frequency (i.e. only
    /// limited by the hardware's capabilities).
    ///  </para>
    ///  <para>Values lower than 1.0Hz are clamped to 0.0. Values higher than 500.0Hz are clamped to 500.0Hz.</para>
    /// </remarks>
    public double UpdateFrequency
    {
        get => this._updateFrequency;
        set
        {
            if (value < 1.0)
                this._updateFrequency = 0.0;
            else if (value <= 500.0)
                this._updateFrequency = value;
            else
                this._updateFrequency = 500.0;
        }
    }
    
    public Point Location { get; set; }
    public Size Size 
    {
        get => new Size(_nativeWindowSettings.Size.X, _nativeWindowSettings.Size.Y);
        set => _nativeWindowSettings.Size = new Vector2i(value.Width, value.Height);
    }

    private NativeWindow _window;
    
    private GameWindowSettings _gameWindowSettings = GameWindowSettings.Default;
    private NativeWindowSettings _nativeWindowSettings = NativeWindowSettings.Default;
    public CrossPlatformWindow(WindowProps initSettings, EventQueue eventQueue): base(initSettings)
    {
        DebugTimer.Start();
        if (Instance != null)
        {
            var message = "Can't have two instances of the game";
            Log.Error(message);
            throw new InvalidOperationException(message);
        }

        Instance = this;
        if (Application.PlatformOS == OS.Mac)
        {
            _nativeWindowSettings.Flags = ContextFlags.ForwardCompatible;
        }
        _nativeWindowSettings.Profile = ContextProfile.Core;
        _nativeWindowSettings.API = ContextAPI.OpenGL;
        _nativeWindowSettings.AutoLoadBindings = true;
        _nativeWindowSettings.Title = initSettings.Title;
        _nativeWindowSettings.Size = new Vector2i(initSettings.Width, initSettings.Height);
        DebugTimer.Start("Creating window");
        _window = new NativeWindow(_nativeWindowSettings);
        var context = new OpenGLContext(_window);
        DebugTimer.End("Creating window");
        Context = context;
        _events = eventQueue;
        //Time gameTime = new Time();
        UpdateFrequency = 0;
        RenderFrequency = 0;
        GL.ClearColor(Color4.CornflowerBlue);
        //_window.Load += () => { _controller = new ImGuiController(_window.ClientSize.X, _window.ClientSize.Y); };
        VSync = initSettings.VSync;
        isMultiThreaded = initSettings.IsGame;
        SetupEventsQueue();
        DebugTimer.End();
    }

    private void SetupEventsQueue()
    {
        DebugTimer.Start();
        _window.MouseMove += (e) => { _events.AddEvent(new MouseMovedEvent(e.X, e.Y, e.DeltaX, e.DeltaY)); };
        _window.Resize += (e) =>
        {
            _events.AddEvent(new WindowResizedEvent(e.Width, e.Height));
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
        DebugTimer.End();
    }

    private readonly EventQueue _events;
    private Thread _renderThread;
    private Stopwatch _watchRender;
    private Stopwatch _watchUpdate;
    private double _updateFrequency;

    public override void Init()
    {
        Context.Init();
        _watchRender = new Stopwatch();
        _watchUpdate = new Stopwatch();
        Context.MakeCurrent();
        _events.AddEvent(new WindowResizedEvent(Size.Width, Size.Height));
    }

    public override unsafe void Run(Action updateLoop, Action renderLoop)
    {
        UpdateLoop = updateLoop;
        RenderLoop = renderLoop;
        _watchUpdate.Start();
        _watchRender.Start();
        while (!GLFW.WindowShouldClose(_window.WindowPtr))
        {
            double val1 = this.DispatchUpdateFrame();
            double val2 = this.DispatchRenderFrame();
            val1 = Math.Min(val1, val2);
            if (val1 > 0.0)
                Thread.Sleep((int) Math.Floor(val1 * 1000.0));
        }
    }
    public override unsafe void RunMultiThreaded(Action updateLoop, Action renderLoop)
    {
        UpdateLoop = updateLoop;
        RenderLoop = renderLoop;
        RunRenderThread();
        _watchUpdate.Start();
        while (!GLFW.WindowShouldClose(_window.WindowPtr))
        {
            double val1 = this.DispatchUpdateFrame();
            if (val1 > 0.0)
                Thread.Sleep((int) Math.Floor(val1 * 1000.0));
        }
    }

    public bool isMultiThreaded { get; init; }

    private void RunRenderThread()
    {
        Context.MakeNonCurrent();
        this._renderThread = new Thread(this.StartRenderThread);
    }
    private unsafe void StartRenderThread()
    {
        Context.MakeCurrent();
        this._watchRender.Start();
        while (!GLFW.WindowShouldClose(_window.WindowPtr))
            this.DispatchRenderFrame();
    }

    private Action UpdateLoop;
    private double _updateEpsilon;
    /// <returns>Time to next update frame.</returns>
    private double DispatchUpdateFrame()
    {
        DebugTimer.Start();
        int num1 = 4;
        double totalSeconds = this._watchUpdate.Elapsed.TotalSeconds;
        double num2;
        for (num2 = this.UpdateFrequency == 0.0 ? 0.0 : 1.0 / this.UpdateFrequency; totalSeconds > 0.0 && totalSeconds + this._updateEpsilon >= num2; totalSeconds = this._watchUpdate.Elapsed.TotalSeconds)
        {
            _window.ProcessInputEvents();
            NativeWindow.ProcessWindowEvents(_window.IsEventDriven);
            this._watchUpdate.Restart();
            this.UpdateTime = totalSeconds;
            Time.Update();
            UpdateLoop.Invoke();
            this._updateEpsilon += totalSeconds - num2;
            if (this.UpdateFrequency > double.Epsilon)
            {
                this.IsRunningSlowly = this._updateEpsilon >= num2;
                if (this.IsRunningSlowly && --num1 == 0)
                {
                    this._updateEpsilon = 0.0;
                    break;
                }
            }
            else
                break;
        }
        DebugTimer.End();
        return this.UpdateFrequency != 0.0 ? num2 - totalSeconds : 0.0;
    }

    public bool IsRunningSlowly { get; private set; }
    private Action RenderLoop;

    /// <returns>Time to next render frame.</returns>
    private double DispatchRenderFrame()
    {
        DebugTimer.Start();
        double totalSeconds = this._watchRender.Elapsed.TotalSeconds;
        double num = this.RenderFrequency == 0.0 ? 0.0 : 1.0 / this.RenderFrequency;
        if (totalSeconds > 0.0 && totalSeconds >= num)
        {
            this._watchRender.Restart();
            this.RenderTime = totalSeconds;
            RenderLoop.Invoke();
            if (this.VSync == VSync.Adaptive)
                Context.SwapInterval = this.IsRunningSlowly ? 0 : 1;
        }
        
        //GL.Viewport(0,0, _window.Size.X, _window.Size.Y);
        Context.SwapBuffers();
        DebugTimer.End();
        return this.RenderFrequency != 0.0 ? num - totalSeconds : 0.0;
    }

    public override void ReleaseUnmanagedResources()
    {
        Context.Destroy();
    }

    internal NativeWindow GetWindow()
    {
        return _window;
    }
}