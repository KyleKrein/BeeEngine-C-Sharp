using BeeEngine.OpenTK.Events;
using BeeEngine.OpenTK.Renderer;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace BeeEngine.OpenTK.Platform.OpenGL;

public class OpenGLTexture2D: Texture2D
{
    private readonly string _path;
    private int _rendererId;
    private PixelInternalFormat _internalFormat;
    
    public OpenGLTexture2D(string path)
    {
        path = ResourceManager.ProcessFilePath(path);
        //_rendererId = GL.GenTexture();
        _path = path;
        // stb_image loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
        // This will correct that, making the texture display properly.
        StbImage.stbi_set_flip_vertically_on_load(1);
        ImageResult image;
        using (var stream = File.OpenRead(path))
        {
            image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        }

        if (Application.PlatformOS == OS.Mac)
        {
            _rendererId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _rendererId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 
                0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        else
        {
            GL.CreateTextures(TextureTarget.Texture2D, 1, out _rendererId);
            GL.TextureStorage2D(_rendererId, 1, SizedInternalFormat.Rgba32f, image.Width, image.Height);
            GL.TextureSubImage2D(_rendererId, 0, 0, 0, image.Width, image.Height, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.GenerateTextureMipmap(_rendererId);
        }
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        
        Width = image.Width;
        Height = image.Height;
    }

    public OpenGLTexture2D(uint width, uint height)
    {
        Width = (int) width;
        Height = (int) height;
        if (Application.PlatformOS == OS.Mac)
        {
            _rendererId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _rendererId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 
                0, PixelFormat.Rgba, PixelType.UnsignedByte, nint.Zero);
           // GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        else
        {
            GL.CreateTextures(TextureTarget.Texture2D, 1, out _rendererId);
            GL.TextureStorage2D(_rendererId, 1, SizedInternalFormat.Rgba32f, Width, Height);
            //GL.TextureSubImage2D(_rendererId, 0, 0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            //GL.GenerateTextureMipmap(_rendererId);
        }
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
    }
    public override void Bind(int slot = 0)
    {
        if (Application.PlatformOS == OS.Mac)
        {
            DebugLog.Assert(slot<=32, "Could not bind texture to slot {0}", slot);
            GL.ActiveTexture(_textureUnits[slot]);
            GL.BindTexture(TextureTarget.Texture2D, _rendererId);
            return;
        }
        GL.BindTextureUnit(slot, _rendererId);
        //
    }

    public override void SetData(byte[] data, uint size)
    {
        Log.AssertAndThrow(size == Width * Height * 4, "Data must fill the entire texture!");
        if (Application.PlatformOS == OS.Mac)
        {
            GL.BindTexture(TextureTarget.Texture2D, _rendererId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 
                0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return;
        }
        GL.TextureSubImage2D(_rendererId, 0, 0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        GL.GenerateTextureMipmap(_rendererId);
    }

    public override void SetData(nint data, uint size)
    {
        Log.AssertAndThrow(size == Width * Height * 4, "Data must fill the entire texture!");
        if (Application.PlatformOS == OS.Mac)
        {
            GL.BindTexture(TextureTarget.Texture2D, _rendererId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 
                0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return;
        }
        GL.TextureSubImage2D(_rendererId, 0, 0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        GL.GenerateTextureMipmap(_rendererId);
    }

    private static TextureUnit[] _textureUnits = Enum.GetValues<TextureUnit>();

    protected override void Dispose(bool disposing)
    {
        GL.DeleteTexture(_rendererId);
    }
}