using BeeEngine;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GL = OpenTK.Graphics.OpenGL4.GL;
using StringName = OpenTK.Graphics.OpenGL4.StringName;

namespace BeeEngine.Platform.OpenGL;
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
        var vendor = GL.GetString(StringName.Vendor);
        var renderer = GL.GetString(StringName.Renderer);
        var version = GL.GetString(StringName.Version);
        Log.Info("OpenGL Context is initialized.\nGPU: {0} {1}\nVersion {2}", vendor, renderer, version);
    }

    public override unsafe void SwapBuffers()
    {
        DebugTimer.Start();
        GLFW.SwapBuffers(this._windowPtr);
        DebugTimer.End();
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