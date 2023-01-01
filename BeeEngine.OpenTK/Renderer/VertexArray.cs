using BeeEngine.OpenTK.Platform.OpenGL;
using NotSupportedException = System.NotSupportedException;

namespace BeeEngine.OpenTK.Renderer;

public abstract class VertexArray: IDisposable
{
    public abstract BufferLayout Layout { get; }
    public abstract void Bind();
    public abstract void Unbind();
    public abstract void AddVertexBuffer(VertexBuffer buffer);
    public abstract void SetIndexBuffer(IndexBuffer buffer);
    public static VertexArray Create()
    {
        switch (Renderer.RendererAPI)
        {
            case RendererAPI.OpenGL:
                return new OpenGLVertexArray();
            case RendererAPI.None:
                Log.Error("{0} is not supported", Renderer.RendererAPI);
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