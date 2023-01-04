using BeeEngine.OpenTK.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK.Platform.OpenGL;

internal class OpenGLVertexBuffer: VertexBuffer
{
    private uint _rendererId;
    
    public OpenGLVertexBuffer(int size)
    {
        DebugTimer.Start(".ctor (int size)");
        Count = size/sizeof(float);
        if (Application.PlatformOS == OS.Mac)
        {
            GL.GenBuffers(1, out _rendererId);
        }
        else
        {
            GL.CreateBuffers(1, out _rendererId);
        }
        GL.BindBuffer(BufferTarget.ArrayBuffer, _rendererId);
        GL.BufferData(BufferTarget.ArrayBuffer, size, nint.Zero, BufferUsageHint.DynamicDraw);
        DebugTimer.End(".ctor (int size)");
    }
    public OpenGLVertexBuffer(float[] vertices)
    {
        DebugTimer.Start(".ctor (float[] vertices)");
        Count = vertices.Length;
        if (Application.PlatformOS == OS.Mac)
        {
            GL.GenBuffers(1, out _rendererId);
        }
        else
        {
            GL.CreateBuffers(1, out _rendererId);
        }
        GL.BindBuffer(BufferTarget.ArrayBuffer, _rendererId);
        GL.BufferData(BufferTarget.ArrayBuffer, Count*sizeof(float), vertices, BufferUsageHint.StaticDraw);
        DebugTimer.End(".ctor (float[] vertices)");
    }

    

    public override void Bind()
    {
        DebugTimer.Start();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _rendererId);
        DebugTimer.End();
    }

    public override void Unbind()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }

    protected override void Dispose(bool disposing)
    {
        GL.DeleteBuffer(_rendererId);
    }

    public override void SetData(IntPtr data, int size)
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, _rendererId);
        GL.BufferSubData(BufferTarget.ArrayBuffer, 0, size ,data);
    }
}