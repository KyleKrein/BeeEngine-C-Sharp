using BeeEngine.OpenTK.Platform.OpenGL;

namespace BeeEngine.OpenTK.Renderer;

public abstract class Texture: IDisposable
{
    public int Width { get; protected set; }
    public int Height { get; protected set; }

    public abstract void Bind(int slot = 0);

    protected abstract void Dispose(bool disposing);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Texture()
    {
        Dispose(false);
    }
}

public abstract class Texture2D: Texture
{
    public static Texture2D CreateFromFile(string path)
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGLTexture2D(path);
        }
        Log.Error("Unknown RendererAPI!");
        throw new PlatformNotSupportedException();
    }
}