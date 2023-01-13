using BeeEngine;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GL = OpenTK.Graphics.OpenGL4.GL;
using StringName = OpenTK.Graphics.OpenGL4.StringName;

namespace BeeEngine.Platform.OpenGL;
internal class OpenGLContext: Context
{
    private WindowHandler _window;
    private int _swapInterval;
    public unsafe OpenGLContext(WindowHandler window)
    {
        _window = window;
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
        _window.SwapBuffers();
        DebugTimer.End();
    }

    public override unsafe void MakeCurrent()
    {
        _window.MakeContextCurrent();
    }

    public override unsafe void MakeNonCurrent()
    {
        _window.MakeContextNonCurrent();
    }

    public override void Destroy()
    {
        _window.Dispose();
    }
}