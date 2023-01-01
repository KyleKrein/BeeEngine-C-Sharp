namespace BeeEngine.OpenTK.Renderer;

public abstract class Shader: IDisposable
{
    public static Shader Create(string vertexSrc, string fragmentSrc)
    {
        return Renderer.RendererAPI switch
        {
            RendererAPI.OpenGL => new OpenGlShader(vertexSrc, fragmentSrc),
            _ => throw new PlatformNotSupportedException()
        };
    }

    public static Shader FromFiles(string vertexPath, string fragmentPath)
    {
        string vertexShaderSource = File.ReadAllText(vertexPath);

        string fragmentShaderSource = File.ReadAllText(fragmentPath);
        return Create(vertexShaderSource, fragmentShaderSource);
    }
    public abstract void Bind();

    public abstract void Unbind();

    protected abstract void Dispose(bool disposing);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Shader()
    {
        Dispose(false);
    }
}