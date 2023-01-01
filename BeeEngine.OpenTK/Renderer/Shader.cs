using BeeEngine.OpenTK.Platform.OpenGL;

namespace BeeEngine.OpenTK.Renderer;

public abstract class Shader: IDisposable
{
    public static Shader Create(string vertexSrc, string fragmentSrc)
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGlShader(vertexSrc, fragmentSrc);
            case API.None:
                Log.Error("{0} is not supported", Renderer.API);
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