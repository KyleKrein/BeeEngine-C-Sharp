using BeeEngine.Mathematics;
using BeeEngine.OpenTK.Platform.OpenGL;

namespace BeeEngine.OpenTK.Renderer;

public abstract class Shader: IDisposable
{
    
    public static Shader Create(string filepath)
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGlShader(filepath);
            case API.None:
                Log.Error("{0} is not supported", Renderer.API);
                throw new NotSupportedException();
            default:
                Log.Error("Unknown Renderer API is not supported");
                throw new NotSupportedException();
        }
    }
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
    public abstract void Bind();

    public abstract void Unbind();
    public abstract void UploadUniformMatrix4(string name, ref Matrix4 matrix4);
    public abstract void UploadUniformFloat(string name, float value);
    public abstract void UploadUniformFloat2(string name, Vector2 vector);
    public abstract void UploadUniformFloat3(string name, Vector3 vector);
    public abstract void UploadUniformFloat4(string name, Vector4 vector);
    public abstract void UploadUniformInt(string name, int value);
    public abstract void UploadUniformInt2(string name, Vector2i vector);
    public abstract void UploadUniformInt3(string name, Vector3i vector);
    public abstract void UploadUniformInt4(string name, Vector4i vector);

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