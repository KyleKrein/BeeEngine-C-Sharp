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

public enum DrawingFrequency
{
    
}