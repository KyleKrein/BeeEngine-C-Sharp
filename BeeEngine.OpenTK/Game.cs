using BeeEngine.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace BeeEngine.OpenTK;

public abstract class Game: IDisposable
{
    public string Title { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    
    public Point Location { get; set; }
    public Size Size { get; set; }

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
            GL.ClearColor(Color4.CornflowerBlue);
            Render(gameTime);
            _window.SwapBuffers();
        };
        _window.Run();
    }
    
    public void Run()
    {
        
    }

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