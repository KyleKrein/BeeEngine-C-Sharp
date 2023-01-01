using BeeEngine.OpenTK.Platform.OpenGL;

namespace BeeEngine.OpenTK.Renderer;

public abstract class Shader: IDisposable
{
    public static Shader Create(string vertexSrc, string fragmentSrc)
    {
        switch (Renderer.RendererAPI)
        {
            case RendererAPI.OpenGL:
                return new OpenGlShader(vertexSrc, fragmentSrc);
            case RendererAPI.None:
                Log.Error("{0} is not supported", Renderer.RendererAPI);
                throw new NotSupportedException();
            default:
                Log.Error("Unknown Renderer API is not supported");
                throw new NotSupportedException();
        }
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