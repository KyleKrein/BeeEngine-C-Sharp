namespace BeeEngine.OpenTK.Renderer;
internal abstract class VertexBuffer: IDisposable
{
    public int Count { get; protected init; }
    public static VertexBuffer Create(float[] vertices, DrawingFrequency frequency)
    {
        return Renderer.RendererAPI switch
        {
            RendererAPI.OpenGL => new OpenGLVertexBuffer(vertices, frequency),
            RendererAPI.None => throw new NotSupportedException(
                "Can't create Vertex Buffers if Renderer API is not assigned"),
            _ => throw new ArgumentOutOfRangeException(typeof(RendererAPI).ToString())
        };
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

internal enum DrawingFrequency
{
    
}