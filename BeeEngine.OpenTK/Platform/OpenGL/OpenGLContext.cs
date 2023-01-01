using BeeEngine.OpenTK.Renderer;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace BeeEngine.OpenTK.Platform.OpenGL;
internal class OpenGLContext: Context
{
    private NativeWindow _window;
    private unsafe global::OpenTK.Windowing.GraphicsLibraryFramework.Window* _windowPtr;
    private int _swapInterval;
    public unsafe global::OpenTK.Windowing.GraphicsLibraryFramework.Window* WindowPtr => _windowPtr;
    public unsafe OpenGLContext(NativeWindow window)
    {
        _window = window;
        _windowPtr = window.WindowPtr;
    }
    public override int SwapInterval
    {
        get => this._swapInterval;
        set
        {
            GLFW.SwapInterval(value);
            this._swapInterval = value;
        }
    }

    public override void Init()
    {
        
    }

    public override unsafe void SwapBuffers()
    {
        GLFW.SwapBuffers(this._windowPtr);
    }

    public override unsafe void MakeCurrent()
    {
        GLFW.MakeContextCurrent(_windowPtr);
    }

    public override unsafe void MakeNonCurrent()
    {
        GLFW.MakeContextCurrent(null);
    }

    public override void Destroy()
    {
        _window.Dispose();
    }
}