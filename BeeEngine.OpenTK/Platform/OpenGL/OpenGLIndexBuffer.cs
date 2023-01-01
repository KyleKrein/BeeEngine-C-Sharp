using BeeEngine.OpenTK.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK.Platform.OpenGL;

internal class OpenGLIndexBuffer: IndexBuffer
{
    private uint _rendererId;
    public OpenGLIndexBuffer(uint[] indices)
    {
        Count = indices.Length;
        GL.CreateBuffers(1, out _rendererId);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Count * sizeof(uint), indices, BufferUsageHint.StaticDraw);
    }

    public override void Bind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _rendererId);
    }

    public override void Unbind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    protected override void Dispose(bool disposing)
    {
        GL.DeleteBuffer(_rendererId);
    }
}