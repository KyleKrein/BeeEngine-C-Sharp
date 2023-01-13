using BeeEngine;
using OpenTK.Graphics.OpenGL4;

namespace BeeEngine.Platform.OpenGL;

internal class OpenGLIndexBuffer: IndexBuffer
{
    public OpenGLIndexBuffer(uint[] indices)
    {
        DebugTimer.Start();
        Count = indices.Length;
        if (Application.PlatformOS == OS.Mac)
        {
            RendererID.GetRef() = GL.GenBuffer();
        }
        else
        {
            GL.CreateBuffers(1, out RendererID.GetRef()._id);
        }
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, RendererID.GetRef());
        GL.BufferData(BufferTarget.ElementArrayBuffer, Count * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        DebugTimer.End();
    }

    public override void Bind()
    {
        DebugTimer.Start("OpenGLIndexBuffer.Bind()");
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, RendererID.GetRef());
        DebugTimer.End("OpenGLIndexBuffer.Bind()");
    }

    public override void Unbind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    protected override void Dispose(bool disposing)
    {
        GL.DeleteBuffer(RendererID.GetRef());
    }
}