using System.Diagnostics;
using BeeEngine.Mathematics;
using BeeEngine.OpenTK.Events;
using BeeEngine.OpenTK.Platform.OpenGL;

namespace BeeEngine.OpenTK.Renderer;

[DebuggerDisplay("{Name}")]
public abstract class Shader: IDisposable
{
    public string Name { get; internal set; }
    public static Shader Create(string filepath)
    {
        filepath = ResourceManager.ProcessFilePath(filepath);
        var name = ResourceManager.GetNameFromPath(filepath);
        return Create(name, filepath);
    }

    public static Shader Create(string name, string filepath)
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGlShader(name, filepath);
            case API.None:
                Log.Error("{0} is not supported", Renderer.API);
                throw new NotSupportedException();
            default:
                Log.Error("Unknown Renderer API is not supported");
                throw new NotSupportedException();
        }
    }
    public static Shader Create(string name, string vertexSrc, string fragmentSrc)
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGlShader(name ,vertexSrc, fragmentSrc);
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