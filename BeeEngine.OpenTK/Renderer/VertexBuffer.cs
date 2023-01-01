using BeeEngine.OpenTK.Platform.OpenGL;
using NotSupportedException = System.NotSupportedException;

namespace BeeEngine.OpenTK.Renderer;

public abstract class VertexBuffer: IDisposable
{
    public int Count { get; protected init; }
    private BufferLayout _layout;
    public BufferLayout Layout
    {
        get => _layout;
        set
        {
            if (!value.IsReadOnly)
            {
                value.Build();
            }

            _layout = value;
        }
    }

    public static VertexBuffer Create(float[] vertices, DrawingFrequency frequency)
    {
        switch (Renderer.RendererAPI)
        {
            case RendererAPI.OpenGL:
                return new OpenGLVertexBuffer(vertices, frequency);
            case RendererAPI.None:
                Log.Error("{0} is not supported", Renderer.RendererAPI);
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
        GC.SuppressFinalize(this);
    }

    ~VertexBuffer()
    {
        Dispose(false);
    }
}

public enum DrawingFrequency
{
    
}