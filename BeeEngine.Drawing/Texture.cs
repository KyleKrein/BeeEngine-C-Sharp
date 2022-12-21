using SDL2;

namespace BeeEngine.Drawing;

public sealed class Texture: IDisposable
{
    internal readonly IntPtr _texture;
    public readonly int Width;
    public readonly int Height;
    internal SDL.SDL_Rect TextureRect;
    public static Texture FromFile(string path)
    {
        var texture = Renderer.LoadTexture(path);
        return new Texture(texture);
    }

    public static Texture FromTexture(Texture texture)
    {
        Texture newTexture = new Texture(texture.Width, texture.Height);
        using (Graphics g = Graphics.FromTexture(newTexture))
        {
            g.DrawTexture(texture, 0,0);
        }

        return newTexture;
    }
    public static Texture FromTexture(Texture texture, int width, int height)
    {
        Texture newTexture = new Texture(width, height);
        using (Graphics g = Graphics.FromTexture(newTexture))
        {
            g.DrawTexture(texture, 0,0, width, height);
        }

        return newTexture;
    }

    public Texture(int width, int height)
    {
        _texture = SDL.SDL_CreateTexture(Renderer.GetRenderer(), SDL.SDL_PIXELFORMAT_RGBA8888,
            (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, width, height);
        Width = width;
        Height = height;
        TextureRect = new SDL.SDL_Rect() {x = 0, y = 0, w = Width, h = Height};
    }

    private Texture(IntPtr texture)
    {
        _texture = texture;
        SDL.SDL_QueryTexture(_texture,out _, out _, out Width,  out Height);
        TextureRect = new SDL.SDL_Rect() {x = 0, y = 0, w = Width, h = Height};
    }

    private void ReleaseUnmanagedResources()
    {
        SDL.SDL_DestroyTexture(_texture);
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Texture()
    {
        ReleaseUnmanagedResources();
    }
}