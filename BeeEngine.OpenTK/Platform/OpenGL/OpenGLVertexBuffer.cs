using BeeEngine.OpenTK.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK.Platform.OpenGL;

internal class OpenGLVertexBuffer: VertexBuffer
{
    private uint _rendererId;
    public OpenGLVertexBuffer(float[] vertices, DrawingFrequency frequency)
    {
        DebugTimer.Start();
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
        DebugTimer.End();
    }

    public override void Bind()
    {
        DebugTimer.Start("OpenGLVertexBuffer.Bind()");
        GL.BindBuffer(BufferTarget.ArrayBuffer, _rendererId);
        DebugTimer.End("OpenGLVertexBuffer.Bind()");
    }

    public override void Unbind()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }

    protected override void Dispose(bool disposing)
    {
        GL.DeleteBuffer(_rendererId);
    }
}