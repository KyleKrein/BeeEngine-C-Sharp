using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace BeeEngine.OpenTK;

public sealed class Window: GameWindow

{
    public Window(string title, int width, int height) : base(new GameWindowSettings(),
        new NativeWindowSettings() {Flags = ContextFlags.ForwardCompatible})
    {
        
    }
    internal Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        SwapBuffers();
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(Color4.CornflowerBlue);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}