using BeeEngine.OpenTK.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.OpenTK.Platform.OpenGL;

internal class OpenGLIndexBuffer: IndexBuffer
{
    public OpenGLIndexBuffer(uint[] indices)
    {
        Count = indices.Length;
        if (Application.PlatformOS == OS.Mac)
        {
            RendererID = GL.GenBuffer();
        }
        else
        {
            int rendererId;
            GL.CreateBuffers(1, out rendererId);
            RendererID = rendererId;
        }
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, RendererID);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Count * sizeof(uint), indices, BufferUsageHint.StaticDraw);
    }

    public override void Bind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, RendererID);
    }

    public override void Unbind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    protected override void Dispose(bool disposing)
    {
        GL.DeleteBuffer(RendererID);
    }
}