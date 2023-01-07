using BeeEngine;
using BeeEngine.Platform.OpenGL;
using NotSupportedException = System.NotSupportedException;

namespace BeeEngine;

public abstract class VertexArray: IDisposable
{
    public List<VertexBuffer> VertexBuffers { get; protected set; } = new List<VertexBuffer>();
    public IndexBuffer IndexBuffer { get; protected set; }
    public abstract BufferLayout Layout { get; }
    public abstract void Bind();
    public abstract void Unbind();
    public abstract void AddVertexBuffer(VertexBuffer buffer);
    public abstract void SetIndexBuffer(IndexBuffer buffer);
    public static VertexArray Create()
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGLVertexArray();
            case API.None:
                Log.Error("{0} is not supported", Renderer.API);
                throw new NotSupportedException();
            default:
                Log.Error("Unknown Renderer API is not supported");
                throw new NotSupportedException();
        }
    }

    protected abstract void Dispose(bool disposing);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~VertexArray()
    {
        Dispose(false);
    }
}