using BeeEngine;
using BeeEngine.Platform.Metal;
using BeeEngine.Platform.OpenGL;

namespace BeeEngine;

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
    public abstract void SetData(byte[] data, uint size);
    public abstract void SetData(IntPtr data, uint size);
    public static Texture2D Create(uint width, uint height)
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGLTexture2D(width, height);
            case API.Metal:
                return new MetalTexture2D(width, height);
        }
        Log.Error("Unknown RendererAPI!");
        throw new PlatformNotSupportedException();
    }
    public static Texture2D CreateFromFile(string path)
    {
        switch (Renderer.API)
        {
            case API.OpenGL:
                return new OpenGLTexture2D(path);
            case API.Metal:
                return new MetalTexture2D(path);
        }
        Log.Error("Unknown RendererAPI!");
        throw new PlatformNotSupportedException();
    }

    
}