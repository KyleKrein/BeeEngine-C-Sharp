using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK.Renderer;

internal class OpenGLVertexBuffer: VertexBuffer
{
    private uint _rendererId;
    public OpenGLVertexBuffer(float[] vertices, DrawingFrequency frequency)
    {
        Count = vertices.Length;
        GL.CreateBuffers(1, out _rendererId);
        GL.BufferData(BufferTarget.ArrayBuffer, Count*sizeof(float), vertices, BufferUsageHint.StaticDraw);
    }

    public override void Bind()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, _rendererId);
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