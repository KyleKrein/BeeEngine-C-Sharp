using BeeEngine;
using BeeEngine.Platform.Metal;
using BeeEngine.Platform.OpenGL;
using NotSupportedException = System.NotSupportedException;

namespace BeeEngine;

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
    public static VertexBuffer Create(int size)
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGLVertexBuffer(size);
            case API.Metal:
                return new MetalVertexBuffer(size);
            case API.None:
                Log.Error("{0} is not supported", Renderer.API);
                throw new NotSupportedException();
            default:
                Log.Error("Unknown Renderer API is not supported");
                throw new NotSupportedException();
        }
    }
    public static VertexBuffer Create(float[] vertices)
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGLVertexBuffer(vertices);
            case API.Metal:
                return new MetalVertexBuffer(vertices);
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
        GC.SuppressFinalize(this);
    }

    ~VertexBuffer()
    {
        Dispose(false);
    }

    public abstract void SetData(IntPtr data, int size);
}

public enum DrawingFrequency
{
    
}