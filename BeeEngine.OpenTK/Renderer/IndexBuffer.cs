namespace BeeEngine.OpenTK.Renderer;

public abstract class IndexBuffer: IDisposable
{
    public int Count { get; protected init; }
    public static IndexBuffer Create(uint[] indecis)
    {
        return Renderer.RendererAPI switch
        {
            RendererAPI.OpenGL => new OpenGLIndexBuffer(indecis),
            RendererAPI.None => throw new NotSupportedException(),
            _ => throw new NotSupportedException()
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

    ~IndexBuffer()
    {
        Dispose(false);
    }
}