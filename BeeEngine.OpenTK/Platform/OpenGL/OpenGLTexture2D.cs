using BeeEngine.OpenTK.Renderer;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace BeeEngine.OpenTK.Platform.OpenGL;

public class OpenGLTexture2D: Texture2D
{
    private readonly string _path;
    private int _rendererId;
    public OpenGLTexture2D(string path)
    {
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
        GL.CreateTextures(TextureTarget.Texture2D, 1, out _rendererId);
        
        GL.TextureStorage2D(_rendererId, 1, SizedInternalFormat.Rgba32f, image.Width, image.Height);
        //GL.TextureParameter(_rendererId, TextureParameterName.TextureMinFilter, (int)FilterMode.Linear); 
        //GL.TextureParameterI(_rendererId, TextureParameterName.TextureMinFilter, ref filterMode);
        //GL.TextureParameter(_rendererId, TextureParameterName.TextureMagFilter, (int)FilterMode.Linear); 
        
        GL.TextureSubImage2D(_rendererId, 0, 0, 0, image.Width, image.Height, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        
        //GL.BindTexture(TextureTarget.Texture2D, _rendererId);
        //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 
         //   0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        Width = image.Width;
        Height = image.Height;
    }
    public override void Bind(int slot = 0)
    {
        GL.BindTextureUnit(slot, _rendererId);
        //GL.BindTexture(TextureTarget.Texture2D, _rendererId);
    }

    protected override void Dispose(bool disposing)
    {
        GL.DeleteTexture(_rendererId);
    }
}