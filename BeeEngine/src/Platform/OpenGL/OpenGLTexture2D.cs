using BeeEngine;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace BeeEngine.Platform.OpenGL;

public class OpenGLTexture2D: Texture2D
{
    private readonly string _path;
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
            RendererID.GetRef() = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, RendererID.GetRef());
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 
                0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        else
        {
            GL.CreateTextures(TextureTarget.Texture2D, 1, out RendererID.GetRef()._id);
            GL.TextureStorage2D(RendererID.GetRef(), 1, SizedInternalFormat.Rgba32f, image.Width, image.Height);
            GL.TextureSubImage2D(RendererID.GetRef(), 0, 0, 0, image.Width, image.Height, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.GenerateTextureMipmap(RendererID.GetRef());
        }
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        
        Width = image.Width;
        Height = image.Height;
    }

    public OpenGLTexture2D(uint width, uint height)
    {
        DebugTimer.Start();
        Width = (int) width;
        Height = (int) height;
        if (Application.PlatformOS == OS.Mac)
        {
            RendererID.GetRef() = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, RendererID.GetRef());
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 
                0, PixelFormat.Rgba, PixelType.UnsignedByte, nint.Zero);
           // GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        else
        {
            GL.CreateTextures(TextureTarget.Texture2D, 1, out RendererID.GetRef()._id);
            GL.TextureStorage2D(RendererID.GetRef(), 1, SizedInternalFormat.Rgba32f, Width, Height);
            //GL.TextureSubImage2D(_rendererId, 0, 0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            //GL.GenerateTextureMipmap(_rendererId);
        }
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        DebugTimer.End();
    }
    public override void Bind(int slot = 0)
    {
        DebugTimer.Start("OpenGLTexture2D.Bind()");
        if (Application.PlatformOS == OS.Mac)
        {
            DebugLog.Assert(slot<=32, "Could not bind texture to slot {0}", slot);
            GL.ActiveTexture(_textureUnits[slot]);
            GL.BindTexture(TextureTarget.Texture2D, RendererID.GetRef());
            DebugTimer.End("OpenGLTexture2D.Bind()");
            return;
        }
        GL.BindTextureUnit(slot, RendererID.GetRef());
        DebugTimer.End("OpenGLTexture2D.Bind()");
        //
    }

    public override void SetData(byte[] data, uint size)
    {
        Log.AssertAndThrow(size == Width * Height * 4, "Data must fill the entire texture!");
        if (Application.PlatformOS == OS.Mac)
        {
            GL.BindTexture(TextureTarget.Texture2D, RendererID.GetRef());
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 
                0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return;
        }
        GL.TextureSubImage2D(RendererID.GetRef(), 0, 0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        GL.GenerateTextureMipmap(RendererID.GetRef());
    }

    public override void SetData(nint data, uint size)
    {
        Log.AssertAndThrow(size == Width * Height * 4, "Data must fill the entire texture!");
        if (Application.PlatformOS == OS.Mac)
        {
            GL.BindTexture(TextureTarget.Texture2D, RendererID.GetRef());
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 
                0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return;
        }
        GL.TextureSubImage2D(RendererID.GetRef(), 0, 0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        GL.GenerateTextureMipmap(RendererID.GetRef());
    }

    private static TextureUnit[] _textureUnits = Enum.GetValues<TextureUnit>();

    public override bool Equals(object? obj)
    {
        return obj is not null && ((OpenGLTexture2D) obj).RendererID.GetRef() == RendererID.GetRef();
    }

    protected override void Dispose(bool disposing)
    {
        GL.DeleteTexture(RendererID.GetRef());
    }
}