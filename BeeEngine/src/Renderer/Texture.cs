using BeeEngine;
using BeeEngine.Platform.Metal;
using BeeEngine.Platform.OpenGL;
using BeeEngine.SmartPointers;

namespace BeeEngine;

public abstract class Texture: IDisposable
{
    private SharedPointer<RendererID> _rendererId = new SharedPointer<RendererID>();

    public unsafe SharedPointer<RendererID> RendererID
    {
        get => _rendererId.Share();
        protected set
        {
            *_rendererId.GetPtr() = *value.GetPtr();
            value.Release();
        }
    }
    public int Width { get; protected set; }
    public int Height { get; protected set; }

    public abstract void Bind(int slot = 0);

    protected abstract void Dispose(bool disposing);

    public void Dispose()
    {
        Dispose(true);
        _rendererId.Dispose();
        GC.SuppressFinalize(this);
    }

    ~Texture()
    {
        Dispose(false);
        _rendererId.Dispose();
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