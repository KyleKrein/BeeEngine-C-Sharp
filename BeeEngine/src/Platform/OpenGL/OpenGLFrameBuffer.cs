using BeeEngine.SmartPointers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace BeeEngine.OpenTK.Platform.OpenGL;

public sealed class OpenGLFrameBuffer: FrameBuffer
{
    internal OpenGLFrameBuffer(SharedPointer<FrameBufferPreferences> preferences)
    {
        m_preferences = preferences;
        Invalidate();
    }
    internal OpenGLFrameBuffer(ref FrameBufferPreferences preferences)
    {
        m_preferences = new SharedPointer<FrameBufferPreferences>(ref preferences);
        Invalidate();
    }

    public override void Invalidate()
    {
        if (RendererID.GetRef()._id != 0)
        {
            GL.DeleteFramebuffer(RendererID.Get());
            GL.DeleteTexture(ColorAttachment);
            GL.DeleteTexture(DepthAttachment);
        }
        RendererID.GetRef() = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, RendererID.GetRef());
        ColorAttachment = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, ColorAttachment);
        GL.TexImage2D(TextureTarget.Texture2D, 0, 
            PixelInternalFormat.Rgba32f, (int) m_preferences.GetRef().Width, 
            (int) m_preferences.GetRef().Height, 0, PixelFormat.Rgba, 
            PixelType.UnsignedByte, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, 
            TextureTarget.Texture2D, ColorAttachment, 0);

        DepthAttachment = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, DepthAttachment);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, 
            (int) m_preferences.GetRef().Width, (int) m_preferences.GetRef().Height, 0, 
            PixelFormat.DepthStencil, PixelType.UnsignedInt248, IntPtr.Zero);
        
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, 
            FramebufferAttachment.DepthAttachment, 
            TextureTarget.Texture2D,  DepthAttachment, 0);
        
        DebugLog.Assert(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete, "Framebuffer creation failed");
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

    }

    public override void Bind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, RendererID.GetRef());
        GL.Viewport(0,0, (int) m_preferences.GetRef().Width, (int) m_preferences.GetRef().Height);
    }

    public override void Unbind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        //GL.Viewport(0,0, Application.Instance.Width*2, Application.Instance.Height*2);
    }

    protected override void Dispose(bool disposing)
    {
        GL.DeleteFramebuffer(RendererID.Get());
        GL.DeleteTexture(ColorAttachment);
        GL.DeleteTexture(DepthAttachment);
    }
}