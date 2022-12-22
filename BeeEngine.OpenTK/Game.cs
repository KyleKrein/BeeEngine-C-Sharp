using BeeEngine.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace BeeEngine.OpenTK;

public abstract class Game: IDisposable
{
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
        _nativeWindowSettings.Flags = ContextFlags.ForwardCompatible;
        _nativeWindowSettings.Title = title;
        _nativeWindowSettings.Size = new Vector2i(width, height);
        _window = new GameWindow(_gameWindowSettings, _nativeWindowSettings);
        Time gameTime = new Time();
        
        _window.Load += LoadContent;
        _window.Load += Initialize;
        _window.Load += () => GL.ClearColor(Color4.CornflowerBlue);
        _window.Unload += UnloadResources;

        _window.Resize += (e) => { GL.Viewport(0, 0, e.Width, e.Height); };
        
        _window.UpdateFrame += e =>
        {
            gameTime.TotalTime += TimeSpan.FromMilliseconds(e.Time);
            gameTime.ElapsedTime = TimeSpan.FromMilliseconds(e.Time);
            InitializeGameObjects(gameTime);
            Update(gameTime);
            UpdateGameObjects(gameTime);
            LateUpdateGameObjects(gameTime);
        };
        _window.RenderFrame += e =>
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Render(gameTime);
            _window.SwapBuffers();
        };
        _window.Run();
    }
    
    public void Run()
    {
        
    }

    protected abstract void UnloadResources();

    private void UpdateGameObjects(Time gameTime)
    {
        
    }

    private void LateUpdateGameObjects(Time gameTime)
    {
        
    }

    private void InitializeGameObjects(Time gameTime)
    {
        
    }
    
    protected abstract void Initialize();
    protected abstract void LoadContent();
    protected abstract void Update(Time gameTime);
    protected abstract void Render(Time gameTime);

    public void Dispose()
    {
        _window.Dispose();
    }
}