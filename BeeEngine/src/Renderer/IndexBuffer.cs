using BeeEngine;
using BeeEngine.Platform.Metal;
using BeeEngine.Platform.OpenGL;
using BeeEngine.SmartPointers;
using NotSupportedException = System.NotSupportedException;

namespace BeeEngine;

public abstract class IndexBuffer: IDisposable
{
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

    public int Count { get; protected init; }
    public static IndexBuffer Create(uint[] indices)
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGLIndexBuffer(indices);
            case API.Metal:
                #if IOS
                return new MetalIndexBuffer(indices);
                #endif
            case API.None:
                Log.Error("{0} is not supported", Renderer.API);
                throw new NotSupportedException();
            default:
                Log.Error("Unknown Renderer API is not supported");
                throw new NotSupportedException();
        }
    }

    public abstract void Bind();
    public abstract void Unbind();

    protected abstract void Dispose(bool disposing);

    public void Dispose()
    {
        Dispose(true);
        _rendererId.Dispose();
        GC.SuppressFinalize(this);
    }

    ~IndexBuffer()
    {
        Dispose(false);
        _rendererId.Dispose();
    }
}