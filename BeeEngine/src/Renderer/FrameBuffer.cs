using BeeEngine.OpenTK.Platform.OpenGL;
using BeeEngine.SmartPointers;

namespace BeeEngine;

public abstract class FrameBuffer: IDisposable
{
    protected SharedPointer<FrameBufferPreferences> m_preferences;
    public RendererID ColorAttachment;
    public RendererID DepthAttachment;

    private SharedPointer<RendererID> _rendererId = new SharedPointer<RendererID>();

    public unsafe SharedPointer<RendererID> RendererID
    {
        get => _rendererId.Share();
        protected set
        {
            *_rendererId.GetPtr() = *value.GetPtr();
            value.Release();
        }
    }

    public static FrameBuffer Create(ref FrameBufferPreferences preferences)
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGLFrameBuffer(ref preferences);
            case API.Metal:
#if IOS
                //return new MetalFrameBuffer(ref preferences);
#endif
            case API.None:
                Log.Error("{0} is not supported", Renderer.API);
                throw new NotSupportedException();
            default:
                Log.Error("Unknown Renderer API is not supported");
                throw new NotSupportedException();
        }
    }
    public static FrameBuffer Create(SharedPointer<FrameBufferPreferences> preferences)
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGLFrameBuffer(preferences);
            case API.Metal:
#if IOS
            //return new MetalFrameBuffer(ref preferences);
#endif
            case API.None:
                Log.Error("{0} is not supported", Renderer.API);
                throw new NotSupportedException();
            default:
                Log.Error("Unknown Renderer API is not supported");
                throw new NotSupportedException();
        }
    }

    public abstract void Invalidate();
    public abstract void Bind();
    public abstract void Unbind();

    protected abstract void Dispose(bool disposing);

    public void Dispose()
    {
        Dispose(true);
        _rendererId.Dispose();
        GC.SuppressFinalize(this);
    }

    ~FrameBuffer()
    {
        Dispose(false);
    }
}

public struct FrameBufferPreferences
{
    public uint Width, Height;
    public uint Samples;
    public bool SwapChainTarget;

    public FrameBufferPreferences()
    {
        Width = 1280;
        Height = 720;
        Samples = 1;
        SwapChainTarget = false;
    }

    public FrameBufferPreferences(uint width = 1280, uint height = 720, uint samples = 1, bool swapChainTarget = false)
    {
        Width = width;
        Height = height;
        Samples = samples;
        SwapChainTarget = swapChainTarget;
    }
}