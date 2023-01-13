using System.Diagnostics;
using BeeEngine.Drawing;
using BeeEngine.Events;
using BeeEngine.Platform.OpenGL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using InvalidOperationException = System.InvalidOperationException;

namespace BeeEngine;

internal class CrossPlatformWindow:Window, IDisposable
{
    public static CrossPlatformWindow Instance { get; private set; }
    public override string Title
    {
        get => _window.Title;
        set => _window.Title = value;
    }

    public override int Width
    {
        get => _window.Width;
        set => _window.Width = value;
    }

    public override int Height
    {
        get => _window.Height;
        set => _window.Height = value;
    }
    
    private VSync _vSync = VSync.On;
    public override VSync VSync
    {
        get => _vSync;
        set
        {
            _window.VSync = value;
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
        get => new Size(_window.Width, _window.Height);
        set
        {
            _window.Width = value.Width;
            _window.Height = value.Height;
        }
    }

    private WindowHandler _window;
    public CrossPlatformWindow(WindowProps initSettings, EventQueue eventQueue)
    {
        DebugTimer.Start();
        if (Instance != null)
        {
            var message = "Can't have two instances of the game";
            Log.Error(message);
            throw new InvalidOperationException(message);
        }

        Instance = this;
        DebugTimer.Start("Creating window");
        
        _window = WindowHandler.Create(WindowHandlerType.GLFW, ref initSettings, eventQueue);
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
        _events.AddEvent(new WindowResizedEvent(Width, Height));
    }

    public override unsafe void Run(Action updateLoop, Action renderLoop)
    {
        UpdateLoop = updateLoop;
        RenderLoop = renderLoop;
        _watchUpdate.Start();
        _watchRender.Start();
        //RenderCommand.SetViewPort(0,0, Width, Height);
        while (_window.IsRunning())
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
        while (_window.IsRunning())
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
        while (_window.IsRunning())
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
            _window.ProcessEvents();
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

    internal WindowHandler GetWindow()
    {
        return _window;
    }
}